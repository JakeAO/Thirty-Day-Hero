using System;
using System.Collections.Generic;
using Core.Actors;
using Core.Actors.Calamity;
using Core.Actors.Player;
using Core.Classes.Calamity;
using Core.Classes.Player;
using Core.Database;
using Core.Etc;
using Core.EventOptions;
using Core.Items;
using Core.Signals;
using Core.States.BaseClasses;
using Core.Wrappers;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States
{
    /// <summary>
    /// State in which new parties are formed from a selection of randomly generated player characters.
    /// <remarks>
    /// - Create an empty PartyData for the new party.
    /// - Pull a number of random Classes from the database.
    /// - Create an Actor for each of the Classes.
    /// - Present Actors to the player for selection:
    ///   - Selected Actors are either added or removed from the active party.
    /// - Once the party has enough Actors:
    ///   - Save the PartyData to JSON.
    ///   - Update PlayerData with new Party ID and save to JSON.
    ///   - Transition to GameHubState.
    /// </remarks>
    /// </summary>
    public class CreatePartyState : TDHStateBase
    {
        public const string CATEGORY_CONTINUE = "Continue";
        public const string CATEGORY_ADD = "Add Hero";
        public const string CATEGORY_REMOVE = "Remove Hero";
        
        public PartyDataWrapper PartyData { get; private set; }
        public PartyDataUpdatedSignal PartyUpdatedSignal { get; private set; }
        public IReadOnlyList<PlayerCharacter> UnassignedCharacterPool => _unassignedCharacterPool;

        private readonly List<PlayerCharacter> _unassignedCharacterPool = new List<PlayerCharacter>((int) Constants.CREATE_PARTY_POOL_SIZE);

        public override void OnEnter(IState fromState)
        {
            if (SharedContext.Get<PlayerDataWrapper>() == null)
                throw new ArgumentException("Entered CreatePartyState without an active Player profile!");
            if (SharedContext.Get<PartyDataWrapper>() != null)
                throw new ArgumentException("Entered CreatePartyState with an already active Party profile!");
        }

        public override void OnContent()
        {
            // Pull databases from context
            PlayerClassDatabase playerClassDatabase = SharedContext.Get<PlayerClassDatabase>();
            CalamityClassDatabase calamityClassDatabase = SharedContext.Get<CalamityClassDatabase>();

            // Pull signal from context (or add)
            if (SharedContext.TryGet(out PartyDataUpdatedSignal partyDataUpdatedSignal))
            {
                PartyUpdatedSignal = partyDataUpdatedSignal;
            }
            else
            {
                PartyUpdatedSignal = new PartyDataUpdatedSignal();
                SharedContext.Set(PartyUpdatedSignal);
            }

            // Generate new unique-ish partyId
            uint partyId = (uint) Guid.NewGuid().GetHashCode();

            // Create random pool of PlayerCharacters to select from
            _unassignedCharacterPool.AddRange(CreateRandomActorPool(partyId, playerClassDatabase));

            // Create random Calamity this party needs to fight
            CalamityCharacter calamityActor = CreateRandomCalamity(calamityClassDatabase);

            // Initialize the PartyData
            PartyData = new PartyDataWrapper(partyId, new PlayerCharacter[0], new IItem[0], calamityActor, PartyUpdatedSignal);

            SetupOptions();
        }

        private void SetupOptions()
        {
            // Pull/Create lists
            if (!_currentOptions.TryGetValue(CATEGORY_CONTINUE, out var continueList))
                _currentOptions[CATEGORY_CONTINUE] = continueList = new List<IEventOption>(1);
            if (!_currentOptions.TryGetValue(CATEGORY_REMOVE, out var removeList))
                _currentOptions[CATEGORY_REMOVE] = removeList = new List<IEventOption>((int) Constants.CREATE_PARTY_POOL_SIZE);
            if (!_currentOptions.TryGetValue(CATEGORY_ADD, out var addList))
                _currentOptions[CATEGORY_ADD] = addList = new List<IEventOption>((int) Constants.CREATE_PARTY_POOL_SIZE);

            // Clear lists
            continueList.Clear();
            removeList.Clear();
            addList.Clear();

            // Generate options
            bool canSubmit = PartyData.Characters.Count >= Constants.PARTY_SIZE_MIN &&
                             PartyData.Characters.Count <= Constants.PARTY_SIZE_MAX;
            continueList.Add(new EventOption(
                "Begin Adventure",
                SubmitParty,
                CATEGORY_CONTINUE,
                99,
                !canSubmit));

            foreach (PlayerCharacter character in PartyData.Characters)
            {
                uint characterId = character.Id;
                removeList.Add(new EventOption(
                    "Remove Hero",
                    () => RemoveActorById(characterId),
                    CATEGORY_REMOVE,
                    context: character));
            }

            foreach (PlayerCharacter character in _unassignedCharacterPool)
            {
                uint characterId = character.Id;
                addList.Add(new EventOption(
                    "Add Hero",
                    () => AddActorById(characterId),
                    CATEGORY_ADD,
                    context: character));
            }

            OptionsChangedSignal?.Fire(this);
        }

        private void AddActorById(uint actorId)
        {
            if (_unassignedCharacterPool.Find(x => x.Id == actorId) is PlayerCharacter actorInPool)
            {
                // Add to party, remove from pool.
                _unassignedCharacterPool.Remove(actorInPool);
                PartyData.Characters.Add(actorInPool);

                SetupOptions();
            }
        }

        private void RemoveActorById(uint actorId)
        {
            if (PartyData.Characters.Find(x => x.Id == actorId) is PlayerCharacter actorInParty)
            {
                // Remove from party, add to pool.
                PartyData.Characters.Remove(actorInParty);
                _unassignedCharacterPool.Add(actorInParty);
                
                SetupOptions();
            }
        }

        private void SubmitParty()
        {
            if (PartyData.Characters.Count >= Constants.PARTY_SIZE_MIN &&
                PartyData.Characters.Count <= Constants.PARTY_SIZE_MAX)
            {
                // Add Party to Context
                SharedContext.Set(PartyData);

                // Save Party Data
                SaveLoadHelper.SavePartyData(SharedContext);

                // Set Active Party, Save Player Data
                PlayerDataWrapper playerDataWrapper = SharedContext.Get<PlayerDataWrapper>();
                playerDataWrapper.SetActiveParty(PartyData.PartyId);
                SaveLoadHelper.SavePlayerData(SharedContext);

                // Transition to PreGameState
                SharedContext.Get<IStateMachine>().ChangeState<PreGameState>();
            }
        }

        private static IEnumerable<PlayerCharacter> CreateRandomActorPool(uint partyId, PlayerClassDatabase playerClassDatabase)
        {
            IReadOnlyCollection<IPlayerClass> randomClasses = playerClassDatabase.GetRandom(Constants.CREATE_PARTY_POOL_SIZE);
            foreach (IPlayerClass randomClass in randomClasses)
            {
                PlayerCharacter newCharacter = ActorUtil.CreatePlayer(
                    partyId,
                    randomClass,
                    1u);
                yield return newCharacter;
            }
        }

        private static CalamityCharacter CreateRandomCalamity(CalamityClassDatabase calamityClassDatabase)
        {
            ICalamityClass randomClass = calamityClassDatabase.GetRandom();
            CalamityCharacter newCharacter = ActorUtil.CreateCalamity(
                (uint) Guid.NewGuid().GetHashCode(),
                randomClass,
                Constants.CALAMITY_LEVEL);
            return newCharacter;
        }
    }
}
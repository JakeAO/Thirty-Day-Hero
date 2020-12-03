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
        }

        public override IEnumerable<IEventOption> GetOptions()
        {
            bool canSubmit = PartyData.Characters.Count >= Constants.PARTY_SIZE_MIN &&
                             PartyData.Characters.Count <= Constants.PARTY_SIZE_MAX;
            yield return new EventOption(
                "Begin Adventure",
                SubmitParty,
                priority: 99,
                disabled: !canSubmit);

            foreach (PlayerCharacter character in PartyData.Characters)
            {
                uint characterId = character.Id;
                yield return new EventOption(
                    "Remove Hero",
                    () => RemoveActorById(characterId),
                    "Active Party",
                    context: character);
            }

            foreach (PlayerCharacter character in _unassignedCharacterPool)
            {
                uint characterId = character.Id;
                yield return new EventOption(
                    "Add Hero",
                    () => AddActorById(characterId),
                    "Available Heroes",
                    context: character);
            }
        }

        private void AddActorById(uint actorId)
        {
            if (_unassignedCharacterPool.Find(x => x.Id == actorId) is PlayerCharacter actorInPool)
            {
                // Add to party, remove from pool.
                _unassignedCharacterPool.Remove(actorInPool);
                PartyData.Characters.Add(actorInPool);

                OptionsChangedSignal?.Fire(this);
            }
        }

        private void RemoveActorById(uint actorId)
        {
            if (PartyData.Characters.Find(x => x.Id == actorId) is PlayerCharacter actorInParty)
            {
                // Remove from party, add to pool.
                PartyData.Characters.Remove(actorInParty);
                _unassignedCharacterPool.Add(actorInParty);
                
                OptionsChangedSignal?.Fire(this);
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
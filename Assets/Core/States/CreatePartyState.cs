using System;
using System.Collections.Generic;
using System.IO;
using Core.Actors;
using Core.Actors.Enemy;
using Core.Actors.Player;
using Core.Classes.Enemy;
using Core.Classes.Player;
using Core.Database;
using Core.Etc;
using Core.Items;
using Core.Signals;
using Core.Wrappers;
using Newtonsoft.Json;
using SadPumpkin.Util.Context;
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
    public class CreatePartyState : IState
    {
        public PartyDataWrapper PartyData { get; private set; }
        public PartyDataUpdatedSignal PartyUpdatedSignal { get; private set; }
        public IReadOnlyList<PlayerCharacter> UnassignedCharacterPool => _unassignedCharacterPool;

        private readonly List<PlayerCharacter> _unassignedCharacterPool = new List<PlayerCharacter>((int) Constants.CREATE_PARTY_POOL_SIZE);

        private IContext _context;

        public void PerformSetup(IContext context, IState previousState)
        {
            _context = context;

            if (context.Get<PlayerDataWrapper>() == null)
                throw new ArgumentException("Entered CreatePartyState without an active Player profile!");
            if (context.Get<PartyDataWrapper>() != null)
                throw new ArgumentException("Entered CreatePartyState with an already active Party profile!");
        }

        public void PerformContent(IContext context)
        {
            // Pull databases from context
            PlayerClassDatabase playerClassDatabase = context.Get<PlayerClassDatabase>();
            CalamityClassDatabase calamityClassDatabase = context.Get<CalamityClassDatabase>();

            // Pull signal from context (or add)
            if (context.TryGet(out PartyDataUpdatedSignal partyDataUpdatedSignal))
            {
                PartyUpdatedSignal = partyDataUpdatedSignal;
            }
            else
            {
                PartyUpdatedSignal = new PartyDataUpdatedSignal();
                context.Set(PartyUpdatedSignal);
            }

            // Generate new unique-ish partyId
            uint partyId = (uint) Guid.NewGuid().GetHashCode();

            // Create random pool of PlayerCharacters to select from
            _unassignedCharacterPool.AddRange(CreateRandomActorPool(partyId, playerClassDatabase));

            // Create random Calamity this party needs to fight
            EnemyCharacter calamityActor = CreateRandomCalamity(calamityClassDatabase);

            // Initialize the PartyData
            PartyData = new PartyDataWrapper(partyId, new PlayerCharacter[0], new IItem[0], calamityActor, PartyUpdatedSignal);
        }

        public void PerformTeardown(IContext context, IState nextState)
        {

        }

        public void SelectActorById(uint actorId)
        {
            if (PartyData.Characters.Find(x => x.Id == actorId) is PlayerCharacter actorInParty)
            {
                // Remove from party, add to pool.
                PartyData.Characters.Remove(actorInParty);
                _unassignedCharacterPool.Add(actorInParty);
            }
            else if (_unassignedCharacterPool.Find(x => x.Id == actorId) is PlayerCharacter actorInPool)
            {
                // Add to party, remove from pool.
                _unassignedCharacterPool.Remove(actorInPool);
                PartyData.Characters.Add(actorInPool);
            }
        }

        public void SubmitParty()
        {
            if (PartyData.Characters.Count >= Constants.PARTY_SIZE_MIN &&
                PartyData.Characters.Count <= Constants.PARTY_SIZE_MAX)
            {
                JsonSerializerSettings jsonSettings = _context.Get<JsonSerializerSettings>();

                // Set Active Party, Save Player Data
                PathUtility pathUtility = _context.Get<PathUtility>();
                PlayerDataWrapper playerDataWrapper = _context.Get<PlayerDataWrapper>();
                playerDataWrapper.SetActiveParty(PartyData.PartyId);
                File.WriteAllText(pathUtility.GetPlayerDataPath(), JsonConvert.SerializeObject(playerDataWrapper, jsonSettings));

                // Add Party to Context
                _context.Set(PartyData);

                // Save Party Data
                string partyFilePath = pathUtility.GetPartyDataPath(PartyData.PartyId);
                Directory.CreateDirectory(Path.GetDirectoryName(partyFilePath));
                File.WriteAllText(partyFilePath, JsonConvert.SerializeObject(PartyData, jsonSettings));

                // Transition to PreGameState
                _context.Get<IStateMachine>().ChangeState<PreGameState>();
            }
        }

        private static IEnumerable<PlayerCharacter> CreateRandomActorPool(uint partyId, PlayerClassDatabase playerClassDatabase)
        {
            for (int i = 0; i < Constants.CREATE_PARTY_POOL_SIZE; i++)
            {
                IPlayerClass randomClass = playerClassDatabase.GetRandom();
                PlayerCharacter newCharacter = ActorUtil.CreatePlayer(
                    partyId,
                    randomClass,
                    1u);
                yield return newCharacter;
            }
        }

        private static EnemyCharacter CreateRandomCalamity(CalamityClassDatabase calamityClassDatabase)
        {
            IEnemyClass randomClass = calamityClassDatabase.GetRandom();
            EnemyCharacter newCharacter = ActorUtil.CreateEnemy(
                (uint) Guid.NewGuid().GetHashCode(),
                randomClass,
                Constants.CALAMITY_LEVEL);
            return newCharacter;
        }
    }
}
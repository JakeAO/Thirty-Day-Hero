using System.Collections.Generic;
using Core.Actors.Enemy;
using Core.Actors.Player;
using Core.Etc;
using Core.Items;
using Core.Signals;
using SadPumpkin.Util.Signals;

namespace Core.Wrappers
{
    public class PartyDataWrapper
    {
        public static string DataPath(string userId, uint partyId) => $"{userId}/{partyId}.json";

        public readonly ISignal UpdateSignal;

        private uint _partyId;
        private uint _day;
        private TimeOfDay _time;
        private uint _gold;
        private List<PlayerCharacter> _characters;
        private List<IItem> _inventory;
        private EnemyCharacter _calamity;
        private bool _calamityDefeated;

        public uint PartyId
        {
            get => _partyId;
            set
            {
                _partyId = value;
                UpdateSignal.Fire();
            }
        }

        public uint Day
        {
            get => _day;
            set
            {
                _day = value;
                UpdateSignal.Fire();
            }
        }

        public TimeOfDay Time
        {
            get => _time;
            set
            {
                _time = value;
                UpdateSignal.Fire();
            }
        }

        public uint Gold
        {
            get => _gold;
            set
            {
                _gold = value;
                UpdateSignal.Fire();
            }
        }

        public List<PlayerCharacter> Characters
        {
            get => _characters;
            set
            {
                _characters = value;
                UpdateSignal.Fire();
            }
        }

        public List<IItem> Inventory
        {
            get => _inventory;
            set
            {
                _inventory = value;
                UpdateSignal.Fire();
            }
        }

        public EnemyCharacter Calamity
        {
            get => _calamity;
            set
            {
                _calamity = value;
                UpdateSignal.Fire();
            }
        }

        public bool CalamityDefeated
        {
            get => _calamityDefeated;
            set
            {
                _calamityDefeated = value;
                UpdateSignal.Fire();
            }
        }

        public string GetDataPath(string userId) => DataPath(userId, _partyId);

        public PartyDataWrapper()
            : this(0u, null, null, null, null)
        {
        }

        public PartyDataWrapper(
            uint partyId,
            IReadOnlyCollection<PlayerCharacter> characters,
            IReadOnlyCollection<IItem> inventory,
            EnemyCharacter calamity,
            ISignal updateSignal)
        {
            UpdateSignal = updateSignal ?? new PartyDataUpdatedSignal();

            _partyId = partyId;
            _day = 0;
            _time = TimeOfDay.Morning;
            _gold = 100;
            _characters = characters != null
                ? new List<PlayerCharacter>(characters)
                : new List<PlayerCharacter>();
            _inventory = inventory != null
                ? new List<IItem>(inventory)
                : new List<IItem>();
            _calamity = calamity;
        }

        public void IncrementTime()
        {
            if (Time == TimeOfDay.Night)
            {
                Time = TimeOfDay.Morning;
                Day++;
            }
            else
            {
                Time++;
            }
        }
    }
}
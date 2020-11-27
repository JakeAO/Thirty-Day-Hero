using System;
using System.Collections.Generic;
using Core.Signals;
using SadPumpkin.Util.Signals;

namespace Core.Wrappers
{
    public class PlayerDataWrapper
    {
        public static string DataPath(string userId) => $"players/{userId}.json";

        public readonly ISignal Updated;

        private uint _activePartyId;
        private List<uint> _pastPartyIds;

        public uint ActivePartyId
        {
            get => _activePartyId;
            set
            {
                _activePartyId = value;
                Updated.Fire();
            }
        }

        public List<uint> PastPartyIds
        {
            get => _pastPartyIds;
            set
            {
                _pastPartyIds = value;
                Updated.Fire();
            }
        }

        public string GetDataPath(string userId) => DataPath(userId);

        public PlayerDataWrapper()
            : this(0u, null, null)
        {
        }

        public PlayerDataWrapper(
            uint activePartyId,
            IReadOnlyCollection<uint> pastParties,
            ISignal updatedSignal)
        {
            Updated = updatedSignal ?? new PlayerDataUpdatedSignal();

            _activePartyId = activePartyId;
            _pastPartyIds = pastParties != null
                ? new List<uint>(pastParties)
                : new List<uint>();
        }

        public void SetActiveParty(uint partyId)
        {
            if (_activePartyId != 0u)
            {
                _pastPartyIds.Add(_activePartyId);
            }

            _activePartyId = partyId;

            Updated.Fire();
        }
    }
}
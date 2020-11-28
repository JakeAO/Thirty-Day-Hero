using System.IO;
using Core.Wrappers;
using Newtonsoft.Json;
using SadPumpkin.Util.Context;

namespace Core.Etc
{
    public static class SaveLoadHelper
    {
        public static void SavePlayerData(IContext context)
        {
            JsonSerializerSettings jsonSettings = context.Get<JsonSerializerSettings>();
            PathUtility pathUtility = context.Get<PathUtility>();
            PlayerDataWrapper playerDataWrapper = context.Get<PlayerDataWrapper>();

            string playerDataPath = pathUtility.GetPlayerDataPath();
            string playerDataDirectory = Path.GetDirectoryName(playerDataPath);

            Directory.CreateDirectory(playerDataDirectory);

            File.WriteAllText(
                playerDataPath,
                JsonConvert.SerializeObject(playerDataWrapper, jsonSettings));
        }

        public static void SavePartyData(IContext context)
        {
            JsonSerializerSettings jsonSettings = context.Get<JsonSerializerSettings>();
            PathUtility pathUtility = context.Get<PathUtility>();
            PartyDataWrapper partyDataWrapper = context.Get<PartyDataWrapper>();

            string partyDataPath = pathUtility.GetPartyDataPath(partyDataWrapper.PartyId);
            string partyDataDirectory = Path.GetDirectoryName(partyDataPath);

            Directory.CreateDirectory(partyDataDirectory);

            File.WriteAllText(
                partyDataPath,
                JsonConvert.SerializeObject(partyDataWrapper, jsonSettings));
        }

        public static PlayerDataWrapper LoadPlayerData(IContext context)
        {
            JsonSerializerSettings jsonSettings = context.Get<JsonSerializerSettings>();
            PathUtility pathUtility = context.Get<PathUtility>();

            string playerDataPath = pathUtility.GetPlayerDataPath();
            if (File.Exists(playerDataPath))
            {
                return JsonConvert.DeserializeObject<PlayerDataWrapper>(
                    File.ReadAllText(playerDataPath),
                    jsonSettings);
            }

            return null;
        }

        public static PartyDataWrapper LoadPartyData(IContext context)
        {
            JsonSerializerSettings jsonSettings = context.Get<JsonSerializerSettings>();
            PathUtility pathUtility = context.Get<PathUtility>();
            PlayerDataWrapper playerDataWrapper = context.Get<PlayerDataWrapper>();

            uint activePartyId = playerDataWrapper.ActivePartyId;
            if (activePartyId == 0u)
                return null;
            
            string partyDataPath = pathUtility.GetPartyDataPath(activePartyId);
            if (File.Exists(partyDataPath))
            {
                return JsonConvert.DeserializeObject<PartyDataWrapper>(
                    File.ReadAllText(partyDataPath),
                    jsonSettings);
            }

            return null;
        }
    }
}
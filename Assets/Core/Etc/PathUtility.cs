using System.IO;
using Core.Wrappers;

namespace Core.Etc
{
    public class PathUtility
    {
        public readonly string ActivePlayerId;

        public readonly string SavePath;

        public readonly string AbilityPath;
        public readonly string PlayerClassPath;
        public readonly string EnemyClassPath;
        public readonly string CalamityClassPath;
        public readonly string EnemyGroupPath;
        public readonly string ItemDefinitionPath;
        public readonly string WeaponDefinitionPath;
        public readonly string ArmorDefinitionPath;

        public PathUtility(
            string activePlayerId,
            string savePath,
            string abilityPath,
            string playerClassPath,
            string enemyClassPath,
            string calamityClassPath,
            string enemyGroupPath,
            string itemDefinitionPath,
            string weaponDefinitionPath,
            string armorDefinitionPath)
        {
            ActivePlayerId = activePlayerId;
            SavePath = savePath;
            AbilityPath = abilityPath;
            PlayerClassPath = playerClassPath;
            EnemyClassPath = enemyClassPath;
            CalamityClassPath = calamityClassPath;
            EnemyGroupPath = enemyGroupPath;
            ItemDefinitionPath = itemDefinitionPath;
            WeaponDefinitionPath = weaponDefinitionPath;
            ArmorDefinitionPath = armorDefinitionPath;
        }

        public string GetPlayerDataPath()
        {
            return Path.Combine(SavePath, PlayerDataWrapper.DataPath(ActivePlayerId));
        }

        public string GetPartyDataPath(uint partyId)
        {
            return Path.Combine(SavePath, PartyDataWrapper.DataPath(ActivePlayerId, partyId));
        }
    }
}
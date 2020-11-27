namespace Core.Etc
{
    public class PathUtility
    {
        public readonly string AbilityPath;
        public readonly string PlayerClassPath;
        public readonly string EnemyClassPath;
        public readonly string CalamityClassPath;
        public readonly string EnemyGroupPath;
        public readonly string ItemDefinitionPath;
        public readonly string WeaponDefinitionPath;
        public readonly string ArmorDefinitionPath;

        public PathUtility(
            string abilityPath,
            string playerClassPath,
            string enemyClassPath,
            string calamityClassPath,
            string enemyGroupPath,
            string itemDefinitionPath,
            string weaponDefinitionPath,
            string armorDefinitionPath)
        {
            AbilityPath = abilityPath;
            PlayerClassPath = playerClassPath;
            EnemyClassPath = enemyClassPath;
            CalamityClassPath = calamityClassPath;
            EnemyGroupPath = enemyGroupPath;
            ItemDefinitionPath = itemDefinitionPath;
            WeaponDefinitionPath = weaponDefinitionPath;
            ArmorDefinitionPath = armorDefinitionPath;
        }
    }
}
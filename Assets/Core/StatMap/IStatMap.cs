using Core.Etc;
using SadPumpkin.Util.CombatEngine;

namespace Core.StatMap
{
    public interface IStatMap : ICopyable<IStatMap>
    {
        uint this[StatType statType] { get; }
        
        uint GetStat(StatType statType);

        void ModifyStat(StatType statType, int change);
    }
}
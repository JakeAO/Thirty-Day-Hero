using SadPumpkin.Util.CombatEngine;

namespace Core.Database
{
    public interface IDatabase<T> where T : IIdTracked
    {
        T GetRandom();
        T GetSpecific(uint id);
    }
}
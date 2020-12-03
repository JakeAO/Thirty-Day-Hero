using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine;

namespace Core.Database
{
    public interface IRandomItemProvider<T> where T : IIdTracked
    {
        T GetRandom();
        IReadOnlyCollection<T> GetRandom(uint count);
    }
}
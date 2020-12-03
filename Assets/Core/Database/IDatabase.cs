using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine;

namespace Core.Database
{
    public interface IDatabase<T> where T : IIdTracked
    {
        T GetSpecific(uint id);
        IEnumerable<T> EnumerateAll();
    }
}
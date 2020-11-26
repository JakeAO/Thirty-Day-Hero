using System;

namespace Core.StatMap
{
    public interface IStatMapIncrementor
    {
        IStatMap Increment(IStatMap statMap, Random random);
    }
}
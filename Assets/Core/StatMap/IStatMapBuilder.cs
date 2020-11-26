using System;

namespace Core.StatMap
{
    public interface IStatMapBuilder
    {
        IStatMap Generate(Random random);
    }
}
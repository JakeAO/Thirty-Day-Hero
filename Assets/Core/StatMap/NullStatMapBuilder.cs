using System;

namespace Core.StatMap
{
    public class NullStatMapBuilder : IStatMapBuilder
    {
        public static readonly NullStatMapBuilder Instance = new NullStatMapBuilder();

        public IStatMap Generate(Random random)
        {
            return new StatMap();
        }
    }
}
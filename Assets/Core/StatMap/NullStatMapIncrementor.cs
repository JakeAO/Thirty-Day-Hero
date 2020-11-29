using System;

namespace Core.StatMap
{
    public class NullStatMapIncrementor : IStatMapIncrementor
    {
        public static readonly NullStatMapIncrementor Instance = new NullStatMapIncrementor();

        private NullStatMapIncrementor()
        {
            
        }
        
        public IStatMap Increment(IStatMap statMap, Random random)
        {
            return statMap;
        }
    }
}
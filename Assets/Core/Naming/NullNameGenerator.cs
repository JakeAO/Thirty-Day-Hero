namespace Core.Naming
{
    public class NullNameGenerator : INameGenerator
    {
        public static readonly NullNameGenerator Instance = new NullNameGenerator();

        private NullNameGenerator()
        {
            
        }
        
        public string GetName()
        {
            return string.Empty;
        }
    }
}
using System;
using System.Collections.Generic;

namespace Core.Naming
{
    public class ListNameGenerator : INameGenerator
    {
        private static readonly Random _random = new Random();

        public List<string> Names { get; set; }

        public ListNameGenerator()
        {
            Names = new List<string>();
        }

        public ListNameGenerator(IReadOnlyCollection<string> list)
        {
            Names = new List<string>(list);
        }

        public ListNameGenerator(params string[] list)
        {
            Names = new List<string>(list);
        }

        public string GetName()
        {
            if (Names.Count == 0)
                return string.Empty;

            int index = _random.Next(Names.Count);
            return Names[index];
        }
    }
}
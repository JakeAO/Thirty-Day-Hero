using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.Naming
{
    public class TxtNameGenerator : INameGenerator
    {
        private readonly Random _random = new Random();
        private readonly IReadOnlyList<string> _nameEntries;

        public TxtNameGenerator(string txtFilePath)
            : this(File.ReadAllLines(txtFilePath))
        {
        }

        public TxtNameGenerator(IReadOnlyCollection<string> splitTxtFileContents)
        {
            _nameEntries = new List<string>(splitTxtFileContents
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim()));
        }

        public string GetName()
        {
            int index = _random.Next(_nameEntries.Count);
            return _nameEntries[index];
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.Naming
{
    public class FileNameGenerator : INameGenerator
    {
        public string FilePath { get; set; }

        private readonly Random _random = new Random();
        private IReadOnlyList<string> _nameEntries;

        public string GetName()
        {
            if (_nameEntries == null)
            {
                if (!File.Exists(FilePath))
                    return string.Empty;

                _nameEntries = File
                    .ReadAllLines(FilePath)
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToArray();
            }

            if (_nameEntries.Count == 0)
            {
                return string.Empty;
            }

            int index = _random.Next(_nameEntries.Count);
            return _nameEntries[index];
        }
    }
}
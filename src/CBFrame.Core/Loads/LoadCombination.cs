using System.Collections.Generic;

namespace CBFrame.Core.Loads
{
    public sealed class LoadCombination
    {
        public int Id { get; }
        public string Name { get; set; }
        public LoadCombinationType Type { get; set; }

        public List<LoadCombinationTerm> Terms { get; } = new();

        public bool IsActive { get; set; } = true;

        public LoadCombination(int id, string name, LoadCombinationType type)
        {
            Id = id;
            Name = name;
            Type = type;
        }

        public override string ToString() => $"{Id}: {Name} ({Type})";
    }
}

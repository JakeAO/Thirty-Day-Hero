using System;

namespace Core.EventOptions
{
    public class EventOption : IEventOption
    {
        public bool Disabled { get; }
        public uint Priority { get; }
        public string Text { get; }
        public string Category { get; }
        public object Context { get; }
        public Action SelectOption { get; }

        public EventOption(
            string text,
            Action selectOption,
            string category = null,
            uint priority = 0u,
            bool disabled = false,
            object context = default)
        {
            Disabled = disabled;
            Priority = priority;
            Text = text;
            Category = category;
            Context = context;
            SelectOption = selectOption;
        }

        public int CompareTo(IEventOption other)
        {
            if (ReferenceEquals(this, other))
                return 0;

            if (ReferenceEquals(null, other))
                return 1;

            var categoryComparison = string.Compare(Category, other.Category, StringComparison.Ordinal);
            if (categoryComparison != 0)
                return categoryComparison;

            var priorityComparison = Priority.CompareTo(other.Priority);
            if (priorityComparison != 0)
                return priorityComparison;

            var textComparison = string.Compare(Text, other.Text, StringComparison.Ordinal);
            if (textComparison != 0)
                return textComparison;

            var disabledComparison = Disabled.CompareTo(other.Disabled);
            if (disabledComparison != 0)
                return disabledComparison;

            return 0;
        }
    }
}
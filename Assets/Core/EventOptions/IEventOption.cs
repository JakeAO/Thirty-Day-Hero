using System;

namespace Core.EventOptions
{
    public interface IEventOption : IComparable<IEventOption>
    {
        bool Disabled { get; }
        uint Priority { get; }
        string Text { get; }
        string Category { get; }
        object Context { get; }
        Action SelectOption { get; }
    }
}
namespace DogLifecycle.Events
{
    using System;
    using CrossContext;
    using Honeycomb.Events;

    public class DogIsNamed : Event
    {
        public Earbrand Earbrand { get; private set; }
        public string Name { get; private set; }
        public DateTime DateNamed { get; private set; }
        public StaffMember NameCheckedBy { get; private set; }

        public DogIsNamed(Earbrand earbrand, string name, DateTime dateNamed, StaffMember nameCheckedBy)
        {
            Earbrand = earbrand;
            Name = name;
            DateNamed = dateNamed;
            NameCheckedBy = nameCheckedBy;
        }
    }
}
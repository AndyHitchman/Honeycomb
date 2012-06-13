namespace DogLifecycle.Events
{
    using CrossContext;
    using Honeycomb.Events;

    public class DogIsRegistered : Event
    {
        public DogIsRegistered(Earbrand earbrand, Microchip microchip, Colour colour)
        {
            Earbrand = earbrand;
            Microchip = microchip;
            Colour = colour;
        }

        public Earbrand Earbrand { get; private set; }
        public Microchip Microchip { get; private set; }
        public Colour Colour { get; private set; }
    }
}
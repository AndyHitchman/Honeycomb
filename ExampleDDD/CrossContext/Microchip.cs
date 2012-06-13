namespace CrossContext
{
    public class Microchip
    {
        public Microchip(string number)
        {
            Number = number;
        }

        public string Number { get; private set; }
    }
}
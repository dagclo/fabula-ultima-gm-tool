namespace FabulaUltimaNpc
{
    public readonly struct Die
    {
        public Die(int value) : this()
        {
            Sides = value;
        }

        public int Sides { get; }

        private const int MIN_SIDES = 6;
        private const int MAX_SIDES = 12;
        public static Die Downgrade(Die die)
        {
            if(die.Sides <= MIN_SIDES) return new Die(MIN_SIDES);
            return new Die(die.Sides - 2);
        }

        public static Die Upgrade(Die die)
        {
            if (die.Sides >= MAX_SIDES) return new Die(MAX_SIDES);
            return new Die(die.Sides + 2);
        }

        public override string ToString()
        {
            return $"D{Sides}";
        }
    }
}

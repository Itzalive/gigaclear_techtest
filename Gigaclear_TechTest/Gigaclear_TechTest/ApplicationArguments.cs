namespace Gigaclear_TechTest
{
    partial class Program
    {
        public class ApplicationArguments
        {
            public int Cabinet { get; set; }
            public int Chamber { get; set; }
            public int Pot { get; set; }
            public int TrenchRoad { get; set; }
            public int TrenchVerge { get; set; }
            public string Filename { get; set; } = "";
            public RateCard RateCard => new RateCard() { Cabinet = Cabinet, Chamber = Chamber, Pot = Pot, TrenchRoad = TrenchRoad, TrenchVerge = TrenchVerge };
        }
    }
}

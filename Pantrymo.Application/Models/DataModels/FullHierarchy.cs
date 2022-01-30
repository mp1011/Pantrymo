namespace Pantrymo.Application.Models
{
    public class FullHierarchy
    {
        public string? Level1 { get; set; }
        public string? Level2 { get; set; }
        public string? Level3 { get; set; } 
        public string? Level4 { get; set; } 
        public string? Level5 { get; set; } 
        public string? Level6 { get; set; }
        public string? Level7 { get; set; } 

        public string? GetLevel(int level)
        {
            switch (level)
            {
                case 1: return Level1;
                case 2: return Level2;
                case 3: return Level3;
                case 4: return Level4;
                case 5: return Level5;
                case 6: return Level6;
                case 7: return Level7;
                default:
                    throw new ArgumentException();
            }
        }
    }
}

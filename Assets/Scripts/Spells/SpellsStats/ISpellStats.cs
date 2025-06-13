using System.Drawing;

public interface ISpellStats
{
    float size { get; } 
    int Damage { get; }
    int Heal { get; }
    float FireApplication { get; }
    bool AppliesWater { get; }
}

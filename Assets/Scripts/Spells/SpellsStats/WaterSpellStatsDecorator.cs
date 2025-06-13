using System;

public class WaterSpellStatsDecorator : SpellStatsDecorator
{
    public WaterSpellStatsDecorator(float size, bool appliesWater)
        : base(size, 0, 0, 0.0f, appliesWater)
    {
    }

    // Only when Spray
    public override float GetSize() => _decoratedSpellStats?.GetSize() + _size * 0.5f ?? + _size;

    public override IPrototype Clone() => new WaterSpellStatsDecorator(_size, _appliesWater);
}

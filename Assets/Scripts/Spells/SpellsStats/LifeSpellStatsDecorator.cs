using System;

public class LifeSpellStatsDecorator : SpellStatsDecorator
{
    public LifeSpellStatsDecorator(float size, int heal)
        : base(size, 0, heal, 0.0f, false)
    {
    }
    
    public override int GetHeal() => _decoratedSpellStats?.GetHeal() + _heal ?? + _heal;
    
    public override IPrototype Clone() => new LifeSpellStatsDecorator(_size, _heal);
}

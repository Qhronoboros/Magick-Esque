using System;

public class FireSpellStatsDecorator : SpellStatsDecorator
{
    public FireSpellStatsDecorator(float size, float fireApplication)
        : base(size, 0, 0, fireApplication, false)
    {
    }
    
    // Only when Spray
    public override float GetSize() => _decoratedSpellStats?.GetSize() + _size * 0.5f ?? + _size;
    public override float GetFireApplication() => _decoratedSpellStats?.GetFireApplication() + _fireApplication ?? + _fireApplication;
    
    public override IPrototype Clone() => new FireSpellStatsDecorator(_size, _fireApplication);
}

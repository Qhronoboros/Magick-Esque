using System;

public class ArcaneSpellStatsDecorator : SpellStatsDecorator
{
    public ArcaneSpellStatsDecorator(float size, int damage)
        : base(size, damage, 0, 0.0f, false)
    {
    }
    
    public override int GetDamage() => _decoratedSpellStats?.GetDamage() + (int)(_damage * 0.6f) ?? + _damage;
    
    public override IPrototype Clone() => new ArcaneSpellStatsDecorator(_size, _damage);
}

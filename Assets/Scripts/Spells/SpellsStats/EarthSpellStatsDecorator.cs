using System;

public class EarthSpellStatsDecorator : SpellStatsDecorator
{
    public EarthSpellStatsDecorator(float size, int damage)
        : base(size, damage, 0, 0.0f, false)
    {
    }

    public override float GetSize() => _decoratedSpellStats?.GetSize() + _size * 0.5f ?? + _size;
    public override int GetDamage() => _decoratedSpellStats?.GetDamage() + (int)(_damage * 0.5f) ?? + _damage;

    public override IPrototype Clone() => new EarthSpellStatsDecorator(_size, _damage);
}

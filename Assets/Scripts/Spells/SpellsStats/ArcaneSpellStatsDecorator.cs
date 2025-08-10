using System;
using UnityEngine;

public class ArcaneSpellStatsDecorator : SpellStatsDecorator
{
    public ArcaneSpellStatsDecorator(float size, int damage, Color color)
        : base(size, damage, 0, 0.0f, false, color, null)
    {
    }
    
    public override float GetSize() => _decoratedSpellStats?.GetSize() + _size * 0.75f ?? _size;
    public override int GetDamage() => _decoratedSpellStats?.GetDamage() + (int)(_damage * 0.6f) ?? _damage;
    
    public override IPrototype Clone() => new ArcaneSpellStatsDecorator(_size, _damage, _color);
}

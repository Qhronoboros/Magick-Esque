using System;
using UnityEngine;

public class LifeSpellStatsDecorator : SpellStatsDecorator
{
    public LifeSpellStatsDecorator(string name, float size, int heal, Color color)
        : base(name, size, 0, heal, 0.0f, false, color, null)
    {
    }
    
    public override float GetSize() => _decoratedSpellStats?.GetSize() + _size * 0.75f ?? _size;
    public override int GetHeal() => _decoratedSpellStats?.GetHeal() + _heal ?? _heal;
    
    public override IPrototype Clone() => new LifeSpellStatsDecorator(_name, _size, _heal, _color);
}

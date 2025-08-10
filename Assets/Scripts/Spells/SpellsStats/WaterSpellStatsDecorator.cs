using System;
using UnityEngine;

public class WaterSpellStatsDecorator : SpellStatsDecorator
{
    public WaterSpellStatsDecorator(string name, float size, bool appliesWater, Color color, IElementEffect elementEffect)
        : base(name, size, 0, 0, 0.0f, appliesWater, color, elementEffect)
    {
    }

    public override float GetSize() => _decoratedSpellStats?.GetSize() + _size * 0.3f ?? _size;

    public override IPrototype Clone() => new WaterSpellStatsDecorator(_name, _size, _appliesWater, _color, _elementEffect.Clone() as IElementEffect);
}

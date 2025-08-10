using System;
using UnityEngine;

public class FireSpellStatsDecorator : SpellStatsDecorator
{
    public FireSpellStatsDecorator(float size, float fireApplication, Color color, IElementEffect elementEffect)
        : base(size, 0, 0, fireApplication, false, color, elementEffect)
    {
    }
    
    public override float GetSize() => _decoratedSpellStats?.GetSize() + _size * 0.3f ?? _size;
    public override float GetFireApplication() => _decoratedSpellStats?.GetFireApplication() + _fireApplication ?? _fireApplication;
    
    public override IPrototype Clone() => new FireSpellStatsDecorator(_size, _fireApplication, _color, _elementEffect.Clone() as IElementEffect);
}

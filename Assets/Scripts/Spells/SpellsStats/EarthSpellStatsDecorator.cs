using UnityEngine;

public class EarthSpellStatsDecorator : SpellStatsDecorator
{
    public EarthSpellStatsDecorator(string name, float size, int damage, Color color)
        : base(name, size, damage, 0, 0.0f, false, color, null)
    {
    }

    public override float GetSize() => _decoratedSpellStats?.GetSize() + _size * 0.5f ?? _size;
    public override int GetDamage() => _decoratedSpellStats?.GetDamage() + (int)(_damage * 0.5f) ?? _damage;

    public override IPrototype Clone() => new EarthSpellStatsDecorator(_name, _size, _damage, _color);
}

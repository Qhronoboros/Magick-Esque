using System;

public abstract class SpellStatsDecorator : ISpellStats
{
    protected float _size;
    protected int _damage;
    protected int _heal;
    protected float _fireApplication;
    protected bool _appliesWater;

    protected ISpellStats _decoratedSpellStats;
    
    protected SpellStatsDecorator(float size, int damage, int heal,
        float fireApplication, bool appliesWater)
    {
        _size = size;
        _damage = damage;
        _heal = heal;
        _fireApplication = fireApplication;
        _appliesWater = appliesWater;
    }
    
    public ISpellStats Decorate(ISpellStats spellStats)
    {
        _decoratedSpellStats = spellStats;
        return this;
    }

    public virtual float GetSize() => _decoratedSpellStats?.GetSize() + _size ?? + _size;
    public virtual int GetDamage() => _decoratedSpellStats?.GetDamage() + _damage ?? + _damage;
    public virtual int GetHeal() => _decoratedSpellStats?.GetHeal() + _heal ?? + _heal;
    public virtual float GetFireApplication() => _decoratedSpellStats?.GetFireApplication() + _fireApplication ?? + _fireApplication;
    public virtual bool GetAppliesWater() => _decoratedSpellStats?.GetAppliesWater() == true || _appliesWater;
    
    public abstract IPrototype Clone();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterElementEffect : IElementEffect
{
    public IEntity Actor { get; private set; }
    public ElementType ElementType { get; private set; }
    public Color EffectColor { get; private set; }

    public WaterElementEffect(Color effectColor)
    {
        ElementType = ElementType.WATER;
        EffectColor = effectColor;
        Actor = null;
    }

    public void SetActor(IEntity actor) => Actor = actor;

    public void OnEffectHit(ISpellObject spellObject)
    {
        if (Actor == null) return;
        IMovable movable = Actor as IMovable;
        if (movable == null) return;

        movable.Physics.ApplyForce(spellObject.AttachedGameObject.transform.forward * 2.0f);
    }

    public void ProcessElementChange(ISpellStats spellStats)
    {
        IHealth health = Actor as IHealth;
        
        switch (spellStats.GetElementEffect().ElementType)
        {
            case ElementType.WATER:
                break;
            case ElementType.FIRE:
                // Do extra damage
                if (health != null)
                    health.TakeDamage(50, Color.gray);
                break;
        }
    }

    public void Update(float deltaTime)
    {
    }
    
    public void FixedUpdate(float deltaTime)
    {
    }

    public IPrototype Clone()
    {
        return new WaterElementEffect(EffectColor);
    }
}

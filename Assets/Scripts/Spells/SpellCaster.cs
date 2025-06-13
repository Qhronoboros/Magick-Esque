using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster
{
    private readonly int _elementLimit = 5;
    private List<IElement> _spellElements = new List<IElement>();

    public SpellCaster()
    {
        
    }
    
    public void AddElement(IElement element)
    {
        // Check if there are any elemental clashes
        // Disable both 


        if (_spellElements.Count >= _elementLimit) return;
    }
    
    public void CastSpell()
    {
        
    }
    
    public void StopCastSpell()
    {
        
    }
}


/*
(main damage, healing, fire application, applies water, size)

- Earth
Projectile      priority 2
High single instance of damage
Hold LMB to charge projectile
More charge -> more damage and faster projectile 
More Earth -> bigger projectile -> slower, but also more damage

- Life
Beam            priority 1
Hold LMB to continuously use Beam
Heals
Adds healing to spells
More Life -> more healing

- Arcane
Beam            priority 1
Hold LMB to continuously use Beam
Does damage
Adds more damage to spells
More Arcane -> more damage

- Water
Spray           priority 0
Hold LMB to continuously use Spray
Does not do damage
Applies wet effect on entities
Burn + wet effect = deals an extra instance of high damage
Pushes entities away
When adding more water to spray, bigger spray

- Fire
Spray           priority 0
Does not do damage
Hold LMB to continuously use Spray
applies burn effect on entities
Burn effect does damage on entities per tick
More fire -> more burn damage per tick
When adding more fire to spray, bigger spray

Clashes:
Life + arcane
*/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellCaster
{
    private readonly int _elementSpellLimit = 5;
    private List<ISpell> _spellElements = new List<ISpell>();

    private ISpell _currentSpell = null;

    public void AddElementSpell(ISpell addSpell)
    {
        foreach(ISpell spell in _spellElements)
        {
            // If priority is the same and it is not the same element
            // Remove both spells elements
            if (spell.Priority == addSpell.Priority
                && spell.ActorSpellStatsDecorator.GetType() != addSpell.ActorSpellStatsDecorator.GetType())
            {
                _spellElements.Remove(spell);
                Debug.Log($"Element Count: {_spellElements.Count}");
                
                return;
            }
        }

        if (_spellElements.Count >= _elementSpellLimit) return;
        
        _spellElements.Add(addSpell);

        SortListOnPriority();
        
        Debug.Log($"Element Count: {_spellElements.Count}");
    }
    
    // Sorts the SpellElements list based on priority (high to low)
    public void SortListOnPriority()
    {
        _spellElements = _spellElements.OrderByDescending(x => x.Priority).ToList();
    }
    
    public void CastSpell(GameObject actor)
    {
        if (_spellElements.Count == 0) return;

        _currentSpell = _spellElements[0];
        
        for (int i = 1; i < _spellElements.Count; i++)
        {
            _currentSpell.ActorSpellStatsDecorator = _spellElements[i].ActorSpellStatsDecorator.Decorate(_currentSpell.ActorSpellStatsDecorator) as SpellStatsDecorator;
        }

        _currentSpell.StartCastingSpell(actor);

        _spellElements.Clear();
    }
    
    public void StopCastSpell(GameObject actor)
    {
        if (_currentSpell == null) return;
            _currentSpell.StopCastingSpell(actor);
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
water + fire
*/
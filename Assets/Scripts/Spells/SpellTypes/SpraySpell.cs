using UnityEngine;

public class SpraySpell : ISpell
{
    private Spray _spray;

    public GameObject SpellPrefab { get; private set; }
    public int Priority { get; private set; }
    
    public SpellStatsDecorator ActorSpellStatsDecorator { get; set; }

    public SpraySpell(GameObject spellPrefab, SpellStatsDecorator spellStats)
    {
        SpellPrefab = spellPrefab;
        ActorSpellStatsDecorator = spellStats;
        Priority = 0;
    }
    
    public void StartCastingSpell(GameObject actor)
    {
        GameObject _spellObject = GameObject.Instantiate(SpellPrefab);
        
        _spray = new Spray(_spellObject, actor, ActorSpellStatsDecorator);

        GameManager.instance.spellObjects.Add(_spray);
    }
    
    public void StopCastingSpell(GameObject actor)
    {
        _spray.DestroySelf();
    }

    public IPrototype Clone() => new SpraySpell(SpellPrefab, ActorSpellStatsDecorator.Clone() as SpellStatsDecorator);
}

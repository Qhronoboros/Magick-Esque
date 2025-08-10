using UnityEngine;

public class SpraySpell : ISpell
{
    private Spray _spray;

    private GameObject _sprayParticlePrefab;
    
    public GameObject SpellPrefab { get; private set; }
    public int Priority { get; private set; }
    
    public SpellStatsDecorator ActorSpellStatsDecorator { get; set; }

    public SpraySpell(GameObject spellPrefab, GameObject sprayParticlePrefab, SpellStatsDecorator spellStats)
    {
        SpellPrefab = spellPrefab;
        _sprayParticlePrefab = sprayParticlePrefab;
        ActorSpellStatsDecorator = spellStats;
        Priority = 0;
    }
    
    public void StartCastingSpell(GameObject actor)
    {
        GameObject spellObject = GameObject.Instantiate(SpellPrefab);
        
        _spray = new Spray(_sprayParticlePrefab, spellObject, actor, ActorSpellStatsDecorator);

        GameManager.instance.spellObjects.Add(_spray);
    }
    
    public void StopCastingSpell(GameObject actor)
    {
        _spray.sprayStopped = true;
    }

    public IPrototype Clone() => new SpraySpell(SpellPrefab, _sprayParticlePrefab, ActorSpellStatsDecorator.Clone() as SpellStatsDecorator);
}

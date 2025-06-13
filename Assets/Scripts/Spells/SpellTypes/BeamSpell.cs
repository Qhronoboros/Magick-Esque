using UnityEngine;

public class BeamSpell : ISpell
{
    private Beam _beam;

    public GameObject SpellPrefab { get; private set; }
    public int Priority { get; private set; }
    
    public SpellStatsDecorator ActorSpellStatsDecorator { get; set; }

    public BeamSpell(GameObject spellPrefab, SpellStatsDecorator spellStats)
    {
        SpellPrefab = spellPrefab;
        ActorSpellStatsDecorator = spellStats;
        Priority = 1;
    }

    public void StartCastingSpell(GameObject actor)
    {
        GameObject _spellObject = GameObject.Instantiate(SpellPrefab);
        
        _beam = new Beam(_spellObject, actor, ActorSpellStatsDecorator);

        GameManager.instance.spellObjects.Add(_beam);
    }
    
    public void StopCastingSpell(GameObject actor)
    {
        _beam.DestroySelf();
    }

    public IPrototype Clone() => new BeamSpell(SpellPrefab, ActorSpellStatsDecorator.Clone() as SpellStatsDecorator);
}

using UnityEngine;

public class ProjectileSpell : ISpell
{
    private Projectile _projectile;

    public GameObject SpellPrefab { get; private set; }
    public int Priority { get; private set; }
    
    public SpellStatsDecorator ActorSpellStatsDecorator { get; set; }

    public ProjectileSpell(GameObject spellPrefab, SpellStatsDecorator spellStats)
    {
        SpellPrefab = spellPrefab;
        ActorSpellStatsDecorator = spellStats;
        Priority = 2;
    }

    public void StartCastingSpell(GameObject actor)
    {
        
    }
    
    public void StopCastingSpell(GameObject actor)
    {
        GameObject _spellObject = GameObject.Instantiate(SpellPrefab);
        
        _projectile = new Projectile(_spellObject, actor, ActorSpellStatsDecorator);

        GameManager.instance.spellObjects.Add(_projectile);
    }

    public IPrototype Clone() => new ProjectileSpell(SpellPrefab, ActorSpellStatsDecorator.Clone() as SpellStatsDecorator);
}

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
        GameObject spellObject = GameObject.Instantiate(SpellPrefab);
        
        if (!HelperFunctions.GetPhysicsComponentsFromGameObject(spellObject, out Rigidbody rigidbody, out Collider collider))
            return;

        SimplePhysics simplePhysics = new SimplePhysics(rigidbody, collider);
        _projectile = new Projectile(spellObject, actor, ActorSpellStatsDecorator, simplePhysics);

        GameManager.instance.spellObjects.Add(_projectile);
    }
    
    public void StopCastingSpell(GameObject actor)
    {
        _projectile.LaunchProjectile();
    }

    public IPrototype Clone() => new ProjectileSpell(SpellPrefab, ActorSpellStatsDecorator.Clone() as SpellStatsDecorator);
}

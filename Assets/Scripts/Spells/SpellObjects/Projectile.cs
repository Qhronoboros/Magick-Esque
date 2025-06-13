using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : ISpellObject
{
    public GameObject AttachedGameObject { get; private set; }
    public GameObject Actor { get; private set; }
    public ISpellStats ActorSpellStats { get; private set; }

    public Projectile(GameObject attachedGameObject, GameObject actor, ISpellStats actorSpellStats)
    {
        AttachedGameObject = attachedGameObject;
        Actor = actor;
        ActorSpellStats = actorSpellStats;
    }

    public void Update(float deltaTime)
    {
    }

    public void FixedUpdate(float deltaTime)
    {
        // Move
    }
    
    public void DestroySelf()
    {
        GameObject.Destroy(AttachedGameObject);
    }
}

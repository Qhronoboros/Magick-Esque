using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spray : ISpellObject
{
    public GameObject AttachedGameObject { get; private set; }
    public GameObject Actor { get; private set; }
    public ISpellStats ActorSpellStats { get; private set; }

    public Spray(GameObject attachedGameObject, GameObject actor, ISpellStats actorSpellStats)
    {
        AttachedGameObject = attachedGameObject;
        Actor = actor;
        ActorSpellStats = actorSpellStats;

        float size = ActorSpellStats.GetSize();
        attachedGameObject.transform.localScale = new Vector3(size * 2.0f, size, size);
    }

    public void Update(float deltaTime)
    {
        AttachedGameObject.transform.position = Actor.transform.position + (Actor.transform.forward * ActorSpellStats.GetSize());
        AttachedGameObject.transform.eulerAngles = new Vector3(0.0f, Actor.transform.eulerAngles.y + 90.0f, AttachedGameObject.transform.eulerAngles.z); 
    }

    public void FixedUpdate(float deltaTime)
    {
    }
    
    public void DestroySelf()
    {
        GameObject.Destroy(AttachedGameObject);
    }
}

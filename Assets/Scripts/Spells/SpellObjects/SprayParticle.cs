using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayParticle : ISpellObject, IMovable
{
    private Timer _lifeTimer;
    
    public GameObject AttachedGameObject { get; private set; }
    public Material AttachedMaterial { get; private set; }
    public GameObject Actor { get; private set; }
    public ISpellStats ActorSpellStats { get; private set; }
    public IPhysics Physics { get; private set; }

    public SprayParticle(GameObject attachedGameObject, GameObject actor, ISpellStats actorSpellStats, IPhysics physics)
    {
        AttachedGameObject = attachedGameObject;
        
        HelperFunctions.GetMaterialFromGameObject(attachedGameObject, out Material material);
        AttachedMaterial = material;
        AttachedMaterial.color = actorSpellStats.GetColor();
        
        Actor = actor;
        ActorSpellStats = actorSpellStats;
        Physics = physics;

        // Set scale
        float size = ActorSpellStats.GetSize();
        AttachedGameObject.transform.localScale = new Vector3(size, size, size);

        // Apply force
        Quaternion rotationRandomizer = Quaternion.Euler(0.0f, Random.Range(-20.0f, 20.0f), 0.0f);
        Vector3 randomizedRotation = rotationRandomizer * AttachedGameObject.transform.forward;
        Physics.ApplyForce(randomizedRotation * ActorSpellStats.GetSize() * 30.0f);
        
        // Timer
        _lifeTimer = new Timer();
        _lifeTimer.OnTimerEnd += DestroySelf;
        _lifeTimer.timerDuration = 5.0f;
        _lifeTimer.StartTimer();
    }

    public void Update(float deltaTime)
    {
        if (_lifeTimer.isCounting)
            _lifeTimer.CountTimer(deltaTime);
    }

    public void FixedUpdate(float deltaTime)
    {
        if (HelperFunctions.IsTouchingPlatform(Physics.ActorCollider, GameManager.instance.platform.transform,
                AttachedGameObject.transform.lossyScale.y * 0.5f))
        {
            DestroySelf();
        }
    }
    
    public IEntity HandleCollision(List<IEntity> entities)
    {
        // Check Collision
        foreach(IEntity entity in entities)
        {
            if (Vector3.Distance(AttachedGameObject.transform.position, entity.AttachedGameObject.transform.position)
                <= AttachedGameObject.transform.lossyScale.x)
            {
                IElementStatus elementStatusEntity = entity as IElementStatus;
                if (elementStatusEntity != null)
                    elementStatusEntity.ApplyElement(this);
                    
                return entity;
            }
        }
        
        return null;
    }
    
    public void DestroySelf()
    {
        GameObject.Destroy(AttachedGameObject);
    }
}

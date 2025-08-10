using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spray : ISpellObject
{
    private Timer _spawnTimer;
    private List<IEntity> _spawnedParticles = new List<IEntity>();
    private GameObject _sprayParticlePrefab;
    
    public GameObject AttachedGameObject { get; private set; }
    public Material AttachedMaterial { get; private set; }
    public GameObject Actor { get; private set; }
    public ISpellStats ActorSpellStats { get; private set; }

    public bool sprayStopped = false;
    
    public Spray(GameObject sprayParticlePrefab, GameObject attachedGameObject, GameObject actor, ISpellStats actorSpellStats)
    {
        _sprayParticlePrefab = sprayParticlePrefab;
        AttachedGameObject = attachedGameObject;
        
        Actor = actor;
        ActorSpellStats = actorSpellStats;
        
        _spawnTimer = new Timer();
        _spawnTimer.EnableLooping();
        _spawnTimer.OnTimerStart += SpawnParticle;
        _spawnTimer.timerDuration = 0.03f;
        _spawnTimer.StartTimer();
    }

    // Sets the position in front of the player
    private void FollowPlayer()
    {
        AttachedGameObject.transform.position = Actor.transform.position;
        AttachedGameObject.transform.eulerAngles = new Vector3(0.0f, Actor.transform.eulerAngles.y + 90.0f, 0.0f);
    }

    public void Update(float deltaTime)
    {
        if (!sprayStopped && _spawnTimer.isCounting)
            _spawnTimer.CountTimer(deltaTime);
        
        HelperFunctions.CallMethodFromEntities(_spawnedParticles, new Action<IEntity>((entity) => entity.Update(Time.deltaTime)));
    }

    public void FixedUpdate(float deltaTime)
    {
        if (sprayStopped && _spawnedParticles.Count == 0)
        {
            DestroySelf();
            return;
        }

        // ! If adding enemy magicians, either only use entities that are not part of
        // ! the actor's faction when checking for collision
        // ! or go the easy way and just check collision for all entities with IHealth
         
        List<IEntity> enemies = GameManager.instance.enemies.ToList();
        
        for (int i = 0; i < _spawnedParticles.Count; i++)
        {
            SprayParticle particle = _spawnedParticles[i] as SprayParticle;
            
            if (particle.AttachedGameObject == null) 
            {
                _spawnedParticles.Remove(particle);
                i--;
                continue;
            }

            // Check collisions with enemies
            if (enemies.Count != 0)
            {
                IEntity hit = particle.HandleCollision(enemies);
                if (hit != null)
                    enemies.Remove(hit);
            }
                
            particle.FixedUpdate(deltaTime);
        }
    }
    
    private void SpawnParticle()
    {
        FollowPlayer();
        GameObject particleObject = GameObject.Instantiate(_sprayParticlePrefab, AttachedGameObject.transform.position, Actor.transform.rotation);
        
        if (!HelperFunctions.GetPhysicsComponentsFromGameObject(particleObject, out Rigidbody rigidbody, out Collider collider))
            return;

        SimplePhysics simplePhysics = new SimplePhysics(rigidbody, collider);
        SprayParticle sprayParticle = new SprayParticle(particleObject, Actor, ActorSpellStats, simplePhysics);
        
        _spawnedParticles.Add(sprayParticle);
    }
    
    public void DestroySelf()
    {
        foreach(IDestroyable particle in _spawnedParticles)
        {
            particle.DestroySelf();
        }
        GameObject.Destroy(AttachedGameObject);
    }
}

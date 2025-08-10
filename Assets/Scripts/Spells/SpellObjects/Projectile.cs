using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : ISpellObject, IMovable
{
    private Timer _lifeTimer;
    private Timer _launchTimer;
    private bool _launchMaxed;
    private bool _launched;
    private float _launchPower;
    
    private List<IEntity> _enemiesHit = new List<IEntity>();

    public GameObject AttachedGameObject { get; private set; }
    public Material AttachedMaterial { get; private set; }
    public GameObject Actor { get; private set; }
    public ISpellStats ActorSpellStats { get; private set; }
    public IPhysics Physics { get; private set; }

    public Projectile(GameObject attachedGameObject, GameObject actor, ISpellStats actorSpellStats, IPhysics physics)
    {
        AttachedGameObject = attachedGameObject;
        
        HelperFunctions.GetMaterialFromGameObject(attachedGameObject, out Material material);
        AttachedMaterial = material;
        AttachedMaterial.color = actorSpellStats.GetColor();
        
        Actor = actor;
        ActorSpellStats = actorSpellStats;
        Physics = physics;

        Physics.ActorRigidbody.mass = ActorSpellStats.GetSize() * 20.0f;
        Physics.ActorRigidbody.useGravity = false;
        
        // Set scale
        float size = ActorSpellStats.GetSize();
        AttachedGameObject.transform.localScale = new Vector3(size, size, size);
        
        // Timers
        _lifeTimer = new Timer();
        _lifeTimer.OnTimerEnd += DestroySelf;
        _lifeTimer.timerDuration = 5.0f;
        
        _launchTimer = new Timer();
        _launchTimer.OnTimerEnd += () => _launchMaxed = true;
        _launchTimer.timerDuration = 3.0f;
        _launchTimer.StartTimer();
    }

    // Sets the position in front of the player
    private void FollowPlayer()
    {
        Vector3 direction = Actor.transform.forward * (Actor.transform.lossyScale.x * 0.5f + AttachedGameObject.transform.lossyScale.x * 0.5f + 0.5f);
        AttachedGameObject.transform.position = Actor.transform.position + direction;
        AttachedGameObject.transform.eulerAngles = new Vector3(0.0f, Actor.transform.eulerAngles.y, 0.0f);
    }

    public void LaunchProjectile()
    {
        _launched = true;
        Physics.ActorRigidbody.useGravity = true;
        
        _launchPower = _launchMaxed ? 1.0f : _launchTimer.elapsedTime / _launchTimer.timerDuration;
        Physics.ApplyForce(AttachedGameObject.transform.forward * 75.0f * _launchPower);
        _lifeTimer.StartTimer();
    }

    public void Update(float deltaTime)
    {
        if (_lifeTimer.isCounting)
            _lifeTimer.CountTimer(deltaTime);
        
        if (_launchTimer.isCounting)
        _launchTimer.CountTimer(deltaTime);
    }

    public void FixedUpdate(float deltaTime)
    {
        if (!_launched)
        {
            FollowPlayer();
        }
        else
        {
            DetectEnemies();
        }
    }
    
    private void DetectEnemies()
    {
        List<IEntity> enemies = GameManager.instance.enemies.ToList();
        
        foreach(IEntity enemy in enemies)
        {
            if (_enemiesHit.Contains(enemy)) continue;

            // Check collisions with enemies
            Vector3 enemyPosition = enemy.AttachedGameObject.transform.position;
            Vector3 closestPoint = Physics.ActorCollider.ClosestPoint(enemyPosition);
            float distance = Vector3.Distance(closestPoint, enemyPosition);
            if (distance <= 0.5f) 
            {
                Debug.Log($"{distance} closestPoint");
                
                IElementStatus elementStatusEntity = enemy as IElementStatus;
                if (elementStatusEntity != null)
                    elementStatusEntity.ApplyElement(this);
                    
                IHealth healthEntity = enemy as IHealth;
                if (healthEntity != null)
                    healthEntity.TakeDamage((int)(ActorSpellStats.GetDamage() * _launchPower), ActorSpellStats.GetColor());
                    
                _enemiesHit.Add(enemy);
            }
        }
    }
    
    public void DestroySelf()
    {
        GameObject.Destroy(AttachedGameObject);
    }
}

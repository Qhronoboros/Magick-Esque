using System;
using UnityEngine;

public class CollisionEnemy : IEnemy
{
    private int _attackDamage;

    public event Action<int, int, Vector3, Color> OnHit;
    public event Action OnDeath;
    
    public GameObject AttachedGameObject { get; private set; }
    public Material AttachedMaterial { get; private set; }
    public int MaxHealth { get; private set; }
    public int Health { get; private set; }
    public IElementEffect CurrentElementEffect { get; private set; }
    
    public IPhysics Physics { get; private set; }
    
    public IEntity Target { get; private set; }
    public Collider TargetCollider { get; private set; }

    public CollisionEnemy(GameObject attachedGameObject, IPhysics locomotion, int attackDamage, int maxHealth, IEntity target = null)
    {
        AttachedGameObject = attachedGameObject;
        
        HelperFunctions.GetMaterialFromGameObject(attachedGameObject, out Material material);
        AttachedMaterial = material;
        
        Physics = locomotion;
        _attackDamage = attackDamage;
        MaxHealth = Health = maxHealth;

        OnHit += UIManager.instance.InstantiateDamageText;
        OnDeath += () => GameManager.instance.enemiesKilled++;

        SetTarget(target);
    }

    public void SetTarget(IEntity target)
    {
        Target = target;
        
        if (target == null) return;
        
        if (!target.AttachedGameObject.TryGetComponent(out Collider collider))
        {
            Debug.LogError("Target does not have a collider");
            return;
        }
        
        TargetCollider = collider;
    }

    public void Update(float deltaTime)
    {
        if (CurrentElementEffect == null) return;
        CurrentElementEffect.Update(deltaTime);
    }

    public void FixedUpdate(float deltaTime)
    {
        if (Target == null) return;
        Physics.ApplyForce((Target.AttachedGameObject.transform.position - AttachedGameObject.transform.position).normalized);

        if (IsCollidingTarget())
        {
            IHealth targetHealth = Target as IHealth;
            if (targetHealth == null) return;

            targetHealth.TakeDamage(_attackDamage, Color.red);
        }
        
        if (CurrentElementEffect == null) return;
        CurrentElementEffect.FixedUpdate(deltaTime);
    }

    private bool IsCollidingTarget()
    {
        if (Target == null || TargetCollider == null) return false;

        Vector3 actorPosition = AttachedGameObject.transform.position;
        Vector3 actorScale = AttachedGameObject.transform.lossyScale;

        Vector3 targetClosestPoint = TargetCollider.ClosestPoint(actorPosition);

        // ! Requires the actor to have the same values in the x and z dimensions of the scale
        return (targetClosestPoint - actorPosition).magnitude <= actorScale.x * 0.5f + 0.01f;
    }

    public void ApplyElement(ISpellObject spellObject)
    {
        if (CurrentElementEffect != null)
            CurrentElementEffect.ProcessElementChange(spellObject.ActorSpellStats);

        if (spellObject.ActorSpellStats.GetElementEffect() == null) return;
        
        CurrentElementEffect = spellObject.ActorSpellStats.GetElementEffect();
        CurrentElementEffect.SetActor(this);
        CurrentElementEffect.OnEffectHit(spellObject);
    }
    
    public void TakeDamage(int damage, Color damageColor)
    {
        if (AttachedGameObject == null) return;

        Debug.Log("Here");
        
        Health -= damage;
        OnHit?.Invoke(Health, damage, AttachedGameObject.transform.position, damageColor);
        
        if (Health <= 0)
        {
            Debug.Log($"{AttachedGameObject.name} died");
            OnDeath?.Invoke();
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        GameObject.Destroy(AttachedGameObject);
    }
    
    public IPrototype Clone()
    {
        // Debug.Log("Cloned CollisionEnemy");
        GameObject newGameObject = GameObject.Instantiate(AttachedGameObject);
        newGameObject.SetActive(true);

        HelperFunctions.GetComponentFromGameObject(newGameObject, out Rigidbody rigidbody);
        IPhysics newPhysics = Physics.Clone() as IPhysics;
        newPhysics.ActorRigidbody = rigidbody;

        return new CollisionEnemy(newGameObject, newPhysics, _attackDamage, MaxHealth, Target);
    }
}
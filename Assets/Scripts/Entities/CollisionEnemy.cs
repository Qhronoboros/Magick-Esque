using System;
using UnityEngine;

public class CollisionEnemy : IEnemy
{
    private float _attackDamage;

    public event Action<int, int> OnHit;
    public event Action OnDeath;
    
    public GameObject AttachedGameObject { get; private set; }
    public Material AttachedMaterial { get; private set; }
    public int MaxHealth { get; private set; }
    public int Health { get; private set; }
    
    public IPhysics Physics { get; private set; }
    
    public GameObject Target { get; private set; }
    public Collider TargetCollider { get; private set; }

    public CollisionEnemy(GameObject attachedGameObject, IPhysics locomotion, GameObject target = null)
    {
        AttachedGameObject = attachedGameObject;
        
        HelperFunctions.GetMaterialFromGameObject(attachedGameObject, out Material material);
        AttachedMaterial = material;
        
        Physics = locomotion;

        SetTarget(target);
    }

    public void SetTarget(GameObject target)
    {
        Target = target;
        
        if (target == null) return;
        
        if (!target.TryGetComponent(out Collider collider))
        {
            Debug.LogError("Target does not have a collider");
            return;
        }
        
        TargetCollider = collider;
    }

    public void Update(float deltaTime)
    {
    }

    public void FixedUpdate(float deltaTime)
    {
        if (Target == null) return;
        Physics.ApplyForce((Target.transform.position - AttachedGameObject.transform.position).normalized);

        Debug.Log(IsCollidingTarget());
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

    public void DestroySelf()
    {
        GameObject.Destroy(AttachedGameObject);
    }

    public void TakeDamage(int damage)
    {
    }
    
    public IPrototype Clone()
    {
        Debug.Log("Cloned CollisionEnemy");

        GameObject newGameObject = GameObject.Instantiate(AttachedGameObject);
        newGameObject.SetActive(true);

        HelperFunctions.GetComponentFromGameObject(newGameObject, out Rigidbody rigidbody);
        IPhysics newPhysics = Physics.Clone() as IPhysics;
        newPhysics.ActorRigidbody = rigidbody;

        return new CollisionEnemy(newGameObject, newPhysics, Target);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnemy : IEnemy
{
    private float _attackDamage;

    public event Action<int, int> OnHit;
    public event Action OnDeath;
    
    public GameObject AttachedGameObject { get; private set; }
    public int MaxHealth { get; private set; }
    public int Health { get; private set; }
    
    public ILocomotion ActorLocomotion { get; private set; }
    public GameObject Target { get; private set; }

    public CollisionEnemy(GameObject attachedGameObject, ILocomotion locomotion, GameObject target = null)
    {
        AttachedGameObject = attachedGameObject;
        ActorLocomotion = locomotion;
        Target = target;
    }

    public void SetTarget(GameObject target) => Target = target;

    public void Update(float deltaTime)
    {
    }

    public void FixedUpdate(float deltaTime)
    {
        if (Target == null) return;
        ActorLocomotion.ApplyForce((Target.transform.position - AttachedGameObject.transform.position).normalized);

        Debug.Log(IsCollidingTarget());
    }

    private bool IsCollidingTarget()
    {
        if (Target == null ||!Target.TryGetComponent(out Collider targetCollider)) return false;

        Vector3 actorPosition = AttachedGameObject.transform.position;
        Vector3 actorScale = AttachedGameObject.transform.lossyScale;

        Vector3 targetClosestPoint = targetCollider.ClosestPoint(actorPosition);

        // ! Requires the actor to have the same values in the x and z axis of the scale
        return (targetClosestPoint - actorPosition).magnitude <= actorScale.x * 0.5f + 0.01f;
    }

    public void DestroySelf()
    {
    }

    public void TakeDamage(int damage)
    {
    }
    
    public IPrototype Clone()
    {
        Debug.Log("Cloned CollisionEnemy");

        GameObject newGameObject = GameObject.Instantiate(AttachedGameObject);
        newGameObject.SetActive(true);

        return new CollisionEnemy(newGameObject, ActorLocomotion.Clone() as ILocomotion, Target);
    }
}

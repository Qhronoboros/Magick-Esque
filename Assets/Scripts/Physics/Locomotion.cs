using UnityEngine;

public class Locomotion : IPhysics
{
    private float _movementSpeed;
    private float _velocityMax;
    public Rigidbody ActorRigidbody { get; set; }
    public Collider ActorCollider { get; set; }

    public Locomotion(Rigidbody rigidbody, Collider collider, float movementSpeed, float velocityMax)
    {
        ActorRigidbody = rigidbody;
        ActorCollider = collider;
        _movementSpeed = movementSpeed;
        _velocityMax = velocityMax;
    }

    public void ApplyForce(Vector3 direction)
    {
        ActorRigidbody.AddForce(direction * _movementSpeed, ForceMode.VelocityChange);

        float magnitude = Mathf.Min(ActorRigidbody.velocity.magnitude, _velocityMax);
        ActorRigidbody.velocity = ActorRigidbody.velocity.normalized * magnitude;
    }

    public IPrototype Clone()
    {
        return new Locomotion(ActorRigidbody, ActorCollider, _movementSpeed, _velocityMax);
    }
}
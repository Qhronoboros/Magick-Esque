using UnityEngine;

public class Locomotion : ILocomotion
{
    private float _movementSpeed;
    private float _velocityMax;
    public Rigidbody ActorRigidbody { get; private set; }
    
    public Locomotion(Rigidbody rigidbody, float movementSpeed, float velocityMax)
    {
        ActorRigidbody = rigidbody;
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
        return new Locomotion(ActorRigidbody, _movementSpeed, _velocityMax);
    }
}
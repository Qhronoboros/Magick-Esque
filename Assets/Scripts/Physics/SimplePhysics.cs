using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePhysics : IPhysics
{
    public Rigidbody ActorRigidbody { get; set; }
    public Collider ActorCollider { get; set; }

    public SimplePhysics(Rigidbody rigidbody, Collider collider)
    {
        ActorRigidbody = rigidbody;
        ActorCollider = collider;
    }

    public void ApplyForce(Vector3 direction)
    {
        ActorRigidbody.AddForce(direction, ForceMode.VelocityChange);
    }

    public IPrototype Clone()
    {
        return new SimplePhysics(ActorRigidbody, ActorCollider);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPhysics : IPrototype
{
    Rigidbody ActorRigidbody { get; set; }
    Collider ActorCollider { get; set; }

    void ApplyForce(Vector3 direction);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILocomotion : IPrototype
{
    Rigidbody ActorRigidbody { get; }

    void ApplyForce(Vector3 direction);
}

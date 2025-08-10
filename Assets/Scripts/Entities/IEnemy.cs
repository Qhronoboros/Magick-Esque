using UnityEngine;

public interface IEnemy : IEntity, IHealth, IMovable, IPrototype
{
    GameObject Target { get; }
    Collider TargetCollider { get; }
}

using UnityEngine;

public interface IEnemy : IEntity, IHealth, IElementStatus, IMovable, IPrototype
{
    IEntity Target { get; }
    Collider TargetCollider { get; }
}

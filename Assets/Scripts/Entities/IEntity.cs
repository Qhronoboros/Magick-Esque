using UnityEngine;

public interface IEntity
{
    GameObject AttachedGameObject { get; }
    void Update(float deltaTime);
    void FixedUpdate(float deltaTime);
}
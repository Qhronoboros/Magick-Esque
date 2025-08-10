using UnityEngine;

public interface IEntity
{
    GameObject AttachedGameObject { get; }
    Material AttachedMaterial { get; }
    void Update(float deltaTime);
    void FixedUpdate(float deltaTime);
}
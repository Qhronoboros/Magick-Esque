using UnityEngine;

public interface IEntity : IUpdate
{
    GameObject AttachedGameObject { get; }
    Material AttachedMaterial { get; }
}
using UnityEngine;

public interface ISpellObject : IEntity, IDestroyable
{
    GameObject Actor { get; }
    ISpellStats ActorSpellStats { get; }
}

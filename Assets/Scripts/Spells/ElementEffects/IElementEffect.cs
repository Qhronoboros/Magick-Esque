
using UnityEngine;

public interface IElementEffect : IUpdate, IPrototype
{
    IEntity Actor { get; }
    ElementType ElementType { get; }
    Color EffectColor { get; }
    void SetActor(IEntity actor);
    void OnEffectHit(ISpellObject spellObject);
    void ProcessElementChange(ISpellStats spellStats);
}

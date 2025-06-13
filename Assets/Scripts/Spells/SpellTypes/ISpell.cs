using UnityEngine;

public interface ISpell : IPrototype
{
    GameObject SpellPrefab { get; }
    int Priority { get; }

    SpellStatsDecorator ActorSpellStatsDecorator { get; set;  }

    void StartCastingSpell(GameObject actor);
    void StopCastingSpell(GameObject actor);
}

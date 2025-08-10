using UnityEngine;

public interface ISpellStats : IPrototype
{
    float GetSize();
    int GetDamage();
    int GetHeal();
    float GetFireApplication();
    bool GetAppliesWater();
    Color GetColor();
    IElementEffect GetElementEffect();
}

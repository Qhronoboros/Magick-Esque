using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpraySpellStats : ISpellStats
{
    public float size { get; private set; }
    public int Damage { get; private set; }
    public int Heal { get; private set; }
    public float FireApplication { get; private set; }
    public bool AppliesWater { get; private set; }
}

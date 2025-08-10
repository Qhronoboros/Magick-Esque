using UnityEngine;

public interface IHealth : IDestroyable
{
    event System.Action<int, int, Vector3, Color> OnHit;
    event System.Action OnDeath;
    
    int MaxHealth { get; }
    int Health { get; }

    void TakeDamage(int damage, Color damageColor);
}
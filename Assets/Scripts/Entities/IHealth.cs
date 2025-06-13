public interface IHealth
{
    event System.Action<int, int> OnHit;
    event System.Action OnDeath;
    
    int MaxHealth { get; }
    int Health { get; }

    void TakeDamage(int damage);
    void DestroySelf();
}
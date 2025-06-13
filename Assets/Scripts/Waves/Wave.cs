using UnityEngine;

public class Wave
{
    private int _enemySpawned;
    private Timer _spawnTimer;
    
    public int collisionEnemyAmount;
    public int amountMinPerSpawn;
    public int amountMaxPerSpawn;
    public float spawnTimeMin;
    public float spawnTimeMax;

    public bool waveFinished = false;
    
    public int AmountPerSpawn 
    {
        get { return AmountPerSpawn; }
        set { AmountPerSpawn = Mathf.Min(value, collisionEnemyAmount); }
    }
    
    public void StartWave()
    {
        _spawnTimer = new Timer();
        _spawnTimer.OnTimerEnd += Spawn;

        StartSpawner();
    }
    
    public void StartSpawner()
    {
        AmountPerSpawn = Random.Range(amountMinPerSpawn, amountMaxPerSpawn + 1);
        _spawnTimer.timerDuration = Random.Range(spawnTimeMin, spawnTimeMax);
        _spawnTimer.StartTimer();
    }
    
    public void Update(float deltaTime)
    {
        if (_spawnTimer.isCounting)
            _spawnTimer.CountTimer(deltaTime);
    }
    
    public void Spawn()
    {
        // Spawn units
        

        if (_enemySpawned < collisionEnemyAmount)
        {
            StartSpawner();
        }
        else 
        {
            waveFinished = true;
        }
    }
}

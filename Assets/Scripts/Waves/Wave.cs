using UnityEngine;

public class Wave
{
    private Timer _spawnTimer;

    public IEnemy enemyPrototype;
    public int enemyAmount;
    public int amountMinPerSpawn;
    public int amountMaxPerSpawn;
    public float spawnTimeMin;
    public float spawnTimeMax;

    public bool waveSpawnFinished = false;
    
    private int _amountPerSpawn;
    public int AmountPerSpawn 
    {
        get { return _amountPerSpawn; }
        set { _amountPerSpawn = Mathf.Min(value, enemyAmount); }
    }
    
    public Wave()
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
    
    public bool Update(float deltaTime)
    {
        if (_spawnTimer.isCounting)
            _spawnTimer.CountTimer(deltaTime);

        return waveSpawnFinished;
    }
    
    public void Spawn()
    {
        // Spawn enemies
        for (int i = 0; i < AmountPerSpawn; i++)
        {
            enemyAmount--;
            IEnemy spawnedEnemy = enemyPrototype.Clone() as IEnemy;
            spawnedEnemy.AttachedGameObject.transform.position = WaveManager.instance.GetRandomSpawnLocation();
            GameManager.instance.enemies.Add(spawnedEnemy);
        }

        if (enemyAmount > 0)
        {
            StartSpawner();
        }
        else 
        {
            waveSpawnFinished = true;
        }
    }
}

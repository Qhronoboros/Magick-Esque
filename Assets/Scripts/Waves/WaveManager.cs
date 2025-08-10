using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager
{
    private Wave _currentWave;
    private Dictionary<System.Type, IEnemy> _enemyDictionary;
    private List<Vector3> _spawnLocations = new List<Vector3>();
    
    private int _enemyAmount;
    private int _enemyAmountIncrease;
    private int _amountPerSpawnMin;
    private int _amountPerSpawnMax;
    private int _amountPerSpawnIncrease;
    
    private int _waveCount;
    public int WaveCount
    {
        get { return _waveCount; }
        set
        {
            if (value != _waveCount)
            {
                _waveCount = value;
                OnWaveChanged?.Invoke(value);
            }
        }
    }
	public event System.Action<int> OnWaveChanged;
    
    // Singleton
	public static WaveManager instance { get; private set; }
    
    public WaveManager(Dictionary<System.Type, IEnemy> enemyDictionary, List<Vector3> spawnLocations, int startEnemyAmount,
        int enemyAmountWaveIncrease, int startAmountPerSpawnMin, int startAmountPerSpawnMax, int amountPerSpawnWaveIncrease)
    {
		if (instance == null)
			instance = this;
		else 
		{
			Debug.LogError($"A WaveManager already exists");
			return;
		}
		
        _enemyDictionary = enemyDictionary;
        _spawnLocations = spawnLocations;
        _enemyAmount = startEnemyAmount;
        _enemyAmountIncrease = enemyAmountWaveIncrease;
        _amountPerSpawnMin = startAmountPerSpawnMin;
        _amountPerSpawnMax = startAmountPerSpawnMax;
        _amountPerSpawnIncrease = amountPerSpawnWaveIncrease;
    }
    
    public void Update(float deltaTime)
    {
        if (_currentWave == null)
        {
            if (GameManager.instance.enemies.Count == 0)
                CreateWave();
        }
        else
        {
            // If current wave is done spawning, get ready for next wave
            if (_currentWave.Update(deltaTime))
                ResetWave();
        }
    }
    
    public void CreateWave()
    {
        WaveCount++;
        _currentWave = new WaveBuilder()
            .SetEnemyType(_enemyDictionary[typeof(CollisionEnemy)])
            .SetEnemyAmount(_enemyAmount)
            .SetAmountPerSpawn(_amountPerSpawnMin, _amountPerSpawnMax)
            .SetSpawnTime(3, 6)
            .Build();
    }
    
    public void ResetWave()
    {
        _currentWave = null;
        _enemyAmount += _enemyAmountIncrease;
        _amountPerSpawnMin += _amountPerSpawnIncrease;
        _amountPerSpawnMax += _amountPerSpawnIncrease;
    }
    
    public Vector3 GetRandomSpawnLocation() => _spawnLocations[Random.Range(0, _spawnLocations.Count)];

    public static void ResetSingleton() => instance = null;
}
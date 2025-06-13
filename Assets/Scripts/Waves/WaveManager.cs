using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager
{
    private Wave _currentWave;
    private List<Vector3> _spawnLocations = new List<Vector3>();
    
    public WaveManager(List<Vector3> spawnLocations)
    {
        _spawnLocations = spawnLocations;
    }
    
    public void Update(float deltaTime)
    {
        if (_currentWave == null || _currentWave.waveFinished)
        {
            _currentWave = new WaveBuilder()
                .SetCollisionEnemyAmount(5)
                .SetAmountPerSpawn(4, 6)
                .SetSpawnTime(3, 6)
                .Build();
        }

        _currentWave.Update(deltaTime);
    }
    
    private Vector3 GetRandomSpawnLocation() => _spawnLocations[Random.Range(0, _spawnLocations.Count)];
}

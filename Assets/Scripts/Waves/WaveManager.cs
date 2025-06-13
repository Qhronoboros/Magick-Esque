using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager
{
    private Wave _currentWave;
    
    
    
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
}

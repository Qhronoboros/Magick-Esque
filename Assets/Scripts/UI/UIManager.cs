using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager
{
    private TMP_Text _waveText;
    public void UpdateWaveText(int value) => _waveText.text = $"Wave: {value}";
    
    private TMP_Text _enemyCountText;
    public void UpdateEnemyCountText(int value) => _enemyCountText.text = $"Remaining\nEnemies: {value}";

    // Singleton
	public static UIManager instance { get; private set; }

    public UIManager(TMP_Text waveText, TMP_Text enemyCountText)
    {
		if (instance == null)
			instance = this;
		else 
		{
			Debug.LogError($"A UIManager already exists");
			return;
		}

        _waveText = waveText;
        _enemyCountText = enemyCountText;
    }
    
    public static void ResetSingleton() => instance = null;
}

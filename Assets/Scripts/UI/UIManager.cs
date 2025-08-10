using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class UIManager
{
    private GameObject _damageTextPrefab;

    private TMP_Text _waveText;
    public void UpdateWaveText(int count) => _waveText.text = $"Wave: {count}";
    
    private TMP_Text _enemyCountText;
    public void UpdateEnemyCountText(int count) => _enemyCountText.text = $"Remaining\nEnemies: {count}";

    private TMP_Text _playerHealthText;
    public void UpdatePlayerHealthText(int health, int maxHealth, Vector3 location, Color damageColor)
        => _playerHealthText.text = $"Health: {health}/{maxHealth}";

    // Singleton
	public static UIManager instance { get; private set; }

    public UIManager(GameObject damageTextPrefab, TMP_Text waveText, TMP_Text enemyCountText, TMP_Text playerHealthText)
    {
		if (instance == null)
			instance = this;
		else 
		{
			Debug.LogError($"A UIManager already exists");
			return;
		}

        _damageTextPrefab = damageTextPrefab;
        _waveText = waveText;
        _enemyCountText = enemyCountText;
        _playerHealthText = playerHealthText;
    }
    
    public void InstantiateDamageText(int health, int damage, Vector3 location, Color damageColor)
    {
        // ! Damage text not implemented
        // TMP_Text tmpText = GameObject.Instantiate(_damageTextPrefab, location, Quaternion.identity).GetComponent<TMP_Text>();
        // tmpText.color = damageColor;
        // tmpText.text = $"-{damage}";

        Debug.Log($"{damage} damage dealth");
    }
    
    public static void ResetSingleton() => instance = null;
}
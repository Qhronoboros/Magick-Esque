using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
	public GameObject platformPrefab;
	public GameObject playerObjectPrefab;
	public GameObject collisionEnemyPrefab;
    public GameObject projectilePrefab;
    public GameObject beamPrefab;
    public GameObject sprayPrefab;
    public GameObject sprayParticlePrefab;
    public GameObject damageTextPrefab;

    [Header("UI Objects")]
    public GameObject MainMenuObject;
    // Game UI
    public GameObject GameUIObject;
    public TMP_Text waveText;
    public TMP_Text enemyCountText;
    public TMP_Text playerHealthText;
    // Result Screen
    public GameObject ResultObject;
    public TMP_Text waveReachedText;
    public TMP_Text enemiesKilledText;

    [Header("Player Settings")]
    public float playerMovementSpeed;
    public float playerVelocityMax;

    [Header("Wave Settings")]
    public int startEnemyAmount;
    public int enemyAmountWaveIncrease;
    public int startAmountPerSpawnMin;
    public int startAmountPerSpawnMax;
    public int amountPerSpawnWaveIncrease;
    public List<Vector3> spawnLocations;
	
    [Header("Enemy Settings")]
    public float collsionEnemyMovementSpeed;
    public float collsionEnemyVelocityMax;


    [Header("Misc")]
    // For getting the mousePositionDelta
    public bool gameStarted;
    public LayerMask enemyLayer;
    public int enemiesKilled;
    public Mouse mouse;

    // Game Objects
    [NonSerialized] public GameObject platform;
	[NonSerialized] public PlayerController player;
    [NonSerialized] public List<IEntity> enemies = new List<IEntity>();
    [NonSerialized] public List<IEntity> spellObjects = new List<IEntity>();

    // Managers
    public UIManager uiManager;
    public WaveManager waveManager;
	
	// Singleton
	public static GameManager instance { get; private set;}

    private int _enemyCount;
    public int EnemyCount 
    {
        get { return _enemyCount; }
        set
        {
            if (value != _enemyCount)
            {
                _enemyCount = value;
                OnEnemyCountChanged?.Invoke(value);
            }
        }
    }
	public event System.Action<int> OnEnemyCountChanged;
	
	private void Awake()
	{
		if (instance == null)
			instance = this;
		else 
		{
			Debug.LogError($"A GameManager already exists, deleting self: {name}");
			Destroy(gameObject);
		}
	}

    public void StartGame()
    {
        ResultObject.SetActive(false);
        MainMenuObject.SetActive(false);
    
        SetupGame();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void GoToMenu()
    {
        GameUIObject.SetActive(false);
        ResultObject.SetActive(false);

        MainMenuObject.SetActive(true);
    }

    public void ShowResult()
    {
        gameStarted = false;
        ResultObject.SetActive(true);
        waveReachedText.text = $"Wave Reached: {waveManager.WaveCount}";
        enemiesKilledText.text = $"Enemies Killed: {enemiesKilled}";
        
        ResetGame();
    }

    private void SetupGame()
    {   
        // Create UIManager
        uiManager = new UIManager(damageTextPrefab, waveText, enemyCountText, playerHealthText);
    
        // Instantiate main Platform
        platform = Instantiate(platformPrefab);

        mouse = new Mouse();

        // Setup Player
        GameObject playerGameObject = Instantiate(playerObjectPrefab);

        // Locomotion
        if (!HelperFunctions.GetPhysicsComponentsFromGameObject(playerGameObject, out Rigidbody playerRigidbody, out Collider playerCollider))
            return;
            
        Locomotion playerLocomotion = new Locomotion(playerRigidbody, playerCollider, playerMovementSpeed, playerVelocityMax);
        SpellCaster playerSpellCaster = new SpellCaster();
		
        // CommandHandler
        CommandHandler keyInputHandler = new CommandHandler();
        keyInputHandler.BindCommand(() => Input.GetKey(KeyCode.A), new MoveDirectionCommand(playerLocomotion, Vector3.left));
        keyInputHandler.BindCommand(() => Input.GetKey(KeyCode.D), new MoveDirectionCommand(playerLocomotion, Vector3.right));
        keyInputHandler.BindCommand(() => Input.GetKey(KeyCode.W), new MoveDirectionCommand(playerLocomotion, Vector3.forward));
        keyInputHandler.BindCommand(() => Input.GetKey(KeyCode.S), new MoveDirectionCommand(playerLocomotion, Vector3.back));

        CommandHandler magicInputHandler = new CommandHandler();
        magicInputHandler.BindCommand(() => Input.GetMouseButtonDown(0), new CastSpellCommand(playerSpellCaster));
        magicInputHandler.BindCommand(() => Input.GetMouseButtonUp(0), new StopCastSpellCommand(playerSpellCaster));

        // J earth
        // I Life
        // K Arcane
        // O Water
        // L Fire

        ProjectileSpell earthSpell = new ProjectileSpell(projectilePrefab, new EarthSpellStatsDecorator(1.5f, 100, new Color(0.70f, 0.35f, 0.0f)));
        magicInputHandler.BindCommand(() => Input.GetKeyDown(KeyCode.J), new AddSpellCommand(playerSpellCaster, earthSpell));

        BeamSpell lifeSpell = new BeamSpell(beamPrefab, new LifeSpellStatsDecorator(0.5f, 5, new Color(0.54f, 0.91f, 0.54f)));
        magicInputHandler.BindCommand(() => Input.GetKeyDown(KeyCode.I), new AddSpellCommand(playerSpellCaster, lifeSpell));

        BeamSpell arcaneSpell = new BeamSpell(beamPrefab, new ArcaneSpellStatsDecorator(0.5f, 5, new Color(0.59f, 0.03f, 0.03f)));
        magicInputHandler.BindCommand(() => Input.GetKeyDown(KeyCode.K), new AddSpellCommand(playerSpellCaster, arcaneSpell));

        Color waterColor = new Color(0.22f, 0.59f, 0.83f);
        IElementEffect waterElementEffect = new WaterElementEffect(waterColor);
        SpraySpell waterSpell = new SpraySpell(sprayPrefab, sprayParticlePrefab, new WaterSpellStatsDecorator(0.75f, true, waterColor, waterElementEffect));
        magicInputHandler.BindCommand(() => Input.GetKeyDown(KeyCode.O), new AddSpellCommand(playerSpellCaster, waterSpell));

        Color fireColor = new Color(1.0f, 0.37f, 0.0f);
        IElementEffect fireElementEffect = new FireElementEffect(fireColor);
        SpraySpell fireSpell = new SpraySpell(sprayPrefab, sprayParticlePrefab, new FireSpellStatsDecorator(0.75f, 1, fireColor, fireElementEffect));
        magicInputHandler.BindCommand(() => Input.GetKeyDown(KeyCode.L), new AddSpellCommand(playerSpellCaster, fireSpell));
    
		// PlayerController
		player = new PlayerController(playerGameObject, keyInputHandler, magicInputHandler, playerLocomotion, playerSpellCaster, 300);
		
		// Setup Enemies
        GameObject collisionEnemyGameObject = Instantiate(collisionEnemyPrefab);
        collisionEnemyGameObject.SetActive(false);

        if (!HelperFunctions.GetPhysicsComponentsFromGameObject(collisionEnemyGameObject, out Rigidbody collisionEnemyRigidbody, out Collider collisionEnemyCollider))
            return;
        
        Locomotion collisionEnemyLocomotion = new Locomotion(collisionEnemyRigidbody, collisionEnemyCollider, collsionEnemyMovementSpeed, collsionEnemyVelocityMax);
        CollisionEnemy collisionEnemyScript = new CollisionEnemy(collisionEnemyGameObject, collisionEnemyLocomotion, 2, 200, player);

        Dictionary<Type, IEnemy> enemyDictionary = new Dictionary<Type, IEnemy>
        {
            { typeof(CollisionEnemy), collisionEnemyScript }
        };

        // WaveManager
        waveManager = new WaveManager(enemyDictionary, spawnLocations, startEnemyAmount, enemyAmountWaveIncrease,
            startAmountPerSpawnMin, startAmountPerSpawnMax, amountPerSpawnWaveIncrease);

        // Setup UI
        GameUIObject.SetActive(true);
        waveManager.OnWaveChanged += uiManager.UpdateWaveText;
        OnEnemyCountChanged += uiManager.UpdateEnemyCountText;
        uiManager.UpdatePlayerHealthText(player.Health, player.MaxHealth, Vector3.zero, new Color());
        
        gameStarted = true;
    }

    // Update Game Objects
    private void Update()
    {
        if (!gameStarted) return;
        mouse.Update();
        player.Update(Time.deltaTime);
        HelperFunctions.CallMethodFromEntities(enemies, new Action<IEntity>((entity) => entity.Update(Time.deltaTime)));
        HelperFunctions.CallMethodFromEntities(spellObjects, new Action<IEntity>((entity) => entity.Update(Time.deltaTime)));

        EnemyCount = enemies.Count;

        waveManager.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (!gameStarted) return;
        player.FixedUpdate(Time.deltaTime);
        HelperFunctions.CallMethodFromEntities(enemies, new Action<IEntity>((entity) => entity.FixedUpdate(Time.deltaTime)));
        HelperFunctions.CallMethodFromEntities(spellObjects, new Action<IEntity>((entity) => entity.FixedUpdate(Time.deltaTime)));
	}
    
    public void ResetGame()
    {
        foreach(IDestroyable enemy in enemies)
        {
            enemy.DestroySelf();
        }
        enemies.Clear();
        EnemyCount = 0;
        enemiesKilled = 0;
        
        foreach(IDestroyable spellObject in spellObjects)
        {
            spellObject.DestroySelf();
        }
        spellObjects.Clear();
        
        GameObject.Destroy(platform);
        player.DestroySelf();
        
        WaveManager.ResetSingleton();
        waveManager = null;
        UIManager.ResetSingleton();
        uiManager = null;
    }
}

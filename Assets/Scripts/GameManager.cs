using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
	public GameObject playerObjectPrefab;
	public GameObject collisionEnemyPrefab;
    public GameObject projectilePrefab;
    public GameObject BeamPrefab;
    public GameObject SprayPrefab;

    // For getting the mousePositionDelta
    public Mouse mouse;

    [Header("Game Objects")]
	public PlayerController player;
    public Dictionary<Type, IPrototype> prototypeDictionary = new Dictionary<Type, IPrototype>();
    public List<IEntity> enemies = new List<IEntity>();
    
    public List<IEntity> spellObjects = new List<IEntity>();


    [Header("Player Settings")]
    public float playerMovementSpeed;
    public float playerVelocityMax;

    [Header("Wave Settings")]
    public int waveAmount;
	
    [Header("Enemy Settings")]
    public float collsionEnemyMovementSpeed;
    public float collsionEnemyVelocityMax;
	
	// Singleton
	public static GameManager instance { get; private set;}
	
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

	// Setup game
    private void Start()
    {
        // // Setup Projectiles Prototypes
        // IProjectile bullet = new Projectile(Instantiate(bulletObjectPrefab), Vector3.zero, Vector3.zero, 1.0f,
        // 	MovementOverTime.CONSTANT, TargetFinding.NONE, false);
        // IProjectile rocket = new Projectile(Instantiate(rocketObjectPrefab), Vector3.zero, Vector3.zero, 1.0f,
        // 	MovementOverTime.ACCELERATED, TargetFinding.CLOSEST, false);

        // // Setup Attacker and Weapons
        // Attacker attacker = new Attacker(1);

        // attacker.AddWeapon(new Weapon(new WeaponStats(1.0f, 2.0f, 1), ShooterLaunch.STRAIGHT, bullet));
        // attacker.AddWeapon(new Weapon(new WeaponStats(2.0f, 0.5f, 1), ShooterLaunch.CIRCLE, rocket));

        // // Setup PowerUp Prototypes
        // List<IPowerUp> PowerUpPrototypes = new List<IPowerUp>()
        // {
        // 	new PowerUp(Instantiate(PowerUpObjectPrefab), new AttackMultiplierDecorator(0.5f),
        // 		ShooterLaunch.STRAIGHT, MovementOverTime.CONSTANT, TargetFinding.NONE),
        // 	new PowerUp(Instantiate(PowerUpObjectPrefab), new AttackSpeedDecorator(1.0f),
        // 		ShooterLaunch.STRAIGHT, MovementOverTime.CONSTANT, TargetFinding.NONE),
        // 	new PowerUp(Instantiate(PowerUpObjectPrefab), new ProjectileAmountDecorator(1),
        // 		ShooterLaunch.STRAIGHT, MovementOverTime.CONSTANT, TargetFinding.NONE),

        // 	new PowerUp(Instantiate(PowerUpObjectPrefab), new AttackMultiplierDecorator(1.0f),
        // 		ShooterLaunch.CIRCLE, MovementOverTime.ACCELERATED, TargetFinding.CLOSEST),
        // 	new PowerUp(Instantiate(PowerUpObjectPrefab), new AttackSpeedDecorator(0.5f),
        // 		ShooterLaunch.CIRCLE, MovementOverTime.ACCELERATED, TargetFinding.CLOSEST),
        // 	new PowerUp(Instantiate(PowerUpObjectPrefab), new ProjectileAmountDecorator(1),
        // 		ShooterLaunch.CIRCLE, MovementOverTime.ACCELERATED, TargetFinding.CLOSEST)
        // };

        mouse = new Mouse();

        // Setup Player
        GameObject playerGameObject = Instantiate(playerObjectPrefab);

        // Locomotion
        if (!GetComponentFromGameObject(playerGameObject, out Rigidbody playerRigidbody)) return;
        
        Locomotion playerLocomotion = new Locomotion(playerRigidbody, playerMovementSpeed, playerVelocityMax);
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

        magicInputHandler.BindCommand(() => Input.GetKeyDown(KeyCode.J), new AddSpellCommand(playerSpellCaster, new ProjectileSpell(projectilePrefab, new EarthSpellStatsDecorator(1.0f, 5))));
        magicInputHandler.BindCommand(() => Input.GetKeyDown(KeyCode.I), new AddSpellCommand(playerSpellCaster, new BeamSpell(BeamPrefab, new LifeSpellStatsDecorator(10.0f, 5))));
        magicInputHandler.BindCommand(() => Input.GetKeyDown(KeyCode.K), new AddSpellCommand(playerSpellCaster, new BeamSpell(BeamPrefab, new ArcaneSpellStatsDecorator(10.0f, 5))));
        magicInputHandler.BindCommand(() => Input.GetKeyDown(KeyCode.O), new AddSpellCommand(playerSpellCaster, new SpraySpell(SprayPrefab, new WaterSpellStatsDecorator(2.0f, true))));
        magicInputHandler.BindCommand(() => Input.GetKeyDown(KeyCode.L), new AddSpellCommand(playerSpellCaster, new SpraySpell(SprayPrefab, new FireSpellStatsDecorator(2.0f, 1))));
    
		// PlayerController
		player = new PlayerController(playerGameObject, keyInputHandler, magicInputHandler, playerLocomotion, playerSpellCaster);
		
		
		// Setup Enemies
        GameObject collisionEnemyGameObject = Instantiate(collisionEnemyPrefab);
        collisionEnemyGameObject.SetActive(false);

        if (!GetComponentFromGameObject(collisionEnemyGameObject, out Rigidbody collisionEnemyRigidbody)) return;

        Locomotion collisionEnemyLocomotion = new Locomotion(collisionEnemyRigidbody, collsionEnemyMovementSpeed, collsionEnemyVelocityMax);
        CollisionEnemy collisionEnemyScript = new CollisionEnemy(collisionEnemyGameObject, collisionEnemyLocomotion, playerGameObject);

        prototypeDictionary.Add(typeof(CollisionEnemy), collisionEnemyScript);
        
        
        // WaveBuilder
        
        
        
    }

    // Update Game Objects
    private void Update()
    {
        mouse.Update();
        player.Update(Time.deltaTime);
        CallMethodFromEntities(enemies, new Action<IEntity>((entity) => entity.Update(Time.deltaTime)));
        CallMethodFromEntities(spellObjects, new Action<IEntity>((entity) => entity.Update(Time.deltaTime)));
    }

    private void FixedUpdate()
    {
        player.FixedUpdate(Time.deltaTime);
        CallMethodFromEntities(enemies, new Action<IEntity>((entity) => entity.FixedUpdate(Time.deltaTime)));
        CallMethodFromEntities(spellObjects, new Action<IEntity>((entity) => entity.FixedUpdate(Time.deltaTime)));
	}
	
    private void CallMethodFromEntities(List<IEntity> entities, Action<IEntity> method)
    {
        for (int i = 0; i < entities.Count; i++)
        {
			IEntity entity = entities[i];
			
            if (entity.AttachedGameObject == null) 
            {
                entities.Remove(entity);
                i--;
                continue;
            }

            method(entity);
        }
    }
    
	public static bool GetComponentFromGameObject<T>(GameObject gameObject, out T component)
    {
        gameObject.TryGetComponent(out component);
        if (component == null)
        {
            Debug.LogError($"{gameObject.name} does not have {typeof(T)}");
            return false;
        }
        return true;
    }
}

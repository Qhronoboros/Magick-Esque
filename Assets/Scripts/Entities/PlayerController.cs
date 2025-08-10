using UnityEngine;

public class PlayerController : IEntity, IHealth, IMovable, IMagicUser 
{
    // Player Input
    private CommandHandler _moveInputHandler;
    private CommandHandler _magicInputHandler;

    public Faction faction;
    
    public event System.Action<int, int, Vector3, Color> OnHit;
    public event System.Action OnDeath;

    public GameObject AttachedGameObject { get; private set; }
    public Material AttachedMaterial { get; private set; }
    public int MaxHealth { get; private set; }
    public int Health { get; private set; }

    public IPhysics Physics { get; private set; }
    public SpellCaster SpellCaster { get; private set; }

    public PlayerController(GameObject attachedGameObject, CommandHandler moveInputHandler, CommandHandler magicInputHandler,
        IPhysics locomotion, SpellCaster spellCaster, int maxHealth)
    {
        AttachedGameObject = attachedGameObject;
        
        HelperFunctions.GetMaterialFromGameObject(attachedGameObject, out Material material);
        AttachedMaterial = material;
        
        _moveInputHandler = moveInputHandler;
        _magicInputHandler = magicInputHandler;
        Physics = locomotion;
        SpellCaster = spellCaster;
        
        MaxHealth = Health = maxHealth;
        
        OnHit += UIManager.instance.UpdatePlayerHealthText;
        OnDeath += GameManager.instance.ShowResult;
    }

    public void Update(float deltaTime)
    {
        CommandHandler.ExecuteCommands(_magicInputHandler.ReceiveCommands(), AttachedGameObject);
        FaceTowardsMouse();
    }

    public void FixedUpdate(float deltaTime)
    {
        CommandHandler.ExecuteCommands(_moveInputHandler.ReceiveCommands(), AttachedGameObject);
    }

    private void FaceTowardsMouse()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(AttachedGameObject.transform.position);
        Vector3 direction = (Input.mousePosition - playerScreenPosition).normalized;
        direction = new Vector3(direction.x, 0.0f, direction.y).normalized;

        AttachedGameObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    public void TakeDamage(int damage, Color damageColor)
    {
        if (AttachedGameObject == null) return;
        
        Health -= damage;
        OnHit?.Invoke(Health, MaxHealth, AttachedGameObject.transform.position, damageColor);
        
        if (Health <= 0)
        {
            Debug.Log($"{AttachedGameObject.name} died");
            OnDeath?.Invoke();
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        GameObject.Destroy(AttachedGameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : IEntity, IHealth, IMovable, IMagicUser 
{
    // Player Input
    private CommandHandler _moveInputHandler;
    private CommandHandler _magicInputHandler;

    public Faction faction;
    
    public event System.Action<int, int> OnHit;
    public event System.Action OnDeath;

    public GameObject AttachedGameObject { get; private set; }
    public int MaxHealth { get; private set; }
    public int Health { get; private set; }

    public ILocomotion ActorLocomotion { get; private set; }
    public SpellCaster ActorSpellCaster { get; private set; }

    public PlayerController(GameObject attachedGameObject, CommandHandler moveInputHandler, CommandHandler magicInputHandler,
        ILocomotion locomotion, SpellCaster spellCaster)
    {
        AttachedGameObject = attachedGameObject;
        _moveInputHandler = moveInputHandler;
        _magicInputHandler = magicInputHandler;
        ActorLocomotion = locomotion;
        ActorSpellCaster = spellCaster;
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

    public void TakeDamage(int damage)
    {
    }

    public void DestroySelf()
    {
    }
}

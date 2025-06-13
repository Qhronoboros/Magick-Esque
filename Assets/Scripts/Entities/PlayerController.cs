using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : IEntity, IHealth, IMovable, IMagicUser 
{
    // Player Input
    private CommandHandler _keyInputHandler;
    private CommandHandler _mouseInputHandler;

    public Faction faction;
    
    public event System.Action<int, int> OnHit;
    public event System.Action OnDeath;

    public GameObject AttachedGameObject { get; private set; }
    public int MaxHealth { get; private set; }
    public int Health { get; private set; }

    public ILocomotion ActorLocomotion { get; private set; }
    public SpellCaster ActorSpellCaster { get; private set; }

    public PlayerController(GameObject attachedGameObject, CommandHandler keyInputHandler, CommandHandler mouseInputHandler,
        ILocomotion locomotion, SpellCaster spellCaster)
    {
        AttachedGameObject = attachedGameObject;
        _keyInputHandler = keyInputHandler;
        _mouseInputHandler = mouseInputHandler;
        ActorLocomotion = locomotion;
        ActorSpellCaster = spellCaster;
    }

    public void Update(float deltaTime)
    {
        CommandHandler.ExecuteCommands(_mouseInputHandler.ReceiveCommands());
    }

    public void FixedUpdate(float deltaTime)
    {
        CommandHandler.ExecuteCommands(_keyInputHandler.ReceiveCommands());
    }

    public void TakeDamage(int damage)
    {
    }

    public void DestroySelf()
    {
    }
}

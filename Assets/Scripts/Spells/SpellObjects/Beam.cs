using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : ISpellObject
{
    // Hardcoded max distance
    private float _maxDistance = 150.0f;
    private float _currentDistance;

    public GameObject AttachedGameObject { get; private set; }
    public Material AttachedMaterial { get; private set; }
    public GameObject Actor { get; private set; }
    public ISpellStats ActorSpellStats { get; private set; }

    public Beam(GameObject attachedGameObject, GameObject actor, ISpellStats actorSpellStats)
    {
        AttachedGameObject = attachedGameObject;
        
        HelperFunctions.GetMaterialFromGameObject(attachedGameObject, out Material material);
        AttachedMaterial = material;
        AttachedMaterial.color = actorSpellStats.GetColor();
        
        Actor = actor;
        ActorSpellStats = actorSpellStats;

        _currentDistance = _maxDistance;
        
        DetectEnemies();
        FollowPlayer();
    }

    // Sets the position in front of the player
    private void FollowPlayer()
    {
        AttachedGameObject.transform.localScale = new Vector3(ActorSpellStats.GetSize(), _currentDistance * 0.5f, ActorSpellStats.GetSize());
        AttachedGameObject.transform.position = Actor.transform.position + (Actor.transform.forward * _currentDistance * 0.5f);
        AttachedGameObject.transform.eulerAngles = new Vector3(0.0f, Actor.transform.eulerAngles.y + 90.0f, AttachedGameObject.transform.eulerAngles.z);
    }

    private void DetectEnemies()
    {
        if (!Physics.SphereCast(Actor.transform.position, ActorSpellStats.GetSize(), Actor.transform.forward,
            out RaycastHit hit, _maxDistance, GameManager.instance.enemyLayer))
        {
            _currentDistance = _maxDistance;
            return;
        }
            
        Debug.Log($"Hit {hit.collider.name}");
        _currentDistance = Vector3.Distance(Actor.transform.position, hit.point);
    }

    public void Update(float deltaTime)
    {
    }

    public void FixedUpdate(float deltaTime)
    {
        DetectEnemies();
        FollowPlayer();
    }
    
    public void DestroySelf()
    {
        GameObject.Destroy(AttachedGameObject);
    }
}

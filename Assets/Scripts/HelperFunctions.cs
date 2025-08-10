using System;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions
{
    // Removes the entity from the list if the attached gameObject is destroyed and call the given method
    public static void CallMethodFromEntities(List<IEntity> entities, Action<IEntity> method)
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
    
    public static bool GetPhysicsComponentsFromGameObject(GameObject gameObject, out Rigidbody rigidbody, out Collider collider)
    {
        GetComponentFromGameObject(gameObject, out rigidbody);
        GetComponentFromGameObject(gameObject, out collider);

        return rigidbody != null && collider != null;
    }
    
    public static bool GetMaterialFromGameObject(GameObject gameObject, out Material material)
    {
        material = null;
        
        if (!GetComponentFromGameObject(gameObject, out Renderer renderer))
            return false;

        material = renderer.material;
        return true;
    }
    
    // Returns true if collider is touching the platform
    public static bool IsTouchingPlatform(Collider collider, Transform platformTransform, float verticalOffset = 0.0f)
    {
        Vector3 platformTopPosition = platformTransform.position + platformTransform.lossyScale * 0.5f;
        Vector3 closestPoint = collider.ClosestPoint(platformTopPosition);
        
        return closestPoint.y + verticalOffset <= platformTopPosition.y;
    }
}

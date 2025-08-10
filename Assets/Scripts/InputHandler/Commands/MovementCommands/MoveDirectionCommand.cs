using UnityEngine;

public class MoveDirectionCommand : ICommand
{
    private IPhysics _locomotion;
    private Vector3 _direction;

    public MoveDirectionCommand(IPhysics locomotion, Vector3 direction)
    {
        _locomotion = locomotion;
        _direction = direction.normalized;
    }

    public void Execute(GameObject actor)
    {
        // Debug.Log($"Move {_direction}");
        _locomotion.ApplyForce(_direction);
    }
}
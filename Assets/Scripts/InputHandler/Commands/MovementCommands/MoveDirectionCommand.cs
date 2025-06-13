using UnityEngine;

public class MoveDirectionCommand : ICommand
{
    private ILocomotion _locomotion;
    private Vector3 _direction;

    public MoveDirectionCommand(ILocomotion locomotion, Vector3 direction)
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
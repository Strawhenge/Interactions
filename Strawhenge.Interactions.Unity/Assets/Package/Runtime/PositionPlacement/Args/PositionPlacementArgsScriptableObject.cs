using UnityEngine;

namespace Strawhenge.Interactions.Unity.PositionPlacement
{
    [CreateAssetMenu(menuName = "Strawhenge/Interactions/Position Placement Args")]
    public class PositionPlacementArgsScriptableObject : ScriptableObject, IPositionPlacementArgs
    {
        [SerializeField] float _moveSpeed;
        [SerializeField] float _positionBuffer;
        [SerializeField] float _turnSpeed;
        [SerializeField] float _directionBuffer;
        [SerializeField] float _timeoutInSeconds;

        float IPositionPlacementArgs.MoveSpeed => _moveSpeed;

        float IPositionPlacementArgs.PositionBuffer => _positionBuffer;

        float IPositionPlacementArgs.TurnSpeed => _turnSpeed;

        float IPositionPlacementArgs.DirectionBuffer => _directionBuffer;

        float IPositionPlacementArgs.TimeoutInSeconds => _timeoutInSeconds;
    }
}
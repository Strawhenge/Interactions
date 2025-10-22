using UnityEngine;

namespace Strawhenge.Interactions.Unity.PositionPlacement
{
    public class PositionPlacementInstruction
    {
        public PositionPlacementInstruction(Transform marker, IPositionPlacementArgs args)
            : this(marker.position, marker.forward, args)
        {
        }

        public PositionPlacementInstruction(Vector3 position, Vector3 direction, IPositionPlacementArgs args)
        {
            Position = position;
            Direction = direction;
            Args = args;
        }

        public Vector3 Position { get; }

        public Vector3 Direction { get; }

        public IPositionPlacementArgs Args { get; }
    }
}
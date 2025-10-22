namespace Strawhenge.Interactions.Unity.PositionPlacement
{
    public class NullPositionPlacementArgs : IPositionPlacementArgs
    {
        public static NullPositionPlacementArgs Instance { get; } = new();

        NullPositionPlacementArgs()
        {
        }

        public float MoveSpeed => 0f;

        public float PositionBuffer => 0f;

        public float TurnSpeed => 0f;

        public float DirectionBuffer => 0f;

        public float TimeoutInSeconds => 0f;
    }
}
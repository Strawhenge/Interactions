namespace Strawhenge.Interactions.Unity.PositionPlacement
{
    public interface IPositionPlacementArgs
    {
        float MoveSpeed { get; }

        float PositionBuffer { get; }

        float TurnSpeed { get; }

        float DirectionBuffer { get; }

        float TimeoutInSeconds { get; }
    }
}

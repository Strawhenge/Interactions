using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.PositionPlacement
{
    public class PositionPlacementController
    {
        readonly TransformLerpPosition _transformLerpPosition;
        readonly TransformSlerpTurn _transformSlerpTurn;

        float _timeout;
        float _positionBuffer;
        float _directionBuffer;
        Action _onCompleted;
        float _timePassed;

        public PositionPlacementController(Transform root)
        {
            _transformLerpPosition = new TransformLerpPosition(root);
            _transformSlerpTurn = new TransformSlerpTurn(root);
        }

        public event Action Began;

        public event Action Ended;

        public bool IsInProgress { get; private set; }

        public void PlaceAt(Transform marker, IPositionPlacementArgs args, Action onCompleted = null) =>
            PlaceAt(marker.position, marker.forward, args, onCompleted);

        public void PlaceAt(
            Vector3 position,
            Vector3 direction,
            IPositionPlacementArgs args,
            Action onCompleted = null)
        {
            if (IsInProgress)
                OnCompleted();

            _transformLerpPosition.SetPosition(position, args.MoveSpeed);
            _transformSlerpTurn.SetDirection(direction, args.TurnSpeed);
            _timeout = args.TimeoutInSeconds;
            _positionBuffer = args.PositionBuffer;
            _directionBuffer = args.DirectionBuffer;
            _onCompleted = onCompleted;
            _timePassed = 0;

            IsInProgress = true;
            Began?.Invoke();
        }

        public void Cancel()
        {
            if (!IsInProgress) return;
            OnCompleted();
        }

        internal void Update(float timePassedInSeconds)
        {
            if (!IsInProgress) return;

            _timePassed += timePassedInSeconds;

            _transformLerpPosition.Update(timePassedInSeconds);
            _transformSlerpTurn.Update(timePassedInSeconds);

            if (HasTimedOut() || IsComplete())
            {
                _transformLerpPosition.Complete();
                _transformSlerpTurn.Complete();

                OnCompleted();
            }
        }

        void OnCompleted()
        {
            var onCompleted = _onCompleted;
            _onCompleted = null;
            IsInProgress = false;

            Ended?.Invoke();
            onCompleted?.Invoke();
        }

        bool HasTimedOut() => _timePassed >= _timeout;

        bool IsComplete() =>
            _transformLerpPosition.RemainingDistance <= _positionBuffer &&
            _transformSlerpTurn.RemainingAngle <= _directionBuffer;
    }
}
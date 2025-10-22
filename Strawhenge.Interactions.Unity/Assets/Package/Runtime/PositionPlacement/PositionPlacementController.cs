using Strawhenge.Common.Unity;
using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.PositionPlacement
{
    public class PositionPlacementController
    {
        readonly TransformLerpPosition _transformLerpPosition;
        readonly TransformSlerpTurn _transformSlerpTurn;

        float _timeout;
        Action _onCompleted;
        float _timePassed;

        public PositionPlacementController(Transform root)
        {
            _transformLerpPosition = new TransformLerpPosition(root);
            _transformSlerpTurn = new TransformSlerpTurn(root);
        }

        public event Action IsInProgressChanged;

        public bool IsInProgress { get; private set; }

        public void PlaceAt(
            PositionAndRotation position,
            IPositionPlacementArgs args,
            Action onCompleted = null)
        {
            if (IsInProgress)
                OnCompleted();

            _transformLerpPosition.SetPosition(position.Position, args.MoveSpeed);
            _transformSlerpTurn.SetDirection(position.Rotation.eulerAngles, args.TurnSpeed);
            _timeout = args.TimeoutInSeconds;
            _onCompleted = onCompleted;
            _timePassed = 0;

            IsInProgress = true;
            IsInProgressChanged?.Invoke();
        }

        internal void Update(float timePassedInSeconds)
        {
            if (!IsInProgress) return;

            _timePassed += timePassedInSeconds;
            if (_timePassed >= _timeout)
            {
                OnCompleted();
                return;
            }

            _transformLerpPosition.Update(timePassedInSeconds);
            _transformSlerpTurn.Update(timePassedInSeconds);
        }

        void OnCompleted()
        {
            _transformLerpPosition.Complete();
            _transformSlerpTurn.Complete();

            var onCompleted = _onCompleted;
            _onCompleted = null;
            IsInProgress = false;

            IsInProgressChanged?.Invoke();
            onCompleted?.Invoke();
        }
    }
}
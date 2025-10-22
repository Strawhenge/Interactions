using UnityEngine;

namespace Strawhenge.Interactions.Unity.PositionPlacement
{
    class TransformLerpPosition
    {
        readonly Transform _transform;

        Vector3 _startPosition;
        Vector3 _endPosition;
        float _speed;
        float _totalDistance;
        float _totalTimePassed;

        public TransformLerpPosition(Transform transform)
        {
            _transform = transform;
        }

        public float RemainingDistance => Vector3.Distance(_transform.position, _endPosition);

        public void SetPosition(Vector3 position, float speed)
        {
            _startPosition = _transform.position;
            _endPosition = position;

            _speed = speed;

            _totalDistance = Vector3.Distance(_startPosition, _endPosition);
            _totalTimePassed = 0;
        }

        public void Complete() => _transform.position = _endPosition;

        public void Update(float timePassedInSeconds)
        {
            _totalTimePassed += timePassedInSeconds;

            var interpolation = _totalDistance == 0 ? 0 : _totalTimePassed * _speed / _totalDistance;
            _transform.position = Vector3.Lerp(_startPosition, _endPosition, interpolation);
        }
    }
}
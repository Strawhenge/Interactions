using UnityEngine;

namespace Strawhenge.Interactions.Unity.PositionPlacement
{
    class TransformSlerpTurn
    {
        readonly Transform _transform;

        Vector3 _direction;
        float _speed;

        public TransformSlerpTurn(Transform transform)
        {
            _transform = transform;
        }

        public float RemainingAngle => Vector3.Angle(_transform.forward, _direction);

        public void SetDirection(Vector3 direction, float speed)
        {
            _direction = direction;
            _speed = speed;
        }

        public void Complete() => _transform.forward = _direction;

        public void Update(float timePassedInSeconds)
        {
            _transform.forward = Vector3.Slerp(
                _transform.forward,
                _direction,
                timePassedInSeconds * _speed);
        }
    }
}
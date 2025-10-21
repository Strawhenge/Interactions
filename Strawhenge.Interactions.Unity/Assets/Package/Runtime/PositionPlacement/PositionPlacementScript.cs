using UnityEngine;

namespace Strawhenge.Interactions.Unity.PositionPlacement
{
    public class PositionPlacementScript : MonoBehaviour
    {
        [SerializeField, Tooltip("Optional. Will use 'this' transform if not provided.")]
        Transform _root;

        PositionPlacementController _positionPlacementController;

        public PositionPlacementController PositionPlacementController =>
            _positionPlacementController ??= CreateController();

        void Awake()
        {
            _positionPlacementController ??= CreateController();
        }

        PositionPlacementController CreateController()
        {
            return new PositionPlacementController();
        }
    }
}
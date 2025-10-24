using FunctionalUtilities;
using Strawhenge.Common.Unity.Serialization;
using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.PositionPlacement
{
    [Serializable]
    public class SerializedPositionPlacement
    {
        [SerializeField] Transform _target;

        [SerializeField] SerializedSource<
            IPositionPlacementArgs,
            SerializedPositionPlacementArgs,
            PositionPlacementArgsScriptableObject> _args;

        public Maybe<Transform> Target => _target != null
            ? Maybe.Some(_target)
            : Maybe.None<Transform>();

        public IPositionPlacementArgs Args => _args.TryGetValue(out var args)
            ? args
            : NullPositionPlacementArgs.Instance;
    }
}
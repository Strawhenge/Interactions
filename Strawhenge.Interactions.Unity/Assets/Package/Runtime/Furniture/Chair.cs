using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Furniture;
using Strawhenge.Interactions.Unity.Sit;
using UnityEngine;
using ILogger = Strawhenge.Common.Logging.ILogger;

namespace Strawhenge.Interactions.Unity
{
    public class Chair : Furniture<UserContext>
    {
        readonly Transform _startPosition;
        readonly Transform _sittingPosition;
        readonly Transform _endPosition;
        readonly ISitAnimations _sitAnimations;

        public Chair(
            string name,
            Transform startPosition,
            Transform sittingPosition,
            Transform endPosition,
            ISitAnimations sitAnimations,
            ILogger logger) : base(logger)
        {
            _startPosition = startPosition;
            _sittingPosition = sittingPosition;
            _endPosition = endPosition;
            _sitAnimations = sitAnimations;

            Name = name;
        }

        public override string Name { get; }

        protected override void OnUse(UserContext userContext)
        {
        }

        protected override void OnEndUse()
        {
        }
    }
}
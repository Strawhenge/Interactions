using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Furniture;
using Strawhenge.Interactions.Unity.Sit;
using UnityEngine;
using ILogger = Strawhenge.Common.Logging.ILogger;

namespace Strawhenge.Interactions.Unity
{
    public class Chair : Furniture<UserContext>
    {
        public Chair(
            string name,
            Transform startPosition,
            Transform sittingPosition,
            Transform endPosition,
            ISitAnimations getValue,
            ILogger logger) : base(logger)
        {
            Name = name;
        }

        public override string Name { get; }

        protected override void OnUse()
        {
        }

        protected override void OnEndUse()
        {
        }
    }
}
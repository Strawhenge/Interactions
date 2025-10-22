using Strawhenge.Common.Logging;
using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Furniture;

namespace Strawhenge.Interactions.Unity
{
    public class Chair : Furniture<UserContext>
    {
        public Chair(string name, ILogger logger) : base(logger)
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
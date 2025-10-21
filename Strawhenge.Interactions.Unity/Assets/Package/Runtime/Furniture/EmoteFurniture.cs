using Strawhenge.Common.Logging;
using Strawhenge.Interactions.Furniture;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public class EmoteFurniture : Furniture<UserContext>
    {
        public EmoteFurniture(string name, ILogger logger) : base(logger)
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
using Strawhenge.Common.Logging;

namespace Strawhenge.Interactions.Furniture
{
    public class NullFurniture : Furniture
    {
        public static NullFurniture Instance { get; } = new NullFurniture();

        NullFurniture() : base(NullLogger.Instance)
        {
        }

        public override string Name => string.Empty;

        protected override void OnUse(IFurnitureUserScope scope)
        {
        }

        protected override void OnEndUse()
        {
            Ended();
        }
    }
}
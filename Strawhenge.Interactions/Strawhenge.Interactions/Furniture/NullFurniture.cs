using Strawhenge.Common.Logging;

namespace Strawhenge.Interactions.Furniture
{
    public class NullFurniture<TUserContext> : Furniture<TUserContext> where TUserContext : class
    {
        public NullFurniture(ILogger logger) : base(logger)
        {
            IsDeactivated = true;
        }

        public override string Name => string.Empty;

        protected override void OnUse()
        {
        }

        protected override void OnEndUse()
        {
            Ended();
        }
    }
}
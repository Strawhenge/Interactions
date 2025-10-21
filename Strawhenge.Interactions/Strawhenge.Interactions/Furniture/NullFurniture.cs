using Strawhenge.Common.Logging;

namespace Strawhenge.Interactions.Furniture
{
    public class NullFurniture<TUserContext> : Furniture<TUserContext> where TUserContext : class
    {
        public static NullFurniture<TUserContext> Instance { get; } = new NullFurniture<TUserContext>();
        
        NullFurniture() : base(NullLogger.Instance)
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
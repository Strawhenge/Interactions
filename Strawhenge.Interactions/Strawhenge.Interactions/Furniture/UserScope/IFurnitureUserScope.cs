using FunctionalUtilities;

namespace Strawhenge.Interactions.Furniture
{
    public interface IFurnitureUserScope
    {
        Maybe<T> Get<T>() where T : class;
    }
}
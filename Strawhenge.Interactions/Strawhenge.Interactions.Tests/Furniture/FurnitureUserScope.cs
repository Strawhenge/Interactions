using FunctionalUtilities;
using Strawhenge.Interactions.Furniture;

namespace Strawhenge.Interactions.Tests;

public class FurnitureUserScope : IFurnitureUserScope
{
    public Maybe<T> Get<T>() where T : class => Maybe.None<T>();
}
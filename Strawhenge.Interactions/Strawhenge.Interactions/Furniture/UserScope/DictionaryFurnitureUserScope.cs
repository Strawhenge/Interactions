using System;
using System.Collections.Generic;
using FunctionalUtilities;

namespace Strawhenge.Interactions.Furniture
{
    public class DictionaryFurnitureUserScope : IFurnitureUserScope, IConfigureFurnitureUserScope
    {
        readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        Maybe<T> IFurnitureUserScope.Get<T>()
        {
            return _services.TryGetValue(typeof(T), out var value) && value is T service
                ? Maybe.Some(service)
                : Maybe.None<T>();
        }

        void IConfigureFurnitureUserScope.Set<T>(T service)
        {
            _services[typeof(T)] = service ?? throw new ArgumentNullException(nameof(service));
        }
    }
}
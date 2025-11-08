using FunctionalUtilities;
using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Emotes;
using Strawhenge.Interactions.Unity.PositionPlacement;
using Strawhenge.Interactions.Unity.Sit;
using Strawhenge.Interactions.Unity.Sleep;
using System;
using System.Collections.Generic;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public class FurnitureUserScope : IFurnitureUserScope
    {
        readonly Dictionary<Type, object> _services;

        public FurnitureUserScope(
            EmoteController emoteController,
            SitController sitController,
            SleepController sleepController,
            PositionPlacementController positionPlacementController)
        {
            _services = new Dictionary<Type, object>
            {
                [typeof(EmoteController)] = emoteController,
                [typeof(SitController)] = sitController,
                [typeof(SleepController)] = sleepController,
                [typeof(PositionPlacementController)] = positionPlacementController
            };
        }

        public Maybe<T> Get<T>() where T : class
        {
            return _services.TryGetValue(typeof(T), out var service) && service is T result
                ? Maybe.Some(result)
                : Maybe.None<T>();
        }
    }
}
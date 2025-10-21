using System;
using System.Collections.Generic;
using FunctionalUtilities;
using Strawhenge.Common;
using Strawhenge.Common.Logging;

namespace Strawhenge.Interactions.Furniture
{
    public class FurnitureUser<TUserContext> where TUserContext : class
    {
        readonly TUserContext _context;
        readonly ILogger _logger;
        readonly List<Action> _onEndedCallbacks = new List<Action>();

        bool _isEndingUse;

        public FurnitureUser(TUserContext context, ILogger logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Maybe<Furniture<TUserContext>> CurrentFurniture { get; private set; } =
            Maybe.None<Furniture<TUserContext>>();

        public void Use(Furniture<TUserContext> furniture, Action onEnded = null)
        {
            if (furniture == null) throw new ArgumentNullException(nameof(furniture));

            if (CurrentFurniture.HasSome())
            {
                _logger.LogWarning($"User already using furniture '{furniture.Name}'.");
                onEnded?.Invoke();
                return;
            }

            if (furniture.CurrentUser.HasSome())
            {
                _logger.LogWarning($"Furniture '{furniture.Name}' is already being used.");
                onEnded?.Invoke();
                return;
            }

            if (furniture.IsDeactivated)
            {
                _logger.LogWarning($"Furniture '{furniture.Name}' is deactivated.");
                onEnded?.Invoke();
                return;
            }

            CurrentFurniture = furniture;

            if (onEnded != null)
                _onEndedCallbacks.Add(onEnded);

            furniture.SetUser(this, _context);

            _logger.LogInformation($"Using furniture '{furniture.Name}'.");
        }

        public void EndUse(Action onEnded = null)
        {
            if (!CurrentFurniture.HasSome(out var furniture))
            {
                _logger.LogInformation("User is not using furniture.");
                onEnded?.Invoke();
                return;
            }

            if (onEnded != null)
                _onEndedCallbacks.Add(onEnded);

            if (_isEndingUse)
            {
                _logger.LogInformation("Already ending use.");
                return;
            }

            _logger.LogInformation($"Ending use of furniture '{furniture.Name}'.");
            _isEndingUse = true;

            furniture.EndUse();
        }

        internal void OnFurnitureEnded()
        {
            _logger.LogInformation("Furniture use ended.");

            CurrentFurniture = Maybe.None<Furniture<TUserContext>>();
            _isEndingUse = false;

            var callbacks = _onEndedCallbacks.ToArray();
            _onEndedCallbacks.Clear();
            callbacks.ForEach(callback => callback.Invoke());
        }
    }
}
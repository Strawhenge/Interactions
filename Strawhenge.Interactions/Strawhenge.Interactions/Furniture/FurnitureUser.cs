using System;
using FunctionalUtilities;
using Strawhenge.Common.Logging;

namespace Strawhenge.Interactions.Furniture
{
    public class FurnitureUser<TUserContext> where TUserContext : class
    {
        readonly TUserContext _context;
        readonly ILogger _logger;

        Action _useOnEndedCallback;
        Action _endUseOnEndedCallback;

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

            CurrentFurniture = furniture;
            _useOnEndedCallback = onEnded;
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

            _logger.LogInformation($"Ending use of furniture '{furniture.Name}'.");
            _endUseOnEndedCallback = onEnded;
            furniture.EndUse();
        }

        internal void OnFurnitureEnded()
        {
            CurrentFurniture = Maybe.None<Furniture<TUserContext>>();
            _logger.LogInformation("Furniture use ended.");
            _useOnEndedCallback?.Invoke();
            _endUseOnEndedCallback?.Invoke();
        }
    }
}
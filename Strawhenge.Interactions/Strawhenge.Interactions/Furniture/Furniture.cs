using System;
using FunctionalUtilities;
using Strawhenge.Common.Logging;

namespace Strawhenge.Interactions.Furniture
{
    public abstract class Furniture<TUserContext> where TUserContext : class
    {
        readonly ILogger _logger;
        bool _isDeactivated;

        protected Furniture(ILogger logger)
        {
            _logger = logger;
        }

        public event Action DeactivatedStateChanged;

        public Maybe<FurnitureUser<TUserContext>> CurrentUser { get; protected set; } =
            Maybe.None<FurnitureUser<TUserContext>>();

        protected Maybe<TUserContext> UserContext { get; private set; } = Maybe.None<TUserContext>();

        internal void SetUser(FurnitureUser<TUserContext> user, TUserContext userContext)
        {
            CurrentUser = user;
            UserContext = userContext;

            try
            {
                OnUse();
            }
            catch (Exception exception)
            {
                _logger.LogException(exception);
            }
        }

        public abstract string Name { get; }

        public bool IsDeactivated
        {
            get => _isDeactivated;
            protected set
            {
                if (_isDeactivated == value) return;

                _isDeactivated = value;
                CurrentUser.Do(user => user.EndUse());
                DeactivatedStateChanged?.Invoke();
            }
        }

        protected abstract void OnUse();

        protected abstract void OnEndUse();

        protected void Ended()
        {
            CurrentUser.Do(user => user.OnFurnitureEnded());
            CurrentUser = Maybe.None<FurnitureUser<TUserContext>>();
            UserContext = Maybe.None<TUserContext>();
        }

        internal void EndUse()
        {
            try
            {
                OnEndUse();
            }
            catch (Exception exception)
            {
                _logger.LogException(exception);
            }
        }
    }
}
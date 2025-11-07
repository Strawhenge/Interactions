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

        public abstract string Name { get; }

        public Maybe<FurnitureUser<TUserContext>> CurrentUser { get; protected set; } =
            Maybe.None<FurnitureUser<TUserContext>>();

        public bool IsAvailable => !IsDeactivated && !CurrentUser.HasSome();
        
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

        internal void SetUser(FurnitureUser<TUserContext> user, TUserContext userContext)
        {
            CurrentUser = user;

            try
            {
                OnUse(userContext);
            }
            catch (Exception exception)
            {
                _logger.LogException(exception);
            }
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

        internal void NotifyUserInvalidated()
        {
            CurrentUser = Maybe.None<FurnitureUser<TUserContext>>();
            OnUserInvalidated();
        }

        protected abstract void OnUse(TUserContext userContext);

        protected abstract void OnEndUse();

        protected void Ended()
        {
            CurrentUser.Do(user => user.OnFurnitureEnded());
            CurrentUser = Maybe.None<FurnitureUser<TUserContext>>();
        }

        protected virtual void OnUserInvalidated()
        {
        }
    }
}
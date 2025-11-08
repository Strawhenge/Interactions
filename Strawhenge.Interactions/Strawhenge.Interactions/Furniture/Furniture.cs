using System;
using FunctionalUtilities;
using Strawhenge.Common.Logging;

namespace Strawhenge.Interactions.Furniture
{
    public abstract class Furniture
    {
        readonly ILogger _logger;
        bool _isDeactivated;

        protected Furniture(ILogger logger)
        {
            _logger = logger;
        }

        public event Action DeactivatedStateChanged;

        public abstract string Name { get; }

        public Maybe<FurnitureUser> CurrentUser { get; protected set; } =
            Maybe.None<FurnitureUser>();

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

        internal void SetUser(FurnitureUser user, IFurnitureUserScope userScope)
        {
            CurrentUser = user;

            try
            {
                OnUse(userScope);
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
            CurrentUser = Maybe.None<FurnitureUser>();
            OnUserInvalidated();
        }

        protected abstract void OnUse(IFurnitureUserScope userScope);

        protected abstract void OnEndUse();

        protected void Ended()
        {
            CurrentUser.Do(user => user.OnFurnitureEnded());
            CurrentUser = Maybe.None<FurnitureUser>();
        }

        protected virtual void OnUserInvalidated()
        {
        }
    }
}
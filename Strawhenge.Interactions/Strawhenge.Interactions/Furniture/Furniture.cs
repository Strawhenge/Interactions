using System;
using FunctionalUtilities;

namespace Strawhenge.Interactions.Furniture
{
    public abstract class Furniture<TUserContext> where TUserContext : class
    {
        public Maybe<FurnitureUser<TUserContext>> CurrentUser { get; protected set; } =
            Maybe.None<FurnitureUser<TUserContext>>();

        protected Maybe<TUserContext> UserContext { get; private set; } = Maybe.None<TUserContext>();

        internal void SetUser(FurnitureUser<TUserContext> user, TUserContext userContext)
        {
            CurrentUser = user;
            UserContext = userContext;
            OnUse();
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
            OnEndUse();
        }
    }

    public class FurnitureUser<TUserContext> where TUserContext : class
    {
        readonly TUserContext _context;

        Action _useOnEndedCallback;
        Action _endUseOnEndedCallback;

        public FurnitureUser(TUserContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Maybe<Furniture<TUserContext>> CurrentFurniture { get; private set; } =
            Maybe.None<Furniture<TUserContext>>();

        public void Use(Furniture<TUserContext> furniture, Action onEnded = null)
        {
            CurrentFurniture = furniture ?? throw new ArgumentNullException(nameof(furniture));
            _useOnEndedCallback = onEnded;
            furniture.SetUser(this, _context);
        }

        public void EndUse(Action onEnded = null)
        {
            _endUseOnEndedCallback = onEnded;
            CurrentFurniture.Do(furniture => furniture.EndUse());
        }

        internal void OnFurnitureEnded()
        {
            CurrentFurniture = Maybe.None<Furniture<TUserContext>>();
            _useOnEndedCallback?.Invoke();
            _endUseOnEndedCallback?.Invoke();
        }
    }
}
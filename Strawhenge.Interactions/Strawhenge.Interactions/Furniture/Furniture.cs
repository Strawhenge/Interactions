using System;
using FunctionalUtilities;

namespace Strawhenge.Interactions.Furniture
{
    public abstract class Furniture<TUserContext> where TUserContext : class
    {
        public Maybe<FurnitureUser<TUserContext>> CurrentUser { get; protected set; } =
            Maybe.None<FurnitureUser<TUserContext>>();

        internal void SetUser(FurnitureUser<TUserContext> user, TUserContext userContext)
        {
            CurrentUser = user;
            UserContext = userContext;
            OnUse();
        }

        protected Maybe<TUserContext> UserContext { get; private set; }

        protected abstract void OnUse();

        protected abstract void OnEndUse();

        protected void Ended()
        {
            CurrentUser.Do(user => user.OnFurnitureEnded());
            CurrentUser = Maybe.None<FurnitureUser<TUserContext>>();
            UserContext = Maybe.None<TUserContext>();
        }

        public void EndUse()
        {
            OnEndUse();
        }
    }

    public class FurnitureUser<TUserContext> where TUserContext : class
    {
        readonly TUserContext _context;

        public FurnitureUser(TUserContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Maybe<Furniture<TUserContext>> CurrentFurniture { get; private set; } =
            Maybe.None<Furniture<TUserContext>>();

        public void Use(Furniture<TUserContext> furniture)
        {
            CurrentFurniture = furniture;
            furniture.SetUser(this, _context);
        }

        public void EndUse()
        {
            CurrentFurniture.Do(furniture => furniture.EndUse());
        }

        internal void OnFurnitureEnded()
        {
            CurrentFurniture = Maybe.None<Furniture<TUserContext>>();
        }
    }
}
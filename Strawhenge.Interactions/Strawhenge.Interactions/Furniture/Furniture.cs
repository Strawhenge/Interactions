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
        
        public abstract string Name { get; }

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
}
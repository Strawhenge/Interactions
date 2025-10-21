using FunctionalUtilities;
using Strawhenge.Common.Logging;
using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Emotes;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public class EmoteFurniture : Furniture<UserContext>
    {
        readonly EmoteScriptableObject _emote;

        public EmoteFurniture(string name, EmoteScriptableObject emote, ILogger logger) : base(logger)
        {
            _emote = emote;
            Name = name;
        }

        public override string Name { get; }

        protected override void OnUse()
        {
            if (!UserContext.Map(user => user.EmoteController).HasSome(out var emoteController))
            {
                Ended();
                return;
            }

            emoteController.Perform(_emote, Ended);
        }

        protected override void OnEndUse()
        {
            UserContext
                .Map(user => user.EmoteController)
                .Do(emoteController => emoteController.End());
        }
    }
}
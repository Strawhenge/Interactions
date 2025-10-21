using FunctionalUtilities;
using Strawhenge.Interactions.Unity.Emotes;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public class UserContext
    {
        public UserContext(EmoteController emoteController)
        {
            EmoteController = emoteController;
        }

        public EmoteController EmoteController { get; }
    }
}
using Strawhenge.Common.Unity;

namespace Strawhenge.Interactions.Unity.Emotes
{
    static class AnimatorParameters
    {
        public static AnimatorParameter BeginEmote { get; } = new("Begin Emote");

        public static AnimatorParameter EndEmote { get; } = new("End Emote");

        public static AnimatorParameter EmoteId { get; } = new("Emote Id");
        
        public static AnimatorParameter EmoteLayerId { get; } = new("Emote Layer Id");

        public static AnimatorParameter RepeatingEmote { get; } = new("Repeating Emote");
    }
}
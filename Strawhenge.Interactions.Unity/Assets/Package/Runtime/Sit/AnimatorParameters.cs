using Strawhenge.Common.Unity;

namespace Strawhenge.Interactions.Unity.Sit
{
    static class AnimatorParameters
    {
        public static AnimatorParameter Sit { get; } = new("Sit");

        public static AnimatorParameter Stand { get; } = new("Stand");
    }
}
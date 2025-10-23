using Strawhenge.Common.Unity;

namespace Strawhenge.Interactions.Unity.Sleep
{
    static class AnimatorParameters
    {
        public static AnimatorParameter Sleep { get; } = new("Sleep");

        public static AnimatorParameter WakeUp { get; } = new("Wake Up");
    }
}
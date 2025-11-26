using Strawhenge.Common.Unity;

namespace Strawhenge.Interactions.Unity.Sleep
{
    static class AnimatorParameters
    {
        public static AnimatorParameter Sleep { get; } = new("Sleep");
      
        public static AnimatorParameter SleepTypeId { get; } = new("Sleep Type Id");

        public static AnimatorParameter WakeUp { get; } = new("Wake Up");
    }
}
using Strawhenge.Interactions.Unity.Sleep;
using System.Linq;
using UnityEditor.Animations;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class SleepTypeIdHelper
    {
        public static int GenerateSitTypeId(AnimatorControllerLayer layer)
        {
            int highestId = layer.stateMachine.defaultState.transitions
                .SelectMany(x => x.conditions
                    .Where(y => y.parameter == AnimatorParameters.SleepTypeId.Name)
                    .Select(y => (int)y.threshold))
                .Prepend(0)
                .Max();

            return highestId + 1;
        }
    }
}
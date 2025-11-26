using Strawhenge.Interactions.Unity.Sit;
using System.Linq;
using UnityEditor.Animations;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class SitTypeIdHelper
    {
        public static int GenerateSitTypeId(AnimatorControllerLayer layer)
        {
            int highestId = layer.stateMachine.defaultState.transitions
                .SelectMany(x => x.conditions
                    .Where(y => y.parameter == AnimatorParameters.SitTypeId.Name)
                    .Select(y => (int)y.threshold))
                .Prepend(0)
                .Max();

            return highestId + 1;
        }
    }
}
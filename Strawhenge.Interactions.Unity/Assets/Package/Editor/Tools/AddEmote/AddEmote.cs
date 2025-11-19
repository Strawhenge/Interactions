using Strawhenge.Interactions.Unity.Emotes;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class AddEmote
    {
        public static void Add(AddEmoteArgs args)
        {
            int emoteId = 1; // TODO

            var layer = args.AnimatorController.layers
                .FirstOrDefault(x => x.name == args.LayerName);

            if (layer == null)
            {
                Debug.LogError($"Layer '{args.LayerName}' not found.", args.AnimatorController);
                return;
            }

            var rootStateMachine = layer.stateMachine;
            var emoteStateMachine = layer.stateMachine.stateMachines
                .Select(x => x.stateMachine)
                .FirstOrDefault(x => x.behaviours.OfType<EmotesStateMachine>().Any());

            if (emoteStateMachine == null)
            {
                Debug.LogError(
                    $"Layer '{args.LayerName}' does not contain '{nameof(EmotesStateMachine)}'.",
                    args.AnimatorController);
                return;
            }

            var emoteState = emoteStateMachine.AddState(args.EmoteName);
            emoteState.motion = args.Animation;

            var beginEmoteTransition = rootStateMachine.defaultState.AddTransition(emoteState);
            beginEmoteTransition.AddCondition(AnimatorConditionMode.If, 0, AnimatorParameters.BeginEmote.Name);
            beginEmoteTransition.AddCondition(AnimatorConditionMode.Equals, emoteId, AnimatorParameters.EmoteId.Name);
            beginEmoteTransition.hasExitTime = false;

            var endEmoteTransition = emoteState.AddExitTransition();
            endEmoteTransition.AddCondition(AnimatorConditionMode.If, 0, AnimatorParameters.EndEmote.Name);
            endEmoteTransition.hasExitTime = false;

            if (!args.IsRepeating)
            {
                var animationEndedTransition = emoteState.AddExitTransition();
                animationEndedTransition.hasExitTime = true;
            }
        }
    }
}
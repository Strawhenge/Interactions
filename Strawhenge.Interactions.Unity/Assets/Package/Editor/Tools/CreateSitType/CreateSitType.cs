using Strawhenge.Interactions.Unity.Sit;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class CreateSitType
    {
        public static void Create(
            AnimatorController animatorController,
            string name,
            AnimationClip sitAnimation,
            AnimationClip sittingAnimation,
            AnimationClip standAnimation)
        {
            int id = 1; // TODO

            AnimatorControllerLayer sitLayer = null;
            AnimatorStateMachine sitStateMachine = null;

            foreach (AnimatorControllerLayer layer in animatorController.layers)
            foreach (var stateMachine in layer.stateMachine.stateMachines.Select(x => x.stateMachine))
            {
                if (stateMachine.behaviours.OfType<SitStateMachine>().Any())
                {
                    sitLayer = layer;
                    sitStateMachine = stateMachine;
                }
            }

            if (sitLayer == null || sitStateMachine == null)
            {
                Debug.LogError("Sit layer not found.", animatorController);
                return;
            }

            SitTypeHelper.AddSitType(
                id,
                name,
                sitAnimation,
                sittingAnimation,
                standAnimation,
                sitStateMachine,
                sitLayer.stateMachine
            );
        }
    }
}
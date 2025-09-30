using UnityEngine;

namespace Strawhenge.Interactions.Unity
{
    public static class AnimatorExtensions
    {
        public static void SetTrigger(this Animator animator, AnimatorParameter parameter) =>
            animator.SetTrigger(parameter.Id);

        public static void ResetTrigger(this Animator animator, AnimatorParameter parameter) =>
            animator.ResetTrigger(parameter.Id);

        public static void SetBool(this Animator animator, AnimatorParameter parameter, bool value) =>
            animator.SetBool(parameter.Id, value);
    }
}
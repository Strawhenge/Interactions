using UnityEngine;

namespace Strawhenge.Interactions.Unity
{
    // TODO Move this to Common
    public class AnimatorParameter
    {
        public static implicit operator AnimatorParameter(string name) => new AnimatorParameter(name);

        public AnimatorParameter(string name)
        {
            Name = name;
            Id = Animator.StringToHash(name);
        }

        public string Name { get; }

        public int Id { get; }
    }
}
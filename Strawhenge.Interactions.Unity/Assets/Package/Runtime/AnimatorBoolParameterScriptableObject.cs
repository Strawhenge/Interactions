using UnityEngine;

namespace Strawhenge.Interactions.Unity.Package.Runtime
{
    [CreateAssetMenu(menuName = "Strawhenge/Interactions/Animator Bool Parameter")]
    public class AnimatorBoolParameterScriptableObject : ScriptableObject
    {
        int? _id;

        public int Id => _id ??= Animator.StringToHash(name);
    }
}
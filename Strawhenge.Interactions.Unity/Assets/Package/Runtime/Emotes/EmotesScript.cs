using Strawhenge.Common.Unity.Helpers;
using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    public class EmotesScript : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        AnimatorOverrideController _animatorOverrideController;

        void Awake()
        {
            // TODO ComponentRefHelper.EnsureRootHierarchyComponent(ref _animator, nameof(_animator), this);
            ComponentRefHelper.EnsureHierarchyComponent(ref _animator, nameof(_animator), this);

            _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = _animatorOverrideController;
        }

        public void Perform(EmoteScriptableObject emote)
        {
            emote.Animation.Do(animation =>
                _animatorOverrideController["Emote"] = animation);
            
            _animator.SetTrigger("Begin Emote");
        }

        public void End()
        {
            _animator.SetTrigger("End Emote");
        }
    }
}
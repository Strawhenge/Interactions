using Strawhenge.Common.Unity.Helpers;
using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    public class EmotesScript : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        void Awake()
        {
            // TODO ComponentRefHelper.EnsureRootHierarchyComponent(ref _animator, nameof(_animator), this);
            ComponentRefHelper.EnsureHierarchyComponent(ref _animator, nameof(_animator), this);
        }

        public void Perform(EmoteScriptableObject emote)
        {
            Debug.Log($"Performing emote '{emote.name}'.", emote);
        }

        public void End()
        {
            Debug.Log("Ending emote.", this);
        }
    }
}
using Strawhenge.Common.Unity.AnimatorBehaviours;
using Strawhenge.Common.Unity.Helpers;
using Strawhenge.Inventory.Unity;
using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    public class EmotesScript : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField, Tooltip("Optional.")] InventoryScript _inventory;

        AnimatorOverrideController _animatorOverrideController;
        StateMachineEvents<EmotesStateMachine> _stateMachineEvents;

        void Awake()
        {
            // TODO ComponentRefHelper.EnsureRootHierarchyComponent(ref _animator, nameof(_animator), this);
            ComponentRefHelper.EnsureHierarchyComponent(ref _animator, nameof(_animator), this);

            _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = _animatorOverrideController;

            _stateMachineEvents = _animator.AddEvents<EmotesStateMachine>(
                stateMachine => stateMachine.OnEmoteEnded = OnEmoteEnded,
                _ => { });
        }

        public void Perform(EmoteScriptableObject emote)
        {
            _stateMachineEvents.PrepareIfRequired();

            emote.Animation.Do(animation =>
                _animatorOverrideController["Emote"] = animation);

            _animator.SetTrigger("Begin Emote");
        }

        public void End()
        {
            _stateMachineEvents.PrepareIfRequired();
            _animator.SetTrigger("End Emote");
        }

        void OnEmoteEnded()
        {
            Debug.Log("Emote Ended", this);
        }
    }
}
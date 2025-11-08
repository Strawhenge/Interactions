using Strawhenge.Common;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    [CreateAssetMenu(menuName = "Strawhenge/Interactions/Emote Collection")]
    public class EmoteCollectionScriptableObject : ScriptableObject, IEmoteCollection
    {
        [SerializeField] EmoteScriptableObject[] _emotes;

        public IReadOnlyList<EmoteScriptableObject> Emotes => _emotes.ExcludeNull().ToArray();
    }
}
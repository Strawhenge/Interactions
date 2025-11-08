using Strawhenge.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    [Serializable]
    public class SerializedEmoteCollection : IEmoteCollection
    {
        [SerializeField] EmoteScriptableObject[] _emotes;

        public IReadOnlyList<EmoteScriptableObject> Emotes => _emotes.ExcludeNull().ToArray();
    }
}
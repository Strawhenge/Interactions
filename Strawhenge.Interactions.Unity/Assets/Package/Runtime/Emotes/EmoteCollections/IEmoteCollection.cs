using System.Collections.Generic;

namespace Strawhenge.Interactions.Unity.Emotes
{
    public interface IEmoteCollection
    {
        IReadOnlyList<EmoteScriptableObject> Emotes { get; }
    }
}
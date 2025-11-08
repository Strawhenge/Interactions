using Strawhenge.Common.Unity.Serialization;

namespace Strawhenge.Interactions.Unity.Emotes
{
    public class EmoteCollectionSource
        : SerializedSource<
            IEmoteCollection,
            SerializedEmoteCollection,
            EmoteCollectionScriptableObject>
    {
    }
}
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    public class EmotesScript : MonoBehaviour
    {
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
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public abstract class FurnitureScript : MonoBehaviour
    {
        public abstract Interactions.Furniture.Furniture Furniture { get; }
    }
}
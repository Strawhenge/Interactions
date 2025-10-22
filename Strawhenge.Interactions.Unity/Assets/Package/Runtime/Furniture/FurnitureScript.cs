using Strawhenge.Interactions.Furniture;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public abstract class FurnitureScript : MonoBehaviour
    {
        public abstract Furniture<UserContext> Furniture { get; }
    }
}
using Strawhenge.Interactions.Furniture;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public abstract class FurnitureUserScopeConfigurationScript : MonoBehaviour
    {
        protected internal abstract void Configure(IConfigureFurnitureUserScope scope);
    }
}
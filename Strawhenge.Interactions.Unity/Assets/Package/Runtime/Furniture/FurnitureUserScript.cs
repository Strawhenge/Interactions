using Strawhenge.Common.Unity;
using Strawhenge.Interactions.Furniture;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public class FurnitureUserScript : MonoBehaviour
    {
        [SerializeField, Tooltip("Optional.")] LoggerScript _logger;

        FurnitureUser<UserContext> _user;

        public FurnitureUser<UserContext> User => _user ??= CreateUser();

        void Awake()
        {
            _user ??= CreateUser();
        }

        FurnitureUser<UserContext> CreateUser()
        {
            var context = new UserContext();

            var logger = _logger != null
                ? _logger.Logger
                : new UnityLogger(gameObject);

            return new FurnitureUser<UserContext>(context, logger);
        }
    }
}
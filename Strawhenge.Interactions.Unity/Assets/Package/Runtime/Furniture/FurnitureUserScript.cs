using Strawhenge.Common.Unity;
using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Emotes;
using Strawhenge.Interactions.Unity.PositionPlacement;
using Strawhenge.Interactions.Unity.Sit;
using Strawhenge.Interactions.Unity.Sleep;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public class FurnitureUserScript : MonoBehaviour
    {
        [SerializeField] EmotesScript _emotes;
        [SerializeField] SitScript _sit;
        [SerializeField] SleepScript _sleep;
        [SerializeField] PositionPlacementScript _positionPlacement;
        [SerializeField, Tooltip("Optional.")] FurnitureUserScopeConfigurationScript _scopeConfiguration;
        [SerializeField, Tooltip("Optional.")] LoggerScript _logger;

        FurnitureUser _user;

        public FurnitureUser User => _user ??= CreateUser();

        void Awake()
        {
            _user ??= CreateUser();
        }

        FurnitureUser CreateUser()
        {
            var userScope = new DictionaryFurnitureUserScope();
            ConfigureScope(userScope);

            var logger = _logger != null
                ? _logger.Logger
                : new UnityLogger(gameObject);

            return new FurnitureUser(userScope, logger);
        }

        void ConfigureScope(IConfigureFurnitureUserScope userScope)
        {
            if (_emotes != null)
                userScope.Set(_emotes.EmoteController);
            if (_sit != null)
                userScope.Set(_sit.SitController);
            if (_sleep != null)
                userScope.Set(_sleep.SleepController);
            if (_positionPlacement != null)
                userScope.Set(_positionPlacement.PositionPlacementController);

            if (_scopeConfiguration != null)
                _scopeConfiguration.Configure(userScope);
        }
    }
}
using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Emotes;
using Strawhenge.Interactions.Unity.PositionPlacement;
using UnityEngine;
using ILogger = Strawhenge.Common.Logging.ILogger;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public class EmoteFurniture : Furniture<UserContext>
    {
        readonly EmoteScriptableObject _emote;
        readonly Transform _position;
        readonly IPositionPlacementArgs _positionPlacementArgs;

        UserContext _userContext;

        public EmoteFurniture(
            string name,
            EmoteScriptableObject emote,
            Transform position,
            IPositionPlacementArgs positionPlacementArgs,
            ILogger logger) : base(logger)
        {
            _emote = emote;
            _position = position;
            _positionPlacementArgs = positionPlacementArgs;

            Name = name;
        }

        public override string Name { get; }

        protected override void OnUse(UserContext userContext)
        {
            _userContext = userContext;

            _userContext.PositionPlacementController.PlaceAt(
                _position.position,
                _position.forward,
                _positionPlacementArgs,
                onCompleted: () => _userContext.EmoteController.Perform(_emote, Ended));
        }

        protected override void OnEndUse()
        {
            _userContext.EmoteController.End();
        }
    }
}
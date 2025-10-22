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
        readonly PositionPlacement.PositionPlacementInstruction _positionPlacement;
        readonly Transform _position;
        readonly IPositionPlacementArgs _positionPlacementArgs;

        UserContext _userContext;

        public EmoteFurniture(
            string name,
            EmoteScriptableObject emote,
            PositionPlacement.PositionPlacementInstruction positionPlacement,
            ILogger logger) : base(logger)
        {
            _emote = emote;
            _positionPlacement = positionPlacement;

            Name = name;
        }

        public override string Name { get; }

        protected override void OnUse(UserContext userContext)
        {
            _userContext = userContext;

            _userContext.PositionPlacementController.PlaceAt(
                _positionPlacement,
                onCompleted: () => _userContext.EmoteController.Perform(_emote, Ended));
        }

        protected override void OnEndUse()
        {
            _userContext.EmoteController.End();
        }
    }
}
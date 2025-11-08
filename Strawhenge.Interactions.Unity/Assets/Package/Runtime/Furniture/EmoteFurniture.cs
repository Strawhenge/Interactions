using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Emotes;
using Strawhenge.Interactions.Unity.PositionPlacement;
using ILogger = Strawhenge.Common.Logging.ILogger;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public class EmoteFurniture : Interactions.Furniture.Furniture
    {
        readonly EmoteScriptableObject _emote;
        readonly PositionPlacementInstruction _positionPlacement;
        readonly ILogger _logger;

        EmoteController _emoteController;

        public EmoteFurniture(
            string name,
            EmoteScriptableObject emote,
            PositionPlacementInstruction positionPlacement,
            ILogger logger) : base(logger)
        {
            _emote = emote;
            _positionPlacement = positionPlacement;
            _logger = logger;

            Name = name;
        }

        public override string Name { get; }

        protected override void OnUse(IFurnitureUserScope userScope)
        {
            if (!userScope.Get<EmoteController>().HasSome(out _emoteController))
            {
                _logger.LogWarning($"'{nameof(EmoteController)}' not found in user scope.");
                Ended();
                return;
            }

            if (!userScope.Get<PositionPlacementController>().HasSome(out var positionPlacementController))
            {
                _logger.LogWarning($"'{nameof(PositionPlacementController)}' not found in user scope.");
                Ended();
                return;
            }

            positionPlacementController.PlaceAt(
                _positionPlacement,
                onCompleted: () => _emoteController.Perform(_emote, Ended));
        }

        protected override void OnEndUse()
        {
            _emoteController?.End();
            _emoteController = null;
        }
    }
}
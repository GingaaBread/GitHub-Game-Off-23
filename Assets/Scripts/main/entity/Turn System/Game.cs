using main.entity.Card_Management.Card_Data;

namespace main.entity.Turn_System
{
    /// <summary>
    ///     This entity is responsible for storing information about the current game.
    ///     This includes the state the game is in at the moment, the amount of turns that have elapsed since game
    ///     start, and a tracker for fish scaling.
    /// </summary>
    public class Game
    {
        /// <summary>
        ///     Defines how many turns there are in a game.
        ///     Not that the last turn is EXCLUSIVE and will not be played.
        /// </summary>
        public readonly int TurnsInAGame;

        /// <summary>
        ///     Defines the turn in which the card swap system will start.
        ///     This prevents only having low-rarity card swaps in the early-game.
        /// </summary>
        public readonly int TurnToStartSwappingCards;

        /// <summary>
        ///     Defines the turn in which the card system will stop.
        ///     This turn is still INCLUDED in the swapping mechanic, only after that turn will it stop.
        /// </summary>
        public readonly int TurnToStopSwappingCards;
        /// <summary>
        ///     Defines the amount of fish the player has to scale to win the game.
        ///     This amount is increased everytime a fish is scaled.
        ///     After the <see cref="TurnsInAGame" />, if this value is not reached, it is game over.
        /// </summary>
        public const int RequiredAmountOfFishToScaleToWin = 30;

        /// <summary>
        ///     Tracks the amount of fish that have been completely scaled by the player.
        ///     This is incremented each time a fish is scaled in the FishService.
        /// </summary>
        public int currentAmountOfScaledFish;
        
        /// <summary>
        ///     Determines whether the game is over or not.
        /// </summary>
        public bool IsGameOver { get; set; }

        /// <summary>
        ///     Tracks if at least one fish has been scaled either this turn or in the last turn.
        ///     This is used to track the losing condition (in this case the run is considered lost).
        /// </summary>
        public bool FishHasBeenScaledThisOrLastTurn { get; set; }

        /// <summary>
        ///     Tracks the last three offered cards by the system during the card swap phase.
        ///     This is required to be able to prevent the game from offering the same three cards two consecutive
        ///     times.
        /// </summary>
        public Card[] LastOfferedCards { get; set; }
        
        public Game(int turnsInAGame, int turnToStartSwappingCards, int turnToStopSwappingCards)
        {
            TurnsInAGame = turnsInAGame;
            TurnToStartSwappingCards = turnToStartSwappingCards;
            TurnToStopSwappingCards = turnToStopSwappingCards;
        }
    }
}
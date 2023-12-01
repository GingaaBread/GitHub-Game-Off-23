using main.entity.Turn_System;
using main.service.Fish_Management;
using UnityEngine.Events;

namespace main.service.Turn_System
{
    /// <summary>
    ///     This service provides the business logic for the game entity, including a way to end the current turn.
    /// </summary>
    public class GameService : Service
    {
        private readonly Game game;
        private readonly Turn turn;
        private readonly FishService fishService;

        public GameService(Game game, Turn turn, FishService fishService)
        {
            this.game = game;
            this.turn = turn;
            this.fishService = fishService;
            
            StartNewGame();
        }

        /// <summary>
        ///     Triggered once the game is over because there have been two turns without scaling a fish,
        ///     or the round limit has been reached
        /// </summary>
        public readonly UnityEvent OnGameOver = new();

        /// <summary>
        ///     Starts the current game by creating all required services (resetting the old ones if they existed
        ///     already), loading the deck, pool, discard pile, etc. and then drawing the starter hand
        /// </summary>
        private void StartNewGame()
        {
            LogInfo("Now starting a new game");

            RegisterEvents();
            ResetGameVariables();
            
            LogInfo("Successfully started the game, now waiting for player actions");
        }
        
        // TODO: Remove after player selections are implemented 
        public bool GameIsRunningJustForTest => game.FishHasBeenScaledThisOrLastTurn && turn.CurrentTurnNumber <= game.TurnsInAGame;

        public void HandleGameOver()
        {
            if (!CheckIfGameIsOver())
            {
                return;
            }
            
            LogInfo("Game over. Now ending the game");
            game.IsGameOver = true;
            LogInfo("Now triggering the OnGameOver event");
            OnGameOver.Invoke();
        }

        public bool IsGameOver()
        {
            return game.IsGameOver;
        }

        /// <summary>
        ///     After the start of the game, the game should not instantly expect scaling a fish, and it should be in
        ///     the turn start state, and there should be no last offered cards
        /// </summary>
        private void ResetGameVariables()
        {
            game.currentAmountOfScaledFish = 0;
            game.FishHasBeenScaledThisOrLastTurn = true;
            game.LastOfferedCards = null;
        }

        /// <summary>
        ///     Registers all events that the game service is interested in
        /// </summary>
        private void RegisterEvents()
        {
            fishService.OnFishHasBeenScaled.AddListener(() => game.currentAmountOfScaledFish++);
        }

        /// <summary>
        ///     Yields true if the game is over and false if it is not, and the game should continue normally.
        ///     A game is considered over if there have been no scaled fish for at least two turns or if the maximum
        ///     turns in a game has been reached (or exceeded).
        /// </summary>
        /// <returns>true - if the game is over; false - if the game is not yet over</returns>
        private bool CheckIfGameIsOver()
        {
            return turn.CurrentTurnNumber >= game.TurnsInAGame;
        }

        public int CurrentAmountOfScaledFish(){
            return game.currentAmountOfScaledFish;
        }

        public int RequiredAmountOfFishToScaleToWin(){
            return game.ScaledFishToWin();
        }
    }
}
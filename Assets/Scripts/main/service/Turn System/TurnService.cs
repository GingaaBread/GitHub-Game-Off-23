using System;
using main.entity.Turn_System;
using main.service.Card_Management;
using UnityEngine.Events;
using Zenject;

namespace main.service.Turn_System
{
    public class TurnService : Service, IInitializable, IDisposable
    {
        private readonly Turn turn;
        private readonly PlayerHandService playerHandService;
        private readonly GameService gameService;
        private readonly CardSwapService cardSwapService;
        private readonly EffectAssemblyService effectAssemblyService;

        public TurnService
        (
            Turn turn, 
            PlayerHandService playerHandService, 
            GameService gameService, 
            CardSwapService cardSwapService,
            EffectAssemblyService effectAssemblyService
        )
        {
            this.turn = turn;
            this.playerHandService = playerHandService;
            this.gameService = gameService;
            this.cardSwapService = cardSwapService;
            this.effectAssemblyService = effectAssemblyService;
        }
        
        public void Initialize()
        {
            cardSwapService.OnCardsSwapped += StartTurn;
            
            StartTurn();
        }
        
        public void Dispose()
        {
            cardSwapService.OnCardsSwapped -= StartTurn;
        }
        
        /// <summary>
        ///     Triggered once the turn number is increased when a new turn is started.
        ///     It uses the current turn number as its argument.
        /// </summary>
        public readonly UnityEvent<int> OnTurnNumberIncreased = new();

        public void StartTurn()
        {
            IncrementTurnNumber();
            RestoreTime();

            playerHandService.Draw();
        }

        public void EndTurn()
        {
            LogInfo("Now ending the current turn");
            
            effectAssemblyService.ExecuteAll();
            
            gameService.HandleGameOver();
            
            if (gameService.IsGameOver())
            {
                return;
            }
            
            playerHandService.DiscardHand();
            
            cardSwapService.HandleCardSwapOptions();
        }

        private void IncrementTurnNumber()
        {
            turn.CurrentTurnNumber++;
            LogInfo($"Incrementing turn number. It now is: {turn.CurrentTurnNumber}");

            OnTurnNumberIncreased.Invoke(turn.CurrentTurnNumber);
            LogInfo("Triggering the OnTurnNumberIncreased event");
        }

        private void RestoreTime()
        {
            turn.RemainingTime = turn.InitialTime;
        }
    }
}
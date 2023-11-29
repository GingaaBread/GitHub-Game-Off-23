using System;
using main.entity.Turn_System;
using main.service.Card_Management;
using UnityEngine.Events;
using Zenject;

namespace main.service.Turn_System
{
    public class TurnService : Service, IInitializable, IDisposable
    {
        private readonly CardSwapService cardSwapService;
        private readonly EffectAssemblyService effectAssemblyService;
        private readonly GameService gameService;

        public readonly UnityEvent OnNewTurnStart = new();

        /// <summary>
        ///     Triggered once the turn number is increased when a new turn is started.
        ///     It uses the current turn number as its argument.
        /// </summary>
        public readonly UnityEvent<int> OnTurnNumberIncreased = new();

        private readonly PlayerHandService playerHandService;
        private readonly Turn turn;

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

        public void Dispose()
        {
            cardSwapService.OnCardsSwapped -= StartTurn;
        }

        public void Initialize()
        {
            cardSwapService.OnCardsSwapped += StartTurn;

            StartTurn();
        }

        public void StartTurn()
        {
            IncrementTurnNumber();
            RestoreTime();

            OnNewTurnStart.Invoke();

            playerHandService.StartTurnDraw();
        }

        public void EndTurn()
        {
            LogInfo("Now ending the current turn");

            effectAssemblyService.ExecuteAll();

            gameService.HandleGameOver();

            if (gameService.IsGameOver()) return;

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
            LogInfo("Restoring time to " + turn.InitialTime.Time);
            turn.RemainingTime.Time = turn.InitialTime.Time;
            playerHandService.OnTimeUnitChange.Invoke(turn.RemainingTime.Time);
        }
    }
}
using System;
using main.entity.Card_Management.Card_Data;
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
        
        public readonly UnityEvent OnBeforeEndOfTurn = new();
        public readonly UnityEvent OnNewTurnStart = new();
        /// <summary>
        ///     Triggered once the turn number is increased when a new turn is started.
        ///     It uses the current turn number as its argument.
        /// </summary>
        public readonly UnityEvent<int> OnTurnNumberIncreased = new();
        
        public event Action OnTurnRemainingTimeChanged;

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
            playerHandService.OnCardPlayed += OnCardPlayed;

            StartTurn();
        }

        public void Dispose()
        {
            cardSwapService.OnCardsSwapped -= StartTurn;
            playerHandService.OnCardPlayed -= OnCardPlayed;
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

            OnBeforeEndOfTurn.Invoke();
            effectAssemblyService.ExecuteAll();
        }

        public void ProceedWithEndOfTurn()
        {
            gameService.HandleGameOver();

            if (gameService.IsGameOver()) return;

            playerHandService.DiscardHand();

            cardSwapService.HandleCardSwapOptions();
        }

        public void IncreaseTime(int amount)
        {
            turn.RemainingTime.Time += amount;
            OnTurnRemainingTimeChanged?.Invoke();
        }

        private void DecreaseTime(int amount)
        {
            turn.RemainingTime.Time -= amount;
            LogInfo($"Removing {amount} time, time is now {turn.RemainingTime.Time}");
            OnTurnRemainingTimeChanged?.Invoke();
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
            OnTurnRemainingTimeChanged?.Invoke();
        }
        
        private void OnCardPlayed(Card card)
        {
            DecreaseTime(card.TimeCost);
        }
    }
}
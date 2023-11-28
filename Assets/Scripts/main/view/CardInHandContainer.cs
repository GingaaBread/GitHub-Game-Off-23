﻿using System;
using System.Diagnostics.CodeAnalysis;
using main.entity.Card_Management.Card_Data;
using main.service.Card_Management;
using main.view.Canvas;
using UnityEngine;
using UnityEngine.EventSystems;

namespace main.view
{
    public class CardInHandContainer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public enum CardPlayState
        {
            PLAYABLE,
            UNPLAYABLE,
            IDLE
        }

        private const float CLAMP_WIDTH = 20f, CLAMP_HEIGHT = 10f, PLAY_HEIGHT_LIMIT = -6.5f;

        [SerializeField] private CardView _cardViewPrefab;
        [SerializeField] private Animator _animator;
        private RectTransform _childRectTransform;
        private bool _hasEnoughTimeToPlay;
        private CardPlayState _playState;
        private PlayerHandService playerHandService;

        private PlayerHandView playerHandView;

        public CardView CardView { get; private set; }

        private void OnEnable()
        {
            // Should only subscribe when the card object is set to active, NOT on start
            playerHandService?.OnTimeUnitChange.AddListener(SetUsabilityColourOfCardView);
        }

        private void OnDisable()
        {
            playerHandService.OnTimeUnitChange.RemoveListener(SetUsabilityColourOfCardView);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (playerHandService.CardHasEnoughTime(CardView.Card))
            {
                _hasEnoughTimeToPlay = true;
                PlayerHandCanvas.Instance.SetAsDirectChild(CardView.transform);
                _playState = CardPlayState.UNPLAYABLE;
            }
            else
            {
                _hasEnoughTimeToPlay = false;
                Debug.Log("CANNOT PLAY TOO LITTLE TIME");
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_hasEnoughTimeToPlay) return;

            var pos = PlayerHandCanvas
                .Instance
                .PooledMainCamera
                .ScreenToWorldPoint(eventData.position);

            _childRectTransform.position = new Vector3(Mathf.Clamp(pos.x, -CLAMP_WIDTH, CLAMP_WIDTH),
                Mathf.Clamp(pos.y, -CLAMP_HEIGHT, CLAMP_HEIGHT));

            var lastPlayState = _playState;

            _playState = pos.y >= PLAY_HEIGHT_LIMIT ? CardPlayState.PLAYABLE : CardPlayState.UNPLAYABLE;

            if (lastPlayState == _playState) return;

            switch (_playState)
            {
                case CardPlayState.UNPLAYABLE:
                    playerHandView.IncreaseSpacing();
                    _animator.Play("CardInHandContainer_Expand");
                    CardView.ChangeSelection(CardPlayState.IDLE);
                    break;
                case CardPlayState.PLAYABLE:
                    playerHandView.DecreaseSpacing();
                    _animator.Play("CardInHandContainer_Shrink");
                    CardView.ChangeSelection(CardPlayState.PLAYABLE);
                    break;
                case CardPlayState.IDLE:
                default:
                    throw new ArgumentException("Should only process playable and unplayable cards");
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_hasEnoughTimeToPlay) return;

            switch (_playState)
            {
                case CardPlayState.UNPLAYABLE:
                    _childRectTransform.SetParent(transform);
                    _childRectTransform.anchoredPosition = Vector2.zero;
                    break;
                case CardPlayState.PLAYABLE:
                    playerHandView.IncreaseSpacing();
                    playerHandView.PlayCard(CardView.Card);
                    break;
                case CardPlayState.IDLE:
                default:
                    throw new ArgumentException("Should only process playable and unplayable cards");
            }
        }

        private void SetUsabilityColourOfCardView(int _)
        {
            CardView.ChangeSelection(playerHandService.CardHasEnoughTime(CardView.Card)
                ? CardPlayState.IDLE
                : CardPlayState.UNPLAYABLE);
        }

        public void CreateChild(
            [NotNull] Card cardToContain,
            [NotNull] PlayerHandView callback,
            [NotNull] PlayerHandService playerHandService)
        {
            this.playerHandService = playerHandService;
            playerHandView = callback;

            playerHandService.OnTimeUnitChange.AddListener(SetUsabilityColourOfCardView);

            var newCardView = Instantiate(_cardViewPrefab, transform);
            CardView = newCardView;
            _childRectTransform = CardView.RectTransform;

            PlayerHandCanvas.Instance.SetAsDirectChild(newCardView.transform);

            newCardView.Initialize(cardToContain);
            newCardView.HandleDraw(transform);
            SetUsabilityColourOfCardView(-1);

            _playState = CardPlayState.IDLE;
        }
    }
}
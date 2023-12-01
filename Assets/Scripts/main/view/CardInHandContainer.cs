using System;
using System.Diagnostics.CodeAnalysis;
using FMODUnity;
using main.entity.Card_Management.Card_Data;
using main.service.Card_Management;
using main.service.Turn_System;
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
        private float desiredYOffset, desiredRotation;

        [SerializeField] private CardView _cardViewPrefab;
        [SerializeField] private Animator _animator;
        [SerializeField] private StudioEventEmitter _cardRedockEvent;
        [SerializeField] private StudioEventEmitter _cardSelectionEvent;
        private RectTransform _childRectTransform;
        private bool _hasEnoughTimeToPlay, _isInEndOfTurn;
        private CardPlayState _playState;
        private PlayerHandService playerHandService;

        private PlayerHandView playerHandView;
        private TurnService turnService;

        public CardView CardView { get; private set; }

        private void OnEnable()
        {
            // Should only subscribe when the card object is set to active, NOT on start
            playerHandService?.OnTimeUnitChange.AddListener(SetUsabilityColourOfCardView);
        }

        private void OnDisable()
        {
            playerHandService?.OnTimeUnitChange.RemoveListener(SetUsabilityColourOfCardView);
            turnService.OnBeforeEndOfTurn.RemoveListener(DisableOnEndOfTurn);
            turnService.OnNewTurnStart.RemoveListener(EnableOnNewTurn);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_isInEndOfTurn) return;

            if (playerHandService.CardHasEnoughTime(CardView.Card))
            {
                _cardSelectionEvent.Play();
                _hasEnoughTimeToPlay = true;
                PlayerHandCanvas.Instance.SetAsDirectChild(CardView.transform);
                CardView.ApplyRotation(0);
                _playState = CardPlayState.UNPLAYABLE;
            }
            else
            {
                _hasEnoughTimeToPlay = false;
                WarningCanvas.Instance.DisplayTimeWarning();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_hasEnoughTimeToPlay || _isInEndOfTurn) return;

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
            if (!_hasEnoughTimeToPlay || _isInEndOfTurn) return;

            switch (_playState)
            {
                case CardPlayState.UNPLAYABLE:
                    _cardRedockEvent.Play();
                    _childRectTransform.SetParent(transform);
                    playerHandView.ApplyRotationAndOffsetToChildren();
                    break;
                case CardPlayState.PLAYABLE:
                    playerHandView.IncreaseSpacing();
                    CardView.PlaySound();
                    playerHandView.PlayCard(CardView.Card);
                    playerHandView.ApplyRotationAndOffsetToChildren();
                    break;
                case CardPlayState.IDLE:
                default:
                    throw new ArgumentException("Should only process playable and unplayable cards");
            }
        }

        private void DisableOnEndOfTurn()
        {
            _isInEndOfTurn = true;
        }

        private void EnableOnNewTurn()
        {
            _isInEndOfTurn = false;
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
            [NotNull] PlayerHandService playerHandService,
            [NotNull] TurnService turnService)
        {
            this.playerHandService = playerHandService;
            this.turnService = turnService;
            playerHandView = callback;

            playerHandService.OnTimeUnitChange.AddListener(SetUsabilityColourOfCardView);
            turnService.OnBeforeEndOfTurn.AddListener(DisableOnEndOfTurn);
            turnService.OnNewTurnStart.AddListener(EnableOnNewTurn);

            var newCardView = Instantiate(_cardViewPrefab, transform);
            CardView = newCardView;
            _childRectTransform = CardView.RectTransform;
            CardView.ApplyOffset(desiredYOffset);
            CardView.ApplyRotation(desiredRotation);

            PlayerHandCanvas.Instance.SetAsDirectChild(newCardView.transform);

            newCardView.Initialize(cardToContain);
            newCardView.HandleDraw(transform);
            SetUsabilityColourOfCardView(-1);

            _playState = CardPlayState.IDLE;
        }

        public bool IsBeingDiscarded(){
            if(CardView) return CardView.IsBeingDiscarded();
            else return false;
        }

        public void ApplyRotation(float rotZ){
            this.desiredRotation = rotZ;
            if(CardView) CardView.ApplyRotation(rotZ);
        }

        public void ApplyOffset(float desiredYOffset){
            this.desiredYOffset = desiredYOffset;
            if(CardView) CardView.ApplyOffset(desiredYOffset);
        }
    }
}
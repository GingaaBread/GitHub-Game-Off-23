using System;
using System.Diagnostics.CodeAnalysis;
using FMODUnity;
using main.entity.Card_Management.Card_Data;
using main.service.Card_Management;
using main.service.Turn_System;
using main.view.Canvas;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

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
        [SerializeField] private StudioEventEmitter _cardRedockEvent;
        [SerializeField] private StudioEventEmitter _cardSelectionEvent;
        private RectTransform _childRectTransform;
        private bool _hasEnoughTimeToPlay, _isInEndOfTurn;
        private CardPlayState _playState;
        
        public CardView CardView { get; private set; }
        
        private PlayerHandView playerHandView;
        
        private PlayerHandService playerHandService;
        private TurnService turnService;

        [Inject]
        public void Construct(PlayerHandService playerHandService, TurnService turnService)
        {
            this.playerHandService = playerHandService;
            this.turnService = turnService;
        }
        
        public void Start()
        {
            turnService.OnTurnRemainingTimeChanged += SetUsabilityColourOfCardView;
            turnService.OnBeforeEndOfTurn.AddListener(DisableOnEndOfTurn);
            turnService.OnNewTurnStart.AddListener(EnableOnNewTurn);
        }
        
        public void OnDestroy()
        {
            turnService.OnTurnRemainingTimeChanged -= SetUsabilityColourOfCardView;
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
                    _childRectTransform.anchoredPosition = Vector2.zero;
                    break;
                case CardPlayState.PLAYABLE:
                    playerHandView.IncreaseSpacing();
                    CardView.PlaySound();
                    playerHandView.PlayCard(CardView.Card);
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

        private void SetUsabilityColourOfCardView()
        {
            CardView.ChangeSelection(playerHandService.CardHasEnoughTime(CardView.Card)
                ? CardPlayState.IDLE
                : CardPlayState.UNPLAYABLE);
        }

        public void CreateChild([NotNull] Card cardToContain, [NotNull] PlayerHandView callback)
        {
            playerHandView = callback;

            var newCardView = Instantiate(_cardViewPrefab, transform);
            CardView = newCardView;
            _childRectTransform = CardView.RectTransform;

            PlayerHandCanvas.Instance.SetAsDirectChild(newCardView.transform);

            newCardView.Initialize(cardToContain);
            newCardView.HandleDraw(transform);
            SetUsabilityColourOfCardView();

            _playState = CardPlayState.IDLE;
        }
    }
}
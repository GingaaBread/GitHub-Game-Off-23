using System;
using System.Diagnostics.CodeAnalysis;
using main.entity.Card_Management.Card_Data;
using main.view.Canvas;
using UnityEngine;
using UnityEngine.EventSystems;

namespace main.view
{
    public class CardInHandContainer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private const float CLAMP_WIDTH = 20f, CLAMP_HEIGHT = 10f, PLAY_HEIGHT_LIMIT = -6.5f;
        [SerializeField] private CardView _cardViewPrefab;
        [SerializeField] private Animator _animator;
        private PlayerHandView _callback;

        private CardView _child;
        private RectTransform _childRectTransform;
        private CardPlayState _playState;

        public void OnBeginDrag(PointerEventData eventData)
        {
            PlayerHandCanvas.Instance.SetAsDirectChild(_child.transform);
            _playState = CardPlayState.UNPLAYABLE;
        }

        public void OnDrag(PointerEventData eventData)
        {
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
                    _callback.IncreaseSpacing();
                    _animator.Play("CardInHandContainer_Expand");
                    break;
                case CardPlayState.PLAYABLE:
                    _callback.DecreaseSpacing();
                    _animator.Play("CardInHandContainer_Shrink");
                    break;
                case CardPlayState.IDLE:
                default:
                    throw new ArgumentException("Should only process playable and unplayable cards");
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            switch (_playState)
            {
                case CardPlayState.UNPLAYABLE:
                    _childRectTransform.SetParent(transform);
                    _childRectTransform.anchoredPosition = Vector2.zero;
                    break;
                case CardPlayState.PLAYABLE:
                    Destroy(gameObject);
                    _child.Discard();
                    break;
                case CardPlayState.IDLE:
                default:
                    throw new ArgumentException("Should only process playable and unplayable cards");
            }
        }

        public void CreateChild([NotNull] Card cardToContain, [NotNull] PlayerHandView callback)
        {
            var newCardView = Instantiate(_cardViewPrefab, transform);
            PlayerHandCanvas.Instance.SetAsDirectChild(newCardView.transform);
            newCardView.Render(cardToContain);
            newCardView.HandleDraw(transform);

            _callback = callback;
            _child = newCardView;
            _childRectTransform = _child.RectTransform;
            _playState = CardPlayState.IDLE;
        }

        private enum CardPlayState
        {
            PLAYABLE,
            UNPLAYABLE,
            IDLE
        }
    }
}
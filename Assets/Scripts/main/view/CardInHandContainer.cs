using System;
using System.Diagnostics.CodeAnalysis;
using main.entity.Card_Management.Card_Data;
using main.view.Canvas;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace main.view
{
    public class CardInHandContainer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private const float CLAMP_WIDTH = 20f, CLAMP_HEIGHT = 10f, PLAY_HEIGHT_LIMIT = -6.5f;
        public float desiredYOffsetOfChild;
        [SerializeField] private CardView _cardViewPrefab;
        [SerializeField] private Animator _animator;
        private PlayerHandView _callback;

        private CardView _child;
        private RectTransform _childRectTransform;
        private CardPlayState _playState;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _child.transform.rotation = Quaternion.identity;
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
                    _callback.ApplyRotationAndOffsetToChildren();
                    break;
                case CardPlayState.PLAYABLE:
                    Destroy(gameObject);
                    _child.Discard();
                    _callback.ApplyRotationAndOffsetToChildren();
                    break;
                case CardPlayState.IDLE:
                default:
                    throw new ArgumentException("Should only process playable and unplayable cards");
            }
        }

        public void CreateChild([NotNull] Card cardToContain, [NotNull] PlayerHandView callback)
        {
            var newCardView = Instantiate(_cardViewPrefab, transform);
            newCardView.desiredYOffset = desiredYOffsetOfChild;
            PlayerHandCanvas.Instance.SetAsDirectChild(newCardView.transform);
            newCardView.Render(cardToContain);
            newCardView.HandleDraw(transform);

            _callback = callback;
            _child = newCardView;
            _childRectTransform = _child.RectTransform;
            _playState = CardPlayState.IDLE;
        }

        public void ApplyRotation(float rotZ){
            if(_child)_child.transform.rotation = Quaternion.Euler(0,0,rotZ);
        }

        public void ApplyOffset(float offsetY){
            desiredYOffsetOfChild = offsetY;
            if(_child){
                if(!_child.IsBeingDrawn())_childRectTransform.anchoredPosition = Vector2.up * desiredYOffsetOfChild;
            }
        }

        private enum CardPlayState
        {
            PLAYABLE,
            UNPLAYABLE,
            IDLE
        }
    }
}
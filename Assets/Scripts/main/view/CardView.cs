using System;
using System.Collections;
using FMODUnity;
using JetBrains.Annotations;
using main.entity.Card_Management.Card_Data;
using main.view.Canvas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace main.view
{
    public class CardView : MonoBehaviour
    {
        private const float DISCARD_SPEED = 3.5f, DISCARD_CURVE_HEIGHT = 21f, DRAW_CURVE_HEIGHT = 5f, DRAW_SPEED = 5f;

        [SerializeField] private TMP_Text _cardNameText;
        [SerializeField] private TMP_Text _cardClassText;
        [SerializeField] private TMP_Text _cardDescriptionText;
        [SerializeField] private Image _cardTypeSpriteImage;
        [SerializeField] private Image _cardIconSpriteImage;
        [SerializeField] private StudioEventEmitter _cardPlayEvent;

        [Header("Item Types")] [SerializeField]
        private Sprite _actionCardPanelSprite;

        [SerializeField] private Sprite _itemCardPanelSprite;
        private Vector3[] _bezierNodes;
        private float _bezierTargetCount;
        private bool _isBeingDiscarded, _isBeingDrawn;

        private Transform _transform, _parent;

        public RectTransform RectTransform => _transform as RectTransform;

        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
            HandlePotentialDraw();
            HandlePotentialDiscard();
        }

        private void HandlePotentialDraw()
        {
            if (!_isBeingDrawn) return;

            if (_bezierTargetCount >= 1f)
            {
                _isBeingDrawn = false;
                StartCoroutine(ReparentSelf());
            }

            _bezierTargetCount += DRAW_SPEED * Time.deltaTime;

            var m1 = Vector3.Lerp(_bezierNodes[0], _bezierNodes[1], _bezierTargetCount);
            var m2 = Vector3.Lerp(_bezierNodes[1], _bezierNodes[2], _bezierTargetCount);
            _transform.position = Vector3.Lerp(m1, m2, _bezierTargetCount);
        }

        private IEnumerator ReparentSelf()
        {
            _transform.SetParent(_parent);
            yield return new WaitForEndOfFrame();
            RectTransform.anchoredPosition = Vector3.zero;
        }

        private void HandlePotentialDiscard()
        {
            if (!_isBeingDiscarded) return;

            if (_bezierTargetCount >= 1f) Destroy(gameObject);

            _bezierTargetCount += DISCARD_SPEED * Time.deltaTime;

            var m1 = Vector3.Lerp(_bezierNodes[0], _bezierNodes[1], _bezierTargetCount);
            var m2 = Vector3.Lerp(_bezierNodes[1], _bezierNodes[2], _bezierTargetCount);
            _transform.position = Vector3.Lerp(m1, m2, _bezierTargetCount);
        }

        public void Discard()
        {
            _isBeingDiscarded = true;
            _cardPlayEvent.Play();
            _bezierTargetCount = 0f;

            _bezierNodes = new Vector3[3];
            _bezierNodes[0] = _transform.position;
            _bezierNodes[2] = PlayerHandCanvas.Instance.DiscardTargetPosition;
            _bezierNodes[1] = _bezierNodes[0] + (_bezierNodes[2] - _bezierNodes[0]) / 2 +
                              Vector3.up * DISCARD_CURVE_HEIGHT;
        }

        public void HandleDraw([NotNull] Transform parent)
        {
            _parent = parent;

            // We will draw from the same position used for discarding cards
            var drawOriginPosition = PlayerHandCanvas.Instance.DiscardTargetPosition;

            _transform.position = drawOriginPosition;
            _isBeingDrawn = true;

            _bezierNodes = new Vector3[3];
            _bezierNodes[0] = drawOriginPosition;
            _bezierNodes[2] = parent.position;
            _bezierNodes[1] = _bezierNodes[0] + (_bezierNodes[2] - _bezierNodes[0]) / 2 +
                              Vector3.up * DRAW_CURVE_HEIGHT;
        }

        public void Render([NotNull] Card cardEntity)
        {
            _cardNameText.text = cardEntity.Name;
            _cardClassText.text = cardEntity.Class;
            _cardDescriptionText.text = cardEntity.Description();

            _cardIconSpriteImage.sprite = cardEntity.IconSprite;
            _cardTypeSpriteImage.sprite = cardEntity switch
            {
                ActionCard => _actionCardPanelSprite,
                ItemCard => _itemCardPanelSprite,
                _ => throw new NotImplementedException($"The item type '{cardEntity.GetType()}' is not" +
                                                       " implemented")
            };
        }

        public bool IsBeingDiscarded(){
            return _isBeingDiscarded;
        }
    }
}
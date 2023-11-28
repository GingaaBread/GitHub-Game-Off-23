using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using main.entity.Card_Management.Card_Data;
using main.service.Card_Management;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace main.view
{
    public class PlayerHandView : MonoBehaviour
    {
        private const float BASE_SPACING_AMOUNT = 100f;
        private const float CARD_SPACING_FACTOR = 15f;
        private const float CARD_ROTATION_FACTOR = 5f;
        private const float CARD_ROTATION_CAP = 45f;
        private const float CARD_CURVE_RADIUS_VERTICAL = 2500f;
        private const float CARD_CURVE_RADIUS_HORIZONTAL = 2500f;

        [SerializeField] private CardInHandContainer _cardViewContainerPrefab;
        [SerializeField] private HorizontalLayoutGroup _playerHandLayout;

        private int _drawOffset;
        private PlayerHandService playerHandService;

        private void OnEnable()
        {
            playerHandService.OnCardDrawn.AddListener(RenderNewCard);
            playerHandService.OnCardDiscarded.AddListener(RemoveCardAtIndex);
            playerHandService.OnHandDiscarded.AddListener(RemoveAll);

            _playerHandLayout.spacing = BASE_SPACING_AMOUNT;
        }

        private void OnDisable()
        {
            playerHandService.OnCardDrawn.RemoveListener(RenderNewCard);
            playerHandService.OnCardDiscarded.RemoveListener(RemoveCardAtIndex);
            playerHandService.OnHandDiscarded.RemoveListener(RemoveAll);
        }

        [Inject]
        public void Construct(PlayerHandService playerHandService)
        {
            this.playerHandService = playerHandService;
        }

        public void IncreaseSpacing()
        {
            _playerHandLayout.spacing += CARD_SPACING_FACTOR;
        }

        public void DecreaseSpacing()
        {
            _playerHandLayout.spacing -= CARD_SPACING_FACTOR;
        }

        private void RenderNewCard([NotNull] Card cardEntity)
        {
            var newCardViewContainer = Instantiate(_cardViewContainerPrefab, transform);
            _drawOffset++;
            StartCoroutine(CreateCardAfterTime(newCardViewContainer, cardEntity, _drawOffset));
            DecreaseSpacing();
        }

        private IEnumerator CreateCardAfterTime(
            [NotNull] CardInHandContainer container,
            [NotNull] Card cardEntity,
            int offset)
        {
            // Guarantee to wait one frame
            yield return new WaitForEndOfFrame();

            // Now create a slight draw offset
            yield return new WaitForSeconds(0.1f * offset);

            container.CreateChild(cardEntity, this);
            ApplyRotationAndOffsetToChildren();
        }

        private void RemoveCardAtIndex(int index)
        {
            var cardViewToRemove = _playerHandLayout.transform.GetChild(index);
            Destroy(cardViewToRemove.gameObject);
            IncreaseSpacing();
            ApplyRotationAndOffsetToChildren();
        }

        private void RemoveAll()
        {
            _drawOffset = 0;

            foreach (Transform child in _playerHandLayout.transform) Destroy(child.gameObject);

            _playerHandLayout.spacing = BASE_SPACING_AMOUNT;
        }

        public void ApplyRotationAndOffsetToChildren(){
            List<CardInHandContainer> realChildren = new();
            foreach(Transform child in _playerHandLayout.transform){
                CardInHandContainer component = child.GetComponent<CardInHandContainer>();
                if(component) realChildren.Add(component);
            }
            int currentRotationFactor = Mathf.RoundToInt(-realChildren.Count / 2);
            foreach(CardInHandContainer cardInHand in realChildren){
                if(realChildren.Count <= 1){
                    cardInHand.ApplyRotation(0);
                    cardInHand.ApplyOffset(0);
                }
                else{
                    float rotation = Mathf.Clamp(currentRotationFactor * -CARD_ROTATION_FACTOR, -CARD_ROTATION_CAP, CARD_ROTATION_CAP);
                    cardInHand.ApplyRotation(rotation);
                    float theta = (90f - Mathf.Abs(rotation)) * Mathf.PI / 180;
                    float sinSqared = (1 - Mathf.Cos(theta * 2))/2;
                    float cosSquared = 1 - sinSqared;
                    float floatRadiusForOffset = (CARD_CURVE_RADIUS_HORIZONTAL * CARD_CURVE_RADIUS_VERTICAL)/Mathf.Sqrt((CARD_CURVE_RADIUS_HORIZONTAL * CARD_CURVE_RADIUS_HORIZONTAL * sinSqared)+(CARD_CURVE_RADIUS_VERTICAL * CARD_CURVE_RADIUS_VERTICAL * cosSquared));//r = (ab)/sqrt((a^2)sinSq(theta)+(b^2)cosSq(theta))
                    float offsetY = (floatRadiusForOffset * Mathf.Sin(theta)) - CARD_CURVE_RADIUS_VERTICAL;
                    cardInHand.ApplyOffset(offsetY);
                    currentRotationFactor++;
                    if(currentRotationFactor == 0 && realChildren.Count % 2 == 0){
                        currentRotationFactor++;
                    }
                }
            }
        }
    }
}
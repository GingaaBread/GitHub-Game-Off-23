using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FMODUnity;
using main.entity.Card_Management.Card_Data;
using main.service.Card_Management;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace main.view
{
    public class PlayerHandView : MonoBehaviour
    {
        private const float BASE_SPACING_AMOUNT = 25f;
        private const float CARD_SPACING_FACTOR = 15f;
        private const float CARD_ROTATION_FACTOR = 5f;
        private const float CARD_ROTATION_CAP = 45f;
        private const float CARD_CURVE_RADIUS_VERTICAL = 2500f;
        private const float CARD_CURVE_RADIUS_HORIZONTAL = 2500f;

        [SerializeField] private CardInHandContainer _cardViewContainerPrefab;
        [SerializeField] private HorizontalLayoutGroup _playerHandLayout;
        [SerializeField] private StudioEventEmitter _cardDrawEvent;

        private PlayerHandService playerHandService;
        private DiscardPileService discardPileService;

        private readonly List<CardInHandContainer> cardInHandContainers = new();
        private int _drawOffset;

        [Inject]
        public void Construct(PlayerHandService playerHandService, DiscardPileService discardPileService)
        {
            this.playerHandService = playerHandService;
            this.discardPileService = discardPileService;
        }

        private void OnEnable()
        {
            playerHandService.OnCardDrawn.AddListener(RenderNewCard);
            discardPileService.OnDiscard += RemoveCard;

            _playerHandLayout.spacing = BASE_SPACING_AMOUNT;
        }

        private void OnDisable()
        {
            playerHandService.OnCardDrawn.RemoveListener(RenderNewCard);
            discardPileService.OnDiscard -= RemoveCard;
        }

        public void IncreaseSpacing()
        {
            _playerHandLayout.spacing += CARD_SPACING_FACTOR;
        }

        public void DecreaseSpacing()
        {
            _playerHandLayout.spacing -= CARD_SPACING_FACTOR;
        }

        public void PlayCard(Card card)
        {
            playerHandService.PlayCard(card);
        }

        private void RenderNewCard([NotNull] Card cardEntity)
        {
            var newCardViewContainer = Instantiate(_cardViewContainerPrefab, transform);
            cardInHandContainers.Add(newCardViewContainer);
            StartCoroutine(CreateCardAfterTime(newCardViewContainer, cardEntity));
            ApplyRotationAndOffsetToChildren();
        }
        
        private IEnumerator CreateCardAfterTime([NotNull] CardInHandContainer container, [NotNull] Card cardEntity)
        {
            // Guarantee to wait one frame
            yield return new WaitForEndOfFrame();

            // Now create a slight draw offset
            _drawOffset++;
            yield return new WaitForSeconds(0.1f  * _drawOffset);
            _drawOffset--;
            
            _cardDrawEvent.Play();
            container.CreateChild(cardEntity, this, playerHandService);
        }

        private void RemoveCard(Card card)
        {
            var cardInHandContainer = cardInHandContainers.Find(container => container.CardView.Card == card);
            cardInHandContainer.CardView.Discard();
            cardInHandContainers.Remove(cardInHandContainer);
            Destroy(cardInHandContainer.gameObject);
        }

        public void ApplyRotationAndOffsetToChildren(){
            List<CardInHandContainer> realChildren = new();
            foreach(Transform child in _playerHandLayout.transform){
                CardInHandContainer component = child.GetComponent<CardInHandContainer>();
                if(component && !component.IsBeingDiscarded()) realChildren.Add(component);
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
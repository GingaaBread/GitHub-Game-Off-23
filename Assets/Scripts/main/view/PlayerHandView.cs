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
        private const float CARD_ROTATION_FACTOR = 15f;
        private const float CARD_ROTATION_CAP = 60f;

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
            ApplyRotationToChildren();
        }

        private void RemoveCardAtIndex(int index)
        {
            var cardViewToRemove = _playerHandLayout.transform.GetChild(index);
            Destroy(cardViewToRemove.gameObject);
            IncreaseSpacing();
            ApplyRotationToChildren();
        }

        private void RemoveAll()
        {
            _drawOffset = 0;

            foreach (Transform child in _playerHandLayout.transform) Destroy(child.gameObject);

            _playerHandLayout.spacing = BASE_SPACING_AMOUNT;
        }

        public void ApplyRotationToChildren(){
            List<CardInHandContainer> realChildren = new List<CardInHandContainer>();
            foreach(Transform child in _playerHandLayout.transform){
                CardInHandContainer component = child.GetComponent<CardInHandContainer>();
                if(component && !component.IsBeingDiscarded()) realChildren.Add(component);
            }
            int currentRotationFactor = Mathf.RoundToInt(-realChildren.Count / 2);
            foreach(CardInHandContainer cardInHand in realChildren){
                if(realChildren.Count <= 1)cardInHand.ApplyRotation(0);
                else{
                    cardInHand.ApplyRotation(Mathf.Clamp(currentRotationFactor * -CARD_ROTATION_FACTOR, -CARD_ROTATION_CAP, CARD_ROTATION_CAP));
                    currentRotationFactor++;
                    if(currentRotationFactor == 0 && realChildren.Count % 2 == 0){
                        currentRotationFactor++;
                    }
                }
            }
        }
    }
}
using System.Diagnostics.CodeAnalysis;
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

        [SerializeField] private CardInHandContainer _cardViewContainerPrefab;
        [SerializeField] private HorizontalLayoutGroup _playerHandLayout;
        
        private PlayerHandService playerHandService;
        
        [Inject]
        public void Construct(PlayerHandService playerHandService)
        {
            this.playerHandService = playerHandService;
        }

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
            newCardViewContainer.CreateChild(cardEntity, this);
            DecreaseSpacing();
        }

        private void RemoveCardAtIndex(int index)
        {
            var cardViewToRemove = _playerHandLayout.transform.GetChild(index);
            Destroy(cardViewToRemove.gameObject);
            IncreaseSpacing();
        }

        private void RemoveAll()
        {
            foreach (Transform child in _playerHandLayout.transform) Destroy(child.gameObject);

            _playerHandLayout.spacing = BASE_SPACING_AMOUNT;
        }
    }
}
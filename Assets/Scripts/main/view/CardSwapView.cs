using System.Collections.Generic;
using JetBrains.Annotations;
using main.entity.Card_Management.Card_Data;
using main.service.Turn_System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace main.view
{
    public class CardSwapView : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private GameObject _deckSelectionContainer;
        [SerializeField] private GameObject _cardOfferContainer;
        [SerializeField] private TMP_Text[] _deckSelectionButtonTexts;
        [SerializeField] private TMP_Text[] _deckOfferButtonTexts;

        [FormerlySerializedAs("_confirmationButton")] [SerializeField]
        private Button _removalConfirmationButton;

        [SerializeField] private Button _additionConfirmationButton;

        private List<Card> _cardsThatCanBeRemovedFromDeck, _cardsThatCanBeAddedToDeck;
        private Card _selectedCardToRemove, _selectedCardToAdd;

        private CardSwapService cardSwapService;
        
        [Inject]
        public void Construct(CardSwapService cardSwapService)
        {
            this.cardSwapService = cardSwapService;
        }

        private void OnEnable()
        {
            cardSwapService.OnCardSwapOptions.AddListener(Render);
        }

        private void OnDisable()
        {
            cardSwapService.OnCardSwapOptions.RemoveListener(Render);
        }

        private void Start()
        {
            _container.SetActive(false);
            _deckSelectionContainer.SetActive(false);
            _cardOfferContainer.SetActive(false);
        }

        private void Render([NotNull] List<Card> cardsThatCanBeRemovedFromDeck,
            [NotNull] List<Card> cardsThatCanBeAddedToDeck)
        {
            Assert.AreEqual(3, cardsThatCanBeRemovedFromDeck.Count,
                "Should always have three selections when swapping cards");

            _container.SetActive(true);
            _cardOfferContainer.SetActive(false);
            _deckSelectionContainer.SetActive(true);

            _removalConfirmationButton.interactable = false;
            _additionConfirmationButton.interactable = false;

            _selectedCardToRemove = null;
            _selectedCardToAdd = null;
            _cardsThatCanBeRemovedFromDeck = cardsThatCanBeRemovedFromDeck;
            _cardsThatCanBeAddedToDeck = cardsThatCanBeAddedToDeck;

            for (var i = 0; i < cardsThatCanBeRemovedFromDeck.Count; i++)
                _deckSelectionButtonTexts[i].text = cardsThatCanBeRemovedFromDeck[i].Name;
        }

        public void ConfirmRemoval()
        {
            _deckSelectionContainer.SetActive(false);
            _cardOfferContainer.SetActive(true);

            for (var i = 0; i < _cardsThatCanBeAddedToDeck.Count; i++)
                _deckOfferButtonTexts[i].text = _cardsThatCanBeAddedToDeck[i].Name;

            _additionConfirmationButton.interactable = false;
        }

        public void ConfirmAddition()
        {
            _deckSelectionContainer.SetActive(false);
            _cardOfferContainer.SetActive(false);
            _container.SetActive(false);

            cardSwapService.RegisterCardSwapSelections(_selectedCardToRemove, _selectedCardToAdd);
        }

        public void SelectCardToRemove(int index)
        {
            Assert.IsTrue(index >= 0, "Should only select cards at a positive index");
            Assert.IsTrue(index < 3, "Should only select cards up to index 2");
            Assert.IsNotNull(_cardsThatCanBeRemovedFromDeck, "Card Lists have not been set up, yet.");

            _selectedCardToRemove = _cardsThatCanBeRemovedFromDeck[index];
            _removalConfirmationButton.interactable = true;
        }

        public void SelectCardToAdd(int index)
        {
            Assert.IsTrue(index >= 0, "Should only select cards at a positive index");
            Assert.IsTrue(index < 3, "Should only select cards up to index 2");
            Assert.IsNotNull(_cardsThatCanBeAddedToDeck, "Card Lists have not been set up, yet.");

            _selectedCardToAdd = _cardsThatCanBeAddedToDeck[index];
            _additionConfirmationButton.interactable = true;
        }
    }
}
using System.Diagnostics.CodeAnalysis;
using main.entity.Card_Management.Card_Data;
using main.service.Card_Management;
using UnityEngine;
using UnityEngine.UI;

namespace main.view
{
    public class PlayerHandView : MonoBehaviour
    {
        private const float BASE_SPACING_AMOUNT = 100f;
        private const float CARD_SPACING_FACTOR = 15f;
        private const float CARD_ROTATION_FACTOR = 15f;
        private const float CARD_MAX_ROTATION = 45f;
        [SerializeField] private CardView _cardViewPrefab;
        [SerializeField] private HorizontalLayoutGroup _playerHandLayout;

        private void Start()
        {
            PlayerHandService.Instance.OnCardDrawn.AddListener(RenderNewCard);
            PlayerHandService.Instance.OnCardDiscarded.AddListener(RemoveCardAtIndex);
            PlayerHandService.Instance.OnHandDiscarded.AddListener(RemoveAll);

            _playerHandLayout.spacing = BASE_SPACING_AMOUNT;
        }

        private void RenderNewCard([NotNull] Card cardEntity)
        {
            var newCardView = Instantiate(_cardViewPrefab, transform);
            newCardView.Render(cardEntity);
            _playerHandLayout.spacing -= CARD_SPACING_FACTOR;
            ApplyRotationToChildren();
        }

        private void RemoveCardAtIndex(int index)
        {
            var cardViewToRemove = _playerHandLayout.transform.GetChild(index);
            Destroy(cardViewToRemove.gameObject);
            _playerHandLayout.spacing += CARD_SPACING_FACTOR;
        }

        private void RemoveAll()
        {
            foreach (Transform child in _playerHandLayout.transform) Destroy(child.gameObject);

            _playerHandLayout.spacing = BASE_SPACING_AMOUNT;
        }

        private void ApplyRotationToChildren(){
            int currentRotationFactor = Mathf.RoundToInt(-_playerHandLayout.transform.childCount / 2);
            foreach(Transform child in _playerHandLayout.transform){
                child.transform.rotation = Quaternion.Euler(0,0,Mathf.Clamp(currentRotationFactor * -CARD_ROTATION_FACTOR, -CARD_MAX_ROTATION, CARD_MAX_ROTATION));
                currentRotationFactor++;
                if(currentRotationFactor == 0 && _playerHandLayout.transform.childCount % 2 == 0){
                    currentRotationFactor++;
                }
            }
        }
    }
}
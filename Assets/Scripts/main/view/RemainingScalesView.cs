using main.service.Turn_System;
using TMPro;
using UnityEngine;
using Zenject;

namespace main.view
{
    public class RemainingScalesView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _remainingScalesText;
        [SerializeField] private TMP_Text _previewAmountText;
        [SerializeField] private GameObject _previewContainer;
        private EffectAssemblyService _effectAssemblyService;
        private int _previewAmount;
        private TurnService _turnService;

        private void Start()
        {
            RemovePreview();
        }

        private void OnEnable()
        {
            _effectAssemblyService.OnScalePreviewHasChanged.AddListener(UpdatePreview);
            _turnService.OnNewTurnStart.AddListener(RemovePreview);
        }

        private void OnDisable()
        {
            _effectAssemblyService.OnScalePreviewHasChanged.RemoveListener(UpdatePreview);
            _turnService.OnNewTurnStart.RemoveListener(RemovePreview);
        }

        [Inject]
        public void Construct(EffectAssemblyService effectAssemblyService,
            TurnService turnService)
        {
            _effectAssemblyService = effectAssemblyService;
            _turnService = turnService;
        }

        private void RemovePreview()
        {
            _previewAmount = 0;
            _previewContainer.SetActive(false);
        }

        private void UpdatePreview(int incrementedScaleAmount)
        {
            _previewAmount += incrementedScaleAmount;
            _previewContainer.SetActive(true);
            _previewAmountText.text = _previewAmount.ToString();
        }

        public void Render(int remainingScales)
        {
            if (remainingScales < 0) remainingScales = 0;
            _remainingScalesText.text = remainingScales.ToString();
        }
    }
}
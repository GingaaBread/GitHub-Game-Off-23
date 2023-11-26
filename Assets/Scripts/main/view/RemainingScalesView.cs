using main.service.Fish_Management;
using TMPro;
using UnityEngine;
using Zenject;

namespace main.view
{
    public class RemainingScalesView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _remainingScalesText;
        
        private FishService fishService;
        
        [Inject]
        public void Construct(FishService fishService)
        {
            this.fishService = fishService;
        }

        private void Start()
        {
            fishService.OnFishScalesHaveChanged.AddListener(Render);
            Render(100);
        }

        private void Render(int remainingScales)
        {
            _remainingScalesText.text = remainingScales.ToString();
        }
    }
}
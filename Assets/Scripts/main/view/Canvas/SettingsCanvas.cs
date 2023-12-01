using Core;
using UnityEngine;
using UnityEngine.UI;

namespace main.view.Canvas
{
    public class SettingsCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private Toggle _muteSoundsToggle;
        [SerializeField] private Toggle _muteMusicToggle;
        [SerializeField] private Toggle _enableCameraFollowToggle;

        public void Render(bool enabled)
        {
            _container.SetActive(enabled);
            if (!enabled) return;

            _muteSoundsToggle.isOn = GameSettingsManager.muteSounds;
            _muteMusicToggle.isOn = GameSettingsManager.muteMusic;
            _enableCameraFollowToggle.isOn = GameSettingsManager.enableCameraFollow;
        }

        public void OnResetSettingsButtonClick()
        {
            GameSettingsManager.LoadDefaultValues();
            _muteSoundsToggle.isOn = GameSettingsManager.muteSounds;
            _muteMusicToggle.isOn = GameSettingsManager.muteMusic;
            _enableCameraFollowToggle.isOn = GameSettingsManager.enableCameraFollow;
        }

        public void OnSaveAndExitButtonClick()
        {
            GameSettingsManager.muteMusic = _muteMusicToggle.isOn;
            GameSettingsManager.muteSounds = _muteSoundsToggle.isOn;
            GameSettingsManager.enableCameraFollow = _enableCameraFollowToggle.isOn;

            GameSettingsManager.Save();

            Render(false);
        }
    }
}
using UnityEngine;

namespace Core
{
    public static class GameSettingsManager
    {
        public static bool muteAudio;
        public static int volumeStep;

        public static void Load()
        {
            if (HasBeenSetOnce())
            {
                muteAudio = PlayerPrefs.GetInt("muteAudio") == 1;
                volumeStep = PlayerPrefs.GetInt("volumeStep");
            }
            else
            {
                LoadDefaultValues();
            }
        }

        public static void Save()
        {
            PlayerPrefs.SetInt("muteAudio", muteAudio ? 1 : 0);
            PlayerPrefs.SetInt("volumeStep", volumeStep);
            AudioManager.UpdateBus();
        }

        private static void LoadDefaultValues()
        {
            muteAudio = false;
            volumeStep = 10;
        }

        private static bool HasBeenSetOnce()
        {
            return PlayerPrefs.HasKey("muteAudio");
        }
    }
}
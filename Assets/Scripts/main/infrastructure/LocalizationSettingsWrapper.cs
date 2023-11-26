using UnityEngine.Assertions;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace main.infrastructure
{
    public class LocalizationSettingsWrapper
    {
        public static LocaleIdentifier SelectedLocaleIdentifier
        {
            get
            {
                Assert.IsNotNull(LocalizationSettings.SelectedLocale, "There is no locale selected");
                
                return LocalizationSettings.SelectedLocale.Identifier;
            }
        }
    }
}
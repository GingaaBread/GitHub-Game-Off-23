using main.entity.Card_Management.Deck_Definition;
using main.infrastructure;
using main.repository;
using main.repository.Card_Management.Deck_Definition;
using NUnit.Framework;
using UnityEngine.Localization.Settings;

namespace test.EditMode.integration.main.repository.Card_Management.Deck_Definition
{
    [TestFixture]
    public class ResourceDeckDefinitionRepositoryTest
    {
        private DeckDefinitionRepository repository;
        
        [TestFixture]
        public class LoadDeckDefinitionTest : ResourceDeckDefinitionRepositoryTest
        {
            [Test]
            public void VerifySingleStarterDeckDefinitionResourceFound()
            {
                Assert.IsNotEmpty(LocalizationSettings.AvailableLocales.Locales, 
                    "There should be at least one available locale");
                
                foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
                {
                    LocalizationSettings.SelectedLocale = locale;
                    
                    var resourceLoader = new ResourceLoader<StarterDeckDefinition>(ResourcePath.StarterDeck);
                
                    repository = new DeckDefinitionRepository(resourceLoader);
                
                    var result = repository.LoadDeckDefinition();
            
                    Assert.NotNull(result);
                
                    Assert.That(result, Is.TypeOf<StarterDeckDefinition>());
                }
            }
        
            [Test]
            public void VerifySingleCardPoolDeckDefinitionResourceFound()
            {
                Assert.IsNotEmpty(LocalizationSettings.AvailableLocales.Locales, 
                    "There should be at least one available locale");

                foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
                {
                    LocalizationSettings.SelectedLocale = locale;
                    
                    var resourceLoader = new ResourceLoader<CardPoolDeckDefinition>(ResourcePath.CardPool);
                
                    repository = new DeckDefinitionRepository(resourceLoader);
                
                    var result = repository.LoadDeckDefinition();
            
                    Assert.NotNull(result);
                
                    Assert.That(result, Is.TypeOf<CardPoolDeckDefinition>());
                }
            }
        }
    }
}
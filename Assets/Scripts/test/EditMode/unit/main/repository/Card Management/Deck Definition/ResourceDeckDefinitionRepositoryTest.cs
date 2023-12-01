using main.entity.Card_Management.Card_Data;
using main.entity.Card_Management.Deck_Definition;
using main.repository;
using main.repository.Card_Management.Deck_Definition;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using AssertionException = UnityEngine.Assertions.AssertionException;

namespace test.EditMode.unit.main.repository.Card_Management.Deck_Definition
{
    [TestFixture]
    public class ResourceDeckDefinitionRepositoryTest
    {
        private IResourceLoader<DeckDefinition> resourceLoader;
        
        private DeckDefinitionRepository repository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            resourceLoader = Substitute.For<IResourceLoader<DeckDefinition>>();
            
            repository = new DeckDefinitionRepository(resourceLoader);
        }
        
        [TestFixture]
        public class LoadDeckDefinitionTest : ResourceDeckDefinitionRepositoryTest
        {
            [Test]
            public void WhenSingleResourceFound_returnDeckDefinition()
            {
                var deckDefinition = ScriptableObject.CreateInstance<StarterDeckDefinition>();
                
                var card = ScriptableObject.CreateInstance<ActionCard>();
                
                var cardCopies = new CardCopies(card, 1);
                
                deckDefinition.CardCopiesList.Add(cardCopies);

                resourceLoader.GetAll().Returns(new DeckDefinition[]{ deckDefinition });
                
                var result = repository.LoadDeckDefinition();
            
                Assert.That(result, Is.EqualTo(deckDefinition));
            }
        
            [Test]
            public void WhenNoResourceFound_throwException()
            {
                const string path = "TestPath";
            
                resourceLoader.ResourcePath.Returns(path);
            
                resourceLoader.GetAll().Returns(new DeckDefinition[] {});
                
                var exception = Assert.Throws<AssertionException>(() => repository.LoadDeckDefinition());
            
                Assert.That(exception.Message,
                    Contains.Substring($"There should be exactly one deck definition in Resources path: \"{path}\""));
            }
        
            [Test]
            public void WhenMultipleResourcesFound_throwException()
            {
                const string path = "TestPath";
            
                resourceLoader.ResourcePath.Returns(path);
            
                var deckDefinition1 = ScriptableObject.CreateInstance<StarterDeckDefinition>();
                var deckDefinition2 = ScriptableObject.CreateInstance<StarterDeckDefinition>();
            
                resourceLoader.GetAll().Returns(new DeckDefinition[] { deckDefinition1, deckDefinition2 });
                
                var exception = Assert.Throws<AssertionException>(() => repository.LoadDeckDefinition());
            
                Assert.That(exception.Message,
                    Contains.Substring($"There should be exactly one deck definition in Resources path: \"{path}\""));
            }
            
            [Test]
            public void WhenNoCardCopiesInDeck_throwException()
            {
                var deckDefinition = ScriptableObject.CreateInstance<StarterDeckDefinition>();
                
                var card = ScriptableObject.CreateInstance<ActionCard>();
                
                var cardCopies = new CardCopies(card, 0);
                
                deckDefinition.CardCopiesList.Add(cardCopies);
                
                resourceLoader.GetAll().Returns(new DeckDefinition[]{ deckDefinition });
                
                var exception = Assert.Throws<AssertionException>(() => repository.LoadDeckDefinition());
            
                Assert.That(exception.Message,
                    Contains.Substring("There should be at least one copy of a card in the Deck Definition"));
            }
        }
    }
}
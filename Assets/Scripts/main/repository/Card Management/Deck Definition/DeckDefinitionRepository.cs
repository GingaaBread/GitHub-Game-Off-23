using System.Linq;
using main.entity.Card_Management.Deck_Definition;
using UnityEngine.Assertions;

namespace main.repository.Card_Management.Deck_Definition
{
    public class DeckDefinitionRepository
    {
        private readonly IResourceLoader<DeckDefinition> resourceLoader;

        public DeckDefinitionRepository(IResourceLoader<DeckDefinition> resourceLoader)
        {
            this.resourceLoader = resourceLoader;
        }

        public DeckDefinition LoadDeckDefinition()
        {
            return LoadFromResources();
        }
        
        private DeckDefinition LoadFromResources()
        {
            var definition = resourceLoader.GetAll();

            Assert.AreEqual(definition.Length, 1,
                $"There should be exactly one deck definition in Resources path: \"{resourceLoader.ResourcePath}\"");

             Assert.IsTrue(definition[0].CardCopiesList.Any(copies => copies.NumberOfCopies > 0),
                 "There should be at least one copy of a card in the Deck Definition");

            return definition[0];
        }
    }
}
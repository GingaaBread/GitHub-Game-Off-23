using main.entity.Card_Management;
using main.repository;
using main.repository.Card_Management.Deck_Definition;
using main.service.Card_Management;
using Zenject;

namespace main.infrastructure
{
    public class StarterDeckServiceInstaller : MonoInstaller<StarterDeckServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<StarterDeck>().AsSingle();

            Container.Bind<StarterDeckService>()
                .FromMethod(CreateStarterDeckService)
                .AsSingle()
                .NonLazy();
        }

        private static StarterDeckService CreateStarterDeckService(InjectContext ctx)
        {
            var repositoryStarterDeck = ctx.Container.ResolveId<DeckDefinitionRepository>(ResourcePath.StarterDeck.GetValue());
            return new StarterDeckService(ctx.Container.Resolve<StarterDeck>(), repositoryStarterDeck);
        }
    }
}
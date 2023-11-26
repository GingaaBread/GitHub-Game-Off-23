using main.entity.Card_Management.Deck_Definition;
using main.repository;
using Zenject;

namespace main.infrastructure
{
    public class ResourceLoaderInstaller : MonoInstaller<ResourceLoaderInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<LocalizationSettingsWrapper>().AsSingle();
            
            Container.Bind<IResourceLoader<DeckDefinition>>()
                .WithId(ResourcePath.StarterDeck.GetValue())
                .FromMethod(ctx => CreateResourceLoader(ctx, ResourcePath.StarterDeck))
                .AsTransient();

            Container.Bind<IResourceLoader<DeckDefinition>>()
                .WithId(ResourcePath.CardPool.GetValue())
                .FromMethod(ctx => CreateResourceLoader(ctx, ResourcePath.CardPool))
                .AsTransient();
        }
        
        private static ResourceLoader<DeckDefinition> CreateResourceLoader(InjectContext ctx, ResourcePath resourcePath)
        {
            var localizationSettings = ctx.Container.Resolve<LocalizationSettingsWrapper>();
            return new ResourceLoader<DeckDefinition>(localizationSettings, resourcePath);
        }
    }
}
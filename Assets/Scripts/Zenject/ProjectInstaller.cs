using EazyQuiz.Unity;
using Zenject;

/// <summary>
/// Инсталлер для Zenject
/// </summary>
public class ProjectInstaller : MonoInstaller
{ 
    public override void InstallBindings()
    {
        Container.Bind<ApiProvider>().AsCached().NonLazy();
        Container.Bind<UserService>().AsCached().NonLazy();

        Container.Bind<QuestionsService>().AsTransient().NonLazy();
        Container.Bind<GameOverScreen>().AsCached();

        Container.Bind<AuthController>().AsCached();
        Container.Bind<HistoryController>().AsCached();
        Container.Bind<GameController>().AsCached();
        Container.Bind<MainmenuController>().AsCached();
    }
}
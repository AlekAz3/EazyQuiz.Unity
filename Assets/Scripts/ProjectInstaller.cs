using EazyQuiz.Unity;
using Zenject;

public class ProjectInstaller : MonoInstaller
{ 
    public override void InstallBindings()
    {
        Container.Bind<ApiProvider>().AsTransient().NonLazy();
        Container.Bind<UserService>().AsSingle().NonLazy();

        Container.Bind<QuestionsService>().AsTransient().NonLazy();
        Container.Bind<GameOverScreen>().AsSingle();

        Container.Bind<AuthController>().AsSingle();
        Container.Bind<HistoryController>().AsSingle();
        Container.Bind<GameController>().AsSingle();
        Container.Bind<MainmenuController>().AsSingle();
    }
}
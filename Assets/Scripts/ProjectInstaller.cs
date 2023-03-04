using EazyQuiz.Unity;
using System.Collections.Generic;
using Zenject;

public class ProjectInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        Container.Bind<ApiProvider>().AsTransient().NonLazy();
        Container.Bind<UserService>().AsSingle().NonLazy();

        Container.Bind<GameOverScreen>().AsSingle();

        Container.Bind<AuthController>().AsSingle();
        Container.Bind<GameController>().AsSingle();
        Container.Bind<MainmenuController>().AsSingle();
    }
}
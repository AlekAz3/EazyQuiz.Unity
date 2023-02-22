using EazyQuiz.Models.DTO;
using EazyQuiz.Unity;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        Container.Bind<ApiProvider>().AsTransient().NonLazy();
        Container.Bind<UserService>().AsSingle().NonLazy();

        Container.Bind<AuthController>().AsSingle();
        Container.Bind<GameController>().AsSingle();
        Container.Bind<MainmenuController>().AsSingle();
    }
}
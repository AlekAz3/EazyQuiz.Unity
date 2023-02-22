using EazyQuiz.Models.DTO;
using EazyQuiz.Unity;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private GameObject error;

    public override void InstallBindings()
    {
        Container.Bind<ApiProvider>().AsTransient().NonLazy();
        Container.Bind<UserService>().AsSingle().NonLazy();
        var errorr = Container.InstantiatePrefabForComponent<ErrorScreenService>(error);
        Container.Bind<ErrorScreenService>().FromInstance(errorr).AsSingle().NonLazy();

        Container.Bind<AuthController>().AsSingle();
        Container.Bind<GameController>().AsSingle();
        Container.Bind<MainmenuController>().AsSingle();
    }
}
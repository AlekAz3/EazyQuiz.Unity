using UnityEngine;
using Zenject;

public class PopupInstaller : MonoInstaller
{
    [SerializeField] private GameObject loadingScreenGameObject;

    public override void InstallBindings()
    {
        var loadingScreen = Container.InstantiatePrefabForComponent<LoadingScreen>(loadingScreenGameObject);

        Container.Bind<LoadingScreen>()
            .FromInstance(loadingScreen)
            .AsSingle()
            .NonLazy();
    }
}
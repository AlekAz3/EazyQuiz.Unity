using EazyQuiz.Unity.Services;
using Zenject;

namespace EazyQuiz.Unity.Zenject
{
    /// <summary>
    /// Инсталлер для Zenject
    /// </summary>
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ApiProvider>().AsCached().NonLazy();
            Container.Bind<UserService>().AsCached().NonLazy();
            Container.Bind<SwitchSceneService>().AsCached();
            Container.Bind<SaveUserService>().AsSingle();
            Container.Bind<QuestionsService>().AsTransient().NonLazy();
        }
    }
}
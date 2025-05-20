using UnityEngine;
using Zenject;

namespace BoxSortingGame
{
    public class SceneGameplayInstaller : MonoInstaller
    {
        [SerializeField] private BoxManager _boxManager;
        [SerializeField] private PoolManager _poolManager;
        
        public override void InstallBindings()
        {
            Container.Bind<PoolManager>().
                FromInstance(_poolManager).
                AsSingle();

            Container.Bind<BoxManager>().
                FromInstance(_boxManager).
                AsSingle();
        }
    }
}

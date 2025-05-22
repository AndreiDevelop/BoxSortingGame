using UnityEngine;
using Zenject;

namespace BoxSortingGame
{
    public class SceneGameplayInstaller : MonoInstaller
    {
        [Header("SO")]
        [SerializeField] private BoxSettingsSO _boxSettingsSO;
        [SerializeField] private DropZoneSettingsSO _dropZoneSettingsSO;
        
        [Header("Managers")]
        [SerializeField] private PoolManager _poolManager;
        
        public override void InstallBindings()
        {
            Container.Bind<PoolManager>().
                FromInstance(_poolManager).
                AsSingle();

            var boxModel = new BoxModel(_boxSettingsSO, _poolManager);
            Container.Bind<BoxModel>().
                FromInstance(boxModel).
                AsSingle();
            
            var dropZoneModel = new DropZoneModel(_dropZoneSettingsSO);
            Container.Bind<DropZoneModel>().
                FromInstance(dropZoneModel).
                AsSingle();
        }
    }
}

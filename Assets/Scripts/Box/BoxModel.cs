using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using Zenject;

namespace BoxSortingGame
{
    public class BoxModel
    {
        public ReactiveCommand<BoxController> OnBoxSpawned = new ReactiveCommand<BoxController>();
        
        private PoolManager _poolManager;
        private BoxSettingsSO _boxSettings;
        private int _spawnedBoxCount = 0;
        
        public BoxModel(BoxSettingsSO boxSettings, PoolManager poolManager)
        {
            _poolManager = poolManager;
            _boxSettings = boxSettings;

            _poolManager.AddToPool<BoxController>(_boxSettings.BoxPrefab, _boxSettings.MaxBoxCount);
            
            SpawnBoxProcess().
                Forget();
        }

        public async UniTask SpawnBox()
        {
            float boxChance = UnityEngine.Random.Range(0f, 1f);
            var boxColor = _boxSettings.GetBoxColor(boxChance);
            
            var boxObject = await _poolManager.
                GetFromPool<BoxController>();
            
            //TODO refactoring
            var boxController = boxObject.GetComponent<BoxController>();
            
            boxController.Initialize(this, boxColor);
            _spawnedBoxCount++;

            OnBoxSpawned?.Execute(boxController);
        }

        public async UniTask SpawnBoxProcess()
        {
            while (_spawnedBoxCount<_boxSettings.MaxBoxCount)
            {
                await UniTask.WaitForSeconds(_boxSettings.BoxSpawnDelayInSeconds);
                await SpawnBox();
            }
        }
    }
}

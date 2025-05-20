using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using Zenject;

namespace BoxSortingGame
{
    //TODO refactoring remove MonoBehaviour
    public class BoxManager : MonoBehaviour
    {
        [SerializeField] private BoxSettingsSO _boxSettings;
        [SerializeField] private Transform _boxSpawnPoint;
        
        public ReactiveCommand<BoxController> OnBoxSpawned = new ReactiveCommand<BoxController>();
        
        [Inject] private PoolManager _poolManager;

        private int _spawnedBoxCount = 0;
        
        private void Start()
        {
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
            
            boxObject.transform.position = _boxSpawnPoint.position;
            
            //TODO refactoring
            var boxController = boxObject.GetComponent<BoxController>();
            
            boxController.Initialize(this, boxColor);
            _spawnedBoxCount++;
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

using UniRx;
using UnityEngine;
using Zenject;

namespace BoxSortingGame
{
    public class BoxSpawnerPresenter : MonoBehaviour
    {
        [SerializeField] private float _boxActivationSpeed;
        
        [Inject] private BoxModel _boxModel;

        public void Start()
        {
            _boxModel.OnBoxSpawned.Subscribe(box =>
            {
                SpawnBox(box);
            }).AddTo(this);
        }
        
        public void SpawnBox(BoxController box)
        {
            box.transform.SetParent(transform);
            box.transform.position = transform.position;
            
            box.Activate(transform.forward * _boxActivationSpeed);
        }
    }
}

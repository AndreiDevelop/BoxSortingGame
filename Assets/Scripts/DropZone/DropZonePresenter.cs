using UniRx;
using UnityEngine;
using Zenject;

namespace BoxSortingGame
{
    public class DropZonePresenter : MonoBehaviour
    {
        [SerializeField] private ColorDataSO _colorDataSO;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        [Inject] private DropZoneModel _dropZoneModel;
        
        private string ColorId => _colorDataSO.Data.id;
        
        private void Start()
        {
            _spriteRenderer.color = _colorDataSO.Data.color;
            
            _dropZoneModel.OnDropZoneSearching.Subscribe(colorData =>
            {
                //drop zone finded
                if (colorData.id.Equals(ColorId))
                {
                    _dropZoneModel.FindDropZone(transform);
                }
            }).AddTo(this);
        }
    }
}

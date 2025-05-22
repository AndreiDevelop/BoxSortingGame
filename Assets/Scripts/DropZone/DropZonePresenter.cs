using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace BoxSortingGame
{
    public class DropZonePresenter : MonoBehaviour
    {
        [SerializeField] private ColorDataSO _colorDataSO;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TextMeshPro _boxCountText;
        
        [Inject] private DropZoneModel _dropZoneModel;

        private DropZoneData _dropZoneData;
        private string ColorId => _dropZoneData.colorData.id;
        private string DropZoneId => _dropZoneData.id;
        
        private void Start()
        {
            _spriteRenderer.color = _colorDataSO.Data.color;
            _boxCountText.color = _colorDataSO.Data.color;
            
            _dropZoneData = _dropZoneModel.
                RegistrateDropZone(_colorDataSO.Data);
            
            _dropZoneModel.OnDropZoneSearching.Subscribe(colorData =>
            {
                //drop zone finded
                if (colorData.id.Equals(ColorId))
                {
                    _dropZoneModel.SelectDropZone(DropZoneId, transform);
                }
            }).AddTo(this);
            
            _dropZoneModel.OnDropZoneDataChanged.Subscribe(dropZoneData =>
            {
                if (dropZoneData.id.Equals(DropZoneId))
                {
                    _dropZoneData = dropZoneData;
                    UpdateTextCount();
                }
            }).AddTo(this);

            UpdateTextCount();
        }

        private void UpdateTextCount()
        {
            _boxCountText.text = $"{_dropZoneData.boxCount}/{_dropZoneModel.MaxBoxCount}";
        }
    }
}

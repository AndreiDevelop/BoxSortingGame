using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace BoxSortingGame
{
    [System.Serializable]
    public struct DropZoneData
    {
        public string id;
        public ColorData colorData;
        public int boxCount;
    }
    
    public class DropZoneModel
    {
        public ReactiveCommand<DropZoneData> OnDropZoneFull = new ReactiveCommand<DropZoneData>();
        public ReactiveCommand<DropZoneData> OnDropZoneDataChanged = new ReactiveCommand<DropZoneData>();
        public ReactiveCommand<Transform> OnDropZoneFinded = new ReactiveCommand<Transform>();
        public ReactiveCommand<ColorData> OnDropZoneSearching = new ReactiveCommand<ColorData>();
        
        private List<DropZoneData> _dropZoneDatas = new List<DropZoneData>();

        private string _selectedDropZoneId;
        private DropZoneSettingsSO _dropZoneSettings;
        public int MaxBoxCount => _dropZoneSettings.MaxBoxCount;
        
        public DropZoneModel(DropZoneSettingsSO dropZoneSettings)
        {
            _dropZoneSettings = dropZoneSettings;
        }
        
        public DropZoneData RegistrateDropZone(ColorData colorData)
        {
            var dropZoneData = new DropZoneData
            {
                id = Guid.NewGuid().ToString(),
                colorData = colorData,
                boxCount = 0
            };
            
            _dropZoneDatas.Add(dropZoneData);

            return dropZoneData;
        }
        
        public void DropBoxOnSelectedDropZone()
        {
            var dropZoneDataIndex = _dropZoneDatas.FindIndex(x => x.id.Equals(_selectedDropZoneId));
            
            var selectedDropZone = _dropZoneDatas[dropZoneDataIndex];
            
            var newDropZoneData = new DropZoneData
            {
                id = selectedDropZone.id,
                colorData = selectedDropZone.colorData,
                boxCount = selectedDropZone.boxCount + 1
            };
            
            _dropZoneDatas[dropZoneDataIndex] = newDropZoneData;

            if (newDropZoneData.boxCount >= _dropZoneSettings.MaxBoxCount)
            {
                OnDropZoneFull?.Execute(newDropZoneData);
            }
            
            OnDropZoneDataChanged?.Execute(newDropZoneData);
        }
        
        public void SearchForDropZone(ColorData colorData)
        {
            OnDropZoneSearching?.Execute(colorData);
        }
        
        public void SelectDropZone(string dropZoneId, Transform dropZoneTransform)
        {
            _selectedDropZoneId = dropZoneId;
            OnDropZoneFinded?.Execute(dropZoneTransform);
        }
    }
}

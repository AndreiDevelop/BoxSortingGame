using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
        public ReactiveCommand<DropZoneData> OnBoxDroped = new ReactiveCommand<DropZoneData>();
        public ReactiveCommand<Transform> OnDropZoneFinded = new ReactiveCommand<Transform>();
        public ReactiveCommand<ColorData> OnDropZoneSearching = new ReactiveCommand<ColorData>();
        public ReactiveDictionary<string, List<Transform>> DropZoneBoxes = new ReactiveDictionary<string, List<Transform>>();
        
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
        
        public void DropBoxOnSelectedDropZone(Transform box)
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
            
            AddBoxToDropZone(newDropZoneData.id, box);
            OnBoxDroped?.Execute(newDropZoneData);
        }

        private void AddBoxToDropZone(string dropZoneId, Transform boxTransform)
        {
            if(DropZoneBoxes.ContainsKey(dropZoneId))
            {
                DropZoneBoxes[dropZoneId].Add(boxTransform);
            }
            else
            {
                DropZoneBoxes.Add(dropZoneId, new List<Transform>
                {
                    boxTransform
                });
            }
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

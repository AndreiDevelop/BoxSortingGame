using UnityEngine;
using UniRx;

namespace BoxSortingGame
{
    public class DropZoneModel
    {
        public ReactiveCommand<Transform> OnDropZoneFinded = new ReactiveCommand<Transform>();
        public ReactiveCommand<ColorData> OnDropZoneSearching = new ReactiveCommand<ColorData>();
        
        public void SearchForDropZone(ColorData colorData)
        {
            OnDropZoneSearching?.Execute(colorData);
        }
        
        public void FindDropZone(Transform dropZoneTransform)
        {
            OnDropZoneFinded?.Execute(dropZoneTransform);
        }
    }
}

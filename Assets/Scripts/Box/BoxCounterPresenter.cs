using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace BoxSortingGame
{
    public class BoxCounterPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _boxCountText;
        
        [Inject] private BoxModel _boxModel;

        void Start()
        {
            _boxModel.Boxes.ObserveCountChanged().Subscribe(boxesCount =>
            {
                UpdateTextCount(boxesCount);
            }).AddTo(this);

            UpdateTextCount(0);
        }
        
        private void UpdateTextCount(int newCount)
        {
            _boxCountText.text = $"{newCount}/{_boxModel.MaxBoxCount}";
        }
    }
}

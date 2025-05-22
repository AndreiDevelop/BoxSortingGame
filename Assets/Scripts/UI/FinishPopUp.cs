using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BoxSortingGame
{
    public class FinishPopUp : MonoBehaviour
    {
        [SerializeField] private GameObject _holder;
        [SerializeField] private TextMeshProUGUI _textDescription;
        [SerializeField] private Button _buttonOk;
        
        [Inject] private DropZoneModel _dropZoneModel;

        void Start()
        {
            _dropZoneModel.OnDropZoneFull
                .Subscribe(dropZone =>
                {
                    Activate("The game is finished!", dropZone.colorData.color);
                })
                .AddTo(this);
        }
        
        public void Activate(string text, Color color)
        {
            _textDescription.color = color;
            _textDescription.text = text;
            _holder.SetActive(true);
        }
        
        private void OnEnable()
        {
            _buttonOk.onClick.AddListener(LoadMenuScene);
        }

        private void OnDisable()
        {
            _buttonOk.onClick.RemoveListener(LoadMenuScene);
        }

        private void LoadMenuScene()
        {
            SceneLoadingManager.Instance.LoadScene(Constants.SceneMenu);
        }
    }
}

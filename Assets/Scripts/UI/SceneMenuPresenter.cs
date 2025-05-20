using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BoxSortingGame
{
    public class SceneMenuPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textFPS;
        [SerializeField] private Button _buttonExitMainMenu;
        
        private int _frameCount = 0;
        private float _elapsedTime = 0f;
        
        private void OnEnable()
        {
            _buttonExitMainMenu.onClick.AddListener(OnButtonExitMainMenuClicked);
        }

        private void OnDisable()
        {
            _buttonExitMainMenu.onClick.RemoveListener(OnButtonExitMainMenuClicked);
        }
        
        private void OnButtonExitMainMenuClicked()
        {
            SceneLoadingManager.Instance.LoadScene(Constants.SceneMenu);
        }

        void Update()
        {
            _frameCount++;
            _elapsedTime += Time.unscaledDeltaTime;

            if (_elapsedTime >= 1f)
            {
                int fps = Mathf.RoundToInt(_frameCount / _elapsedTime);
                _textFPS.text = $"FPS: {Mathf.Ceil(fps)}";
                _frameCount = 0;
                _elapsedTime = 0f;
            }
           
        }
    }
}

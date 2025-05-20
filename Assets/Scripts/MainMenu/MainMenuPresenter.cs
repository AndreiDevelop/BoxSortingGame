using UnityEngine;
using UnityEngine.UI;

namespace BoxSortingGame
{
    public class MainMenuPresenter : MonoBehaviour
    {
        [SerializeField] private Button _buttonPlay;

        private void OnEnable()
        {
            _buttonPlay.onClick.AddListener(ButtonPlayClicked);
        }

        private void OnDisable()
        {
            _buttonPlay.onClick.RemoveListener(ButtonPlayClicked);
        }
        
        private void ButtonPlayClicked()
        {
            SceneLoadingManager.Instance.LoadScene(Constants.SceneGameplay);
        }
    }
}

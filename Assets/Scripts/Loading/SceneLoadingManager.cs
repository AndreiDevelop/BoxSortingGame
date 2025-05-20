using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace BoxSortingGame
{
    public class SceneLoadingManager : MonoBehaviour
    {
        public static SceneLoadingManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            StartLoading();
        }

        //load scene first time
        private async UniTask StartLoading()
        {
            await UniTask.WaitForSeconds(1);

            LoadSceneAdditive(Constants.SceneMenu);
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.UnloadSceneAsync(Constants.SceneLoading);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void LoadSceneAdditive(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
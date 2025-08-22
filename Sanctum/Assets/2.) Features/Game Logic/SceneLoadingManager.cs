using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadingManager : MonoBehaviour
{
    private enum DefaultLoadScene {DEFAULT, MAINMENU, TESTING, SANDBOX, HQ }
    
    [Header("These can be found from a the Loading-Canvas Prefab")] [Space] [SerializeField]
    private GameObject _loadingScreen;

    [SerializeField] private Slider _loadingSliderValue;
    [SerializeField] private TextMeshProUGUI _loadingText;
    [SerializeField] private DefaultLoadScene _defaultLoadScene;
    private List<AsyncOperation> _loadingSceneOperations;
    private float _sceneLoadingProgress;

    private void Awake()
    {
        _loadingSceneOperations = new List<AsyncOperation>();
        ResetLoadingProgress();
        
        switch (_defaultLoadScene)
        {
            case DefaultLoadScene.DEFAULT:
            {
                LoadScene(KeywordDictionary.Scenes.MainMenu, LoadSceneMode.Additive);
                break;
            }
            case DefaultLoadScene.TESTING:
            {
                LoadScene(KeywordDictionary.Scenes.Testing, LoadSceneMode.Additive);
                break;
            }
            case DefaultLoadScene.MAINMENU:
            {
                LoadScene(KeywordDictionary.Scenes.MainMenu, LoadSceneMode.Additive);
                break;
            }
            case DefaultLoadScene.SANDBOX:
            {
                LoadScene(KeywordDictionary.Scenes.Sandbox, LoadSceneMode.Additive);
                break;
            }
            case DefaultLoadScene.HQ:
            {
                LoadScene(KeywordDictionary.Scenes.GamePlay, LoadSceneMode.Additive);
                break;
            }
        }
    }

    public void LoadScene(KeywordDictionary.Scenes sceneToLoad, LoadSceneMode loadType)
    {
        if (sceneToLoad != null && loadType != null)
        {
            _loadingScreen.SetActive(true);

            int activeSceneIndex = -1;

            switch (sceneToLoad)
            {
                case KeywordDictionary.Scenes.GameManager:
                {
                    _loadingSceneOperations.Add(SceneManager.LoadSceneAsync((int)KeywordDictionary.Scenes.GameManager,
                        LoadSceneMode.Additive));
                    break;
                }
                case KeywordDictionary.Scenes.MainMenu:
                {
                    _loadingSceneOperations.Add(SceneManager.LoadSceneAsync((int)KeywordDictionary.Scenes.MainMenu,
                        LoadSceneMode.Additive));
                    break;
                }
                case KeywordDictionary.Scenes.GamePlay:
                {
                    //_loadingSceneOperations.Add(SceneManager.UnloadSceneAsync((int)KeywordDictionary.Scenes.MainMenu));
                    _loadingSceneOperations.Add(SceneManager.LoadSceneAsync((int)KeywordDictionary.Scenes.GamePlay,
                        LoadSceneMode.Additive));
                    activeSceneIndex = (int)KeywordDictionary.Scenes.GamePlay;
                    break;
                }
                case KeywordDictionary.Scenes.GameUI:
                {
                    _loadingSceneOperations.Add(SceneManager.LoadSceneAsync((int)KeywordDictionary.Scenes.GameUI,
                        LoadSceneMode.Additive));
                    break;
                }
                case KeywordDictionary.Scenes.Testing:
                {
                    _loadingSceneOperations.Add(SceneManager.LoadSceneAsync((int)KeywordDictionary.Scenes.Testing,
                        LoadSceneMode.Additive));
                    activeSceneIndex = (int)KeywordDictionary.Scenes.Testing;
                    break;
                }
                case KeywordDictionary.Scenes.Sandbox:
                {
                    _loadingSceneOperations.Add(SceneManager.LoadSceneAsync((int)KeywordDictionary.Scenes.Sandbox,
                        LoadSceneMode.Additive));
                    activeSceneIndex = (int)KeywordDictionary.Scenes.Sandbox;
                    break;
                }
            }

            StartCoroutine(LoadSceneAsync(activeSceneIndex));
        }
    }

    private IEnumerator LoadSceneAsync(int activeSceneIndex = -1)
    {
        foreach (AsyncOperation scene in _loadingSceneOperations)
        {
            while (!scene.isDone)
            {
                _sceneLoadingProgress = 0;
                foreach (AsyncOperation operation in _loadingSceneOperations)
                {
                    _sceneLoadingProgress += operation.progress;
                }

                _sceneLoadingProgress = (_sceneLoadingProgress / _loadingSceneOperations.Count) * 100f;
                _loadingSliderValue.value = Mathf.RoundToInt(_sceneLoadingProgress);
                _loadingText.text = ("Loading... " + _loadingSliderValue.value);
                yield return null;
            }
        }

        for (int i = 0; i < _loadingSceneOperations.Count; i++)
        {
            while (!_loadingSceneOperations[i].isDone)
            {
                _sceneLoadingProgress = 0;
                foreach (AsyncOperation operation in _loadingSceneOperations)
                {
                    _sceneLoadingProgress += operation.progress;
                }

                _sceneLoadingProgress = (_sceneLoadingProgress / _loadingSceneOperations.Count) * 100f;
                _loadingSliderValue.value = Mathf.RoundToInt(_sceneLoadingProgress);
                _loadingText.text = ("Loading... " + _loadingSliderValue.value.ToString());
                yield return null;
            }
        }

        _loadingScreen.SetActive(false);
        if (activeSceneIndex != -1)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(activeSceneIndex));
        }

        yield return null;
    }

    private void ResetLoadingProgress()
    {
        _sceneLoadingProgress = 0;
        _loadingSliderValue.value = 0;
    }
}
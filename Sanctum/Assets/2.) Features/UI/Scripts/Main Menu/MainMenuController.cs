using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // Buttons
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _backButton;

    [SerializeField] private UIPanel[] _views;

   private Dictionary<KeywordDictionary.MainMenuPanel, UIPanel> _mainMenuPanelDict;
    private Stack<KeywordDictionary.MainMenuPanel> panelHistory;
    private KeywordDictionary.MainMenuPanel currentPanel;
    
    private const float CardShakeStrength = 0.1f;
    private const float CardShakeDuration = 0.2f;
    private const int CardShakeVibratto = 2;
    private static readonly Vector3 CardShakeScale = new Vector3(0, CardShakeStrength, 0);

    private void Awake()
    {
        InitializePanelDictionaries();
        panelHistory = new Stack<KeywordDictionary.MainMenuPanel>();
    }

    private void OnEnable()
    {
        SwitchPanel(KeywordDictionary.MainMenuPanel.Home);
        _playButton.onClick.AddListener(OnPlayClick);
        _settingsButton.onClick.AddListener(OnSettingsClick);
        _quitButton.onClick.AddListener(OnQuitClick);
        _backButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(OnPlayClick);
        _settingsButton.onClick.RemoveListener(OnSettingsClick);
        _quitButton.onClick.RemoveListener(OnQuitClick);
        _backButton.onClick.RemoveListener(OnBackButtonPressed);
    }

    private void InitializePanelDictionaries()
    {
        _mainMenuPanelDict = new Dictionary<KeywordDictionary.MainMenuPanel, UIPanel>();
        foreach (UIPanel panel in _views)
        {
            _mainMenuPanelDict.Add(panel.GetPanelType(), panel);
        }
    }
    
    public void ShowPanel(KeywordDictionary.MainMenuPanel panelType)
    {
        if (_mainMenuPanelDict.TryGetValue(panelType, out var panel))
        {
            panel.Show();
        }
    }

    public void HidePanel(KeywordDictionary.MainMenuPanel panelType)
    {
        if (_mainMenuPanelDict.TryGetValue(panelType, out var panel))
        {
            panel.Hide();
        }
    }

    public void SwitchPanel(KeywordDictionary.MainMenuPanel panelType)
    {
        if (currentPanel != null && !panelType.Equals(currentPanel))
        {
            if (panelHistory.Count == 0 || panelHistory.Peek() != currentPanel)
            {
                panelHistory.Push(currentPanel);
            }
            HidePanel(currentPanel);
        }

        if (!panelType.Equals(currentPanel))
        {
            ShowPanel(panelType);
            currentPanel = panelType;
        }

        if (currentPanel != KeywordDictionary.MainMenuPanel.Home)
        {
            _backButton.gameObject.SetActive(true);
        }
        else
        {
            _backButton.gameObject.SetActive(false);
        }
    }

    private void OnBackButtonPressed()
    {
        if (panelHistory.Count > 0)
        {
            KeywordDictionary.MainMenuPanel previousPanel = panelHistory.Pop();
            SwitchPanel(previousPanel);
        }
    }
    
    private void OnPlayClick()
    {
        _playButton.transform.DOPunchScale(CardShakeScale, CardShakeDuration, CardShakeVibratto, 1f).OnComplete(() =>
        {
            // GameManager.Instance.ServiceLocator.GetService<SceneLoadingManager>().LoadScene(KeywordDictionary.Scenes.GamePlay, LoadSceneMode.Additive);
            SwitchPanel(KeywordDictionary.MainMenuPanel.PlayerSelection);
        });
    }

    private void OnSettingsClick()
    {
        _settingsButton.transform.DOPunchScale(CardShakeScale, CardShakeDuration, CardShakeVibratto, 1f).OnComplete(() => {
            SwitchPanel(KeywordDictionary.MainMenuPanel.Settings);
        });
    }

    private void OnQuitClick()
    {
        _settingsButton.transform.DOPunchScale(CardShakeScale, CardShakeDuration, CardShakeVibratto, 1f).OnComplete(() => {
            Application.Quit();
        });
    }
}
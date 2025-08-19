using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private KeywordDictionary.MainMenuPanel _panelType;
    public void Show()
    {
        _content.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _content.gameObject.SetActive(false);
    }

    public KeywordDictionary.MainMenuPanel GetPanelType()
    {
        return _panelType;
    }

    public GameObject GetPanelContent()
    {
        return _content;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectionController : MonoBehaviour
{
    [SerializeField] private Button _balancedButton, _fastButton, _toughButton, _athleticButton, _selectButton;
    [SerializeField] private Slider _healthSlider, _armorSlider, _damageSlider, _energySlider;
    [SerializeField] private TMP_Text _titleText, _titleDescription;
    [SerializeField] private Image _titleIcon, _characterIcon;
    [SerializeField] private Sprite _balanedCharacter, _fastCharacter, _toughCharacter, _athleticCharacter;
    private List <CharacterSelectionButton> _buttons;

    private void Awake()
    {
        _buttons = new List<CharacterSelectionButton>();
        _buttons.Add(_balancedButton.GetComponentInParent<CharacterSelectionButton>());
        _buttons.Add(_fastButton.GetComponentInParent<CharacterSelectionButton>());
        _buttons.Add(_toughButton.GetComponentInParent<CharacterSelectionButton>());
        _buttons.Add(_athleticButton.GetComponentInParent<CharacterSelectionButton>());
    }

    private void OnEnable()
    {
        _balancedButton.onClick.AddListener(OnBalancedClick);
        _selectButton.onClick.AddListener(OnSelectClick);
        OnBalancedClick();
    }
    
    private void OnDisable()
    {
        _balancedButton.onClick.RemoveListener(OnBalancedClick);
        _selectButton.onClick.RemoveListener(OnSelectClick);
    }

    private void OnSelectClick()
    {
        GameManager.Instance.PlayerRepository.InitializeNewPlayer();
        GameManager.Instance.ServiceLocator.GetService<SceneLoadingManager>().LoadScene(KeywordDictionary.Scenes.GamePlay, LoadSceneMode.Additive);
    }

    private void OnBalancedClick()
    {
        _characterIcon.sprite = _balanedCharacter;
        _titleIcon.sprite = _balancedButton.transform.GetChild(0).GetComponent<Image>().sprite;
        _titleText.text = "Balanced";
        _titleDescription.text = "A well tuned character that doesn't excel in any specific stat area. Well balanced and tuned for any situation.";
        foreach (CharacterSelectionButton button in _buttons)
        {
            button.UnFocusButton();
        }
        _balancedButton.GetComponentInParent<CharacterSelectionButton>().FocusButton();
       
        _healthSlider.value = 0.85f;
        _armorSlider.value = 0.85f;
        _damageSlider.value = 0.85f;
        _energySlider.value = 0.85f;
    }
}
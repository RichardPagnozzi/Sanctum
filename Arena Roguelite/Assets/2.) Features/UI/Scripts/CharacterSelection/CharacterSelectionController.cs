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
    private KeywordDictionary.PlayerCharacterType _characterType;

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
        _fastButton.onClick.AddListener(OnAgileClick);
        _toughButton.onClick.AddListener(OnToughClick);
        _athleticButton.onClick.AddListener(OnAthleticClick);
        _selectButton.onClick.AddListener(OnSelectClick);
        OnBalancedClick();
    }
    
    
    private void OnDisable()
    {
        _balancedButton.onClick.RemoveListener(OnBalancedClick);
        _fastButton.onClick.RemoveListener(OnAgileClick);
        _toughButton.onClick.RemoveListener(OnToughClick);
        _athleticButton.onClick.RemoveListener(OnAthleticClick);
        _selectButton.onClick.RemoveListener(OnSelectClick);
    }

    private void OnSelectClick()
    {
        GameManager.Instance.PlayerRepository.InitializeNewPlayer(_characterType);
        GameManager.Instance.ServiceLocator.GetService<SceneLoadingManager>().LoadScene(KeywordDictionary.Scenes.GamePlay, LoadSceneMode.Additive);
    }

    private void OnBalancedClick()
    {
        _characterIcon.sprite = _balanedCharacter;
        _titleIcon.sprite = _balancedButton.transform.GetChild(0).GetComponent<Image>().sprite;
        _titleText.text = "Balanced";
        _titleDescription.text =
            "A well tuned character that doesn't excel in any specific stat area. Well balanced and tuned for any situation.";
        _characterType = KeywordDictionary.PlayerCharacterType.Balanced;
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

    private void OnAgileClick()
    {
        _characterIcon.sprite = _fastCharacter;
        _titleIcon.sprite = _fastButton.transform.GetChild(0).GetComponent<Image>().sprite;
        _titleText.text = "Agile";
        _titleDescription.text = "A Fast Character. Boats a fast movement speed while draining energy quickly. Great for repositioning and weaving in and out of the battlefield.";
        _characterType = KeywordDictionary.PlayerCharacterType.Fast;

        foreach (CharacterSelectionButton button in _buttons)
        {
            button.UnFocusButton();
        }
        _fastButton.GetComponentInParent<CharacterSelectionButton>().FocusButton();
        
        _healthSlider.value = 0.5f;
        _armorSlider.value = 0.8f;
        _damageSlider.value = 0.8f;
        _energySlider.value = 0.9f;
    }

    private void OnToughClick()
    {
        _characterIcon.sprite = _toughCharacter;
        _titleIcon.sprite = _toughButton.transform.GetChild(0).GetComponent<Image>().sprite;
        _titleText.text = "Tough";
        _titleDescription.text = "A Tank Character. Boasts a high health and armor rating while balanced with a lower movement speed. Stamina Drain is slow.";
        _characterType = KeywordDictionary.PlayerCharacterType.Tough;

        foreach (CharacterSelectionButton button in _buttons)
        {
            button.UnFocusButton();
        }
        _toughButton.GetComponentInParent<CharacterSelectionButton>().FocusButton();
        
        _healthSlider.value = 1;
        _armorSlider.value = 0.90f;
        _damageSlider.value = 0.8f;
        _energySlider.value = 0.75f;
    }

    private void OnAthleticClick()
    {
        _characterIcon.sprite = _athleticCharacter;
        _titleIcon.sprite = _athleticButton.transform.GetChild(0).GetComponent<Image>().sprite;
        _titleText.text = "Athletic";
        _titleDescription.text = "A High Endurance Character. Can run for a while without needing to worry about energy. High stamina recharge rate, slight boosts to health and movement speed make this a" +
                                 "very flexible character.";
        _characterType = KeywordDictionary.PlayerCharacterType.Athletic;

        foreach (CharacterSelectionButton button in _buttons)
        {
            button.UnFocusButton();
        }
        _athleticButton.GetComponentInParent<CharacterSelectionButton>().FocusButton();
        
        _healthSlider.value = 0.75f;
        _armorSlider.value = 0.8f;
        _damageSlider.value = 0.6f;
        _energySlider.value = 1f;
    }
}
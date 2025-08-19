    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class PlayerSelectionCard : MonoBehaviour
    {
        public enum PlayerSelectionCardType
        {
            Empty, 
            Filled,
            Locked
        }

        [SerializeField] private PlayerSelectionCardType _cardType;
       
        [Header("Scene References")]
        // Buttons
        [SerializeField] private Button _cardButton;
        [SerializeField] private Button _deletePlayerButton;
        // Images
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _cardIcon;
        // Text
        [SerializeField] private TMP_Text _playerNameText;
        [SerializeField] private TMP_Text _playerLevelText;
        // GameObjects
        [SerializeField] private GameObject _filledCardUIContainer;

        [Header("Project References")] 
        [SerializeField] private Sprite _emptyIcon;
        [SerializeField] private Sprite _lockedIcon;
        [SerializeField] private Sprite _emptyBG;
        [SerializeField] private Sprite _filledBG;
        [SerializeField] private Sprite _lockedBG;

        private Action cardClickAction;

        
        public void Subscribe()
        {
            _cardButton.onClick.AddListener(OnCardClick);

        }

        public void UnSubscribe()
        {
            _cardButton.onClick.RemoveListener(OnCardClick);

        }

        private void SetCard(PlayerDetails playerDetails = null)
        {
            switch (_cardType)
            {
                case PlayerSelectionCardType.Empty:
                {
                    SetEmptyCardProperties();
                    break;
                }
                case PlayerSelectionCardType.Filled:
                {
                    SetFilledCardProperties(playerDetails);
                    break;
                }
                case PlayerSelectionCardType.Locked:
                {
                    SetLockedCardProperties();
                    break;
                }
            }
        }

        private void SetEmptyCardProperties()
        {
            _filledCardUIContainer.SetActive(false);
            _cardIcon.gameObject.SetActive(true); 
            _cardIcon.sprite = _emptyIcon;
            _backgroundImage.sprite = _emptyBG;
            cardClickAction = OnEmptyCardClick;
            _cardButton.interactable = true;
        }

        private void SetFilledCardProperties(PlayerDetails playerDetails = null)
        {
            _cardIcon.gameObject.SetActive(false);
            _filledCardUIContainer.SetActive(true);
            _backgroundImage.sprite = _filledBG;
            if (playerDetails != null)
            {
                _playerNameText.text = playerDetails.CharacterType.ToString();
                _playerLevelText.text = $"Level: {playerDetails.Stats.Level}";
            }
            cardClickAction = OnFilledCardCLick;
            _cardButton.interactable = true;
        }

        private void SetLockedCardProperties()
        {
            _filledCardUIContainer.SetActive(false);
            _cardIcon.gameObject.SetActive(true); 
            _cardIcon.sprite = _lockedIcon;
            _backgroundImage.sprite = _lockedBG;
            cardClickAction = null;
            _cardButton.interactable = false;
        }

        public void SetCardType(PlayerSelectionCardType cardType, PlayerDetails playerDetails = null)
        {
            _cardType = cardType;
            SetCard(playerDetails);
        }

        private void OnCardClick()
        {
            if (cardClickAction != null)
            {
                cardClickAction();
            }
        }
        private void OnFilledCardCLick()
        {
            GameManager.Instance.ServiceLocator.GetService<SceneLoadingManager>().LoadScene(KeywordDictionary.Scenes.GamePlay, LoadSceneMode.Additive);
        }

        private void OnEmptyCardClick()
        {
            GameManager.Instance.ServiceLocator.GetService<MainMenuController>()
                .SwitchPanel(KeywordDictionary.MainMenuPanel.CreateCharacter);
        }
        
    }

using System;
using UnityEngine;

public class PlayerSelectionController : MonoBehaviour
{
    [SerializeField] private PlayerSelectionCard[] _playerSelectionCards;
    private bool _isPlayerInitialized;
    private PlayerDetails _playerDetails;
    private Action _cardButtonClickAction;

    
    private void OnEnable()
    {
        Initialize();
        foreach (PlayerSelectionCard card in _playerSelectionCards)
        {
            card.Subscribe();
        }
    }

    private void OnDisable()
    {
        foreach (PlayerSelectionCard card in _playerSelectionCards)
        {
            card.UnSubscribe();
        }
    }

    private void Initialize()
    {
        try
        {
            if (GameManager.Instance.PlayerRepository.CurrentSessionPlayerDetails == null)
            {
                _isPlayerInitialized = false;
            }
            else
            {
                _isPlayerInitialized = true;
                _playerDetails = GameManager.Instance.PlayerRepository.CurrentSessionPlayerDetails;
            }
        }
        catch (Exception e)
        {
            _isPlayerInitialized = false;
            DebugLogger.Log($"Could not find GameManager Instance", DebugLogger.LogStyle.Bold, Color.red);
        }
        
        if (_isPlayerInitialized)
        {
            LockAllCards();
            _playerSelectionCards[0].SetCardType(PlayerSelectionCard.PlayerSelectionCardType.Filled, _playerDetails);
        }
        else
        {
            LockAllCards();
            _playerSelectionCards[0].SetCardType(PlayerSelectionCard.PlayerSelectionCardType.Empty, _playerDetails);
        }
    }
    
    private void LockAllCards()
    {
        foreach (PlayerSelectionCard card in _playerSelectionCards)
        {
            card.SetCardType(PlayerSelectionCard.PlayerSelectionCardType.Locked);
        }
    }

    private void EmptyAllCards()
    {
        foreach (PlayerSelectionCard card in _playerSelectionCards)
        {
            card.SetCardType(PlayerSelectionCard.PlayerSelectionCardType.Empty);
        }
    }
    
}
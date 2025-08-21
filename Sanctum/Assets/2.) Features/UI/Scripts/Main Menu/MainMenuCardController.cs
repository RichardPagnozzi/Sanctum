using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuCardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text _cardLabel;
    private Vector3 _startingPosition, _scaledPosition;
    private const float CardScaledMultiplier = 1.025f;
    private const float CardScaledDuration = 0.25f;

    private void Awake()
    {
        _startingPosition = GetComponent<RectTransform>().localScale;
        _scaledPosition = _startingPosition * CardScaledMultiplier;
    }

    private void OnEnable()
    {
        DisableHoverStateUI(true);
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        EnableHoverStateUI();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DisableHoverStateUI(false);
    }
    
    private void EnableHoverStateUI()
    {
        // Set Bold Font
        _cardLabel.fontStyle = FontStyles.Bold;
        // Scale Card Up
        transform.DOScale(_scaledPosition, CardScaledDuration);
    }

    private void DisableHoverStateUI(bool onEnable)
    {
        // Set Normal Font
        _cardLabel.fontStyle = FontStyles.Normal;
        if (!onEnable)
        {
            // Scale Card Down
            transform.DOScale(_startingPosition, CardScaledDuration);            
        }
    }
}
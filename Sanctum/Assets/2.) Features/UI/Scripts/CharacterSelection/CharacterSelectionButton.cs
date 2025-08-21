using UnityEngine;

public class CharacterSelectionButton : MonoBehaviour
{
    [SerializeField] private GameObject _focusState;
    
    
    public void FocusButton()
    {
        _focusState.SetActive(true);
    }

    public void UnFocusButton()
    {
        _focusState.SetActive(false);
    }
}

using UnityEngine;

public class PlayerMeshPicker : MonoBehaviour
{
    [SerializeField] private Mesh balancedCharacterMesh;
    [SerializeField] private Mesh fastCharacterMesh;
    [SerializeField] private Mesh toughCharacterMesh;
    [SerializeField] private Mesh athleticCharacterMesh;

    [SerializeField] SkinnedMeshRenderer _renderer;
    private KeywordDictionary.PlayerCharacterType _characterType;

    private void Awake()
    {
        _characterType = GameManager.Instance.PlayerRepository.CurrentSessionPlayerDetails.CharacterType;
        SetMesh(_characterType);
    }
    
    
    public void SetMesh(KeywordDictionary.PlayerCharacterType _characterType)
    {
        if (balancedCharacterMesh == null || fastCharacterMesh == null ||  toughCharacterMesh == null ||  athleticCharacterMesh == null)
        {
            Debug.LogWarning("MeshSwapper: No mesh assigned.");
            return;
        }

        switch (_characterType)
        {
            case KeywordDictionary.PlayerCharacterType.Balanced:
            {
                _renderer.sharedMesh = balancedCharacterMesh;
                break;
            }
            case KeywordDictionary.PlayerCharacterType.Athletic:
            {
                _renderer.sharedMesh = athleticCharacterMesh;
                break;
            }
            case KeywordDictionary.PlayerCharacterType.Tough:
            {
                _renderer.sharedMesh = toughCharacterMesh;
                break;
            }
            case KeywordDictionary.PlayerCharacterType.Fast:
            {
                _renderer.sharedMesh = fastCharacterMesh;
                break;
            }
        }

    }
}

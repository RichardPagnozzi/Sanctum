using UnityEngine;

public class PlayerMeshPicker : MonoBehaviour
{
    [SerializeField] private Mesh balancedCharacterMesh;
    [SerializeField] private Mesh fastCharacterMesh;
    [SerializeField] private Mesh toughCharacterMesh;
    [SerializeField] private Mesh athleticCharacterMesh;

    [SerializeField] SkinnedMeshRenderer _renderer;

    private void Awake()
    {
        SetMesh();
    }
    
    
    public void SetMesh()
    {
        if (balancedCharacterMesh == null)
        {
            Debug.LogWarning("MeshSwapper: No mesh assigned.");
            return;
        }

        _renderer.sharedMesh = balancedCharacterMesh;
        
    }
}

using UnityEngine;

public class EnemyCharacterRandomizer : MonoBehaviour
{
    [SerializeField] private Transform _characterMeshParent;
    
    void Start()
    {
        ActivateRandomMesh();
    }

    private void ActivateRandomMesh()
    {
        // Get all child GameObjects
        int childCount = _characterMeshParent.childCount;
        if (childCount == 0) return; // Exit if there are no children

        // Pick a random index
        int randomIndex = Random.Range(0, childCount);

        // Activate the randomly selected child and deactivate others
        for (int i = 0; i < childCount; i++)
        {
            _characterMeshParent.GetChild(i).gameObject.SetActive(i == randomIndex);
        }
    }
}

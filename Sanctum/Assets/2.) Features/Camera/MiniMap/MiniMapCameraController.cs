using System;
using UnityEngine;

public class MiniMapCameraController : MonoBehaviour
{
    [SerializeField] private GameObject minimapCameraPrefab; 
    private Transform target; 
    private Camera minimapCameraInstance;
    private Transform _playerTransform;
    private const int _miniMapCameraHeight = 10;
    
    void Start()
    {
        if (minimapCameraPrefab != null)
        {
            GameObject cameraObj = Instantiate(minimapCameraPrefab);
            minimapCameraInstance = cameraObj.GetComponent<Camera>();
            minimapCameraInstance.transform.position = new Vector3(_playerTransform.transform.position.x,
                _playerTransform.transform.position.y + _miniMapCameraHeight, _playerTransform.transform.position.z);
        }
    }

    private void OnEnable()
    {
        while (target == null)
        {
            TrySetTarget();
        }
    }

    void LateUpdate()
    {
        if (target != null && target.gameObject.activeInHierarchy)
        {
            minimapCameraInstance.transform.position = new Vector3(target.position.x, minimapCameraInstance.transform.position.y, target.position.z);
            // Update the minimap camera's rotation to match the player's rotation
            minimapCameraInstance.transform.rotation = Quaternion.Euler(90f, target.eulerAngles.y, 0f);
        }
    }

    private void TrySetTarget()
    {
        try
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            _playerTransform = target;
        }
        catch(Exception e)
        {
            Debug.Log("MiniMap Could Not Locate Player: " + e);
        }
    }

}

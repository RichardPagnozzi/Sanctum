using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineBrain _cinemachineBrain;
    [SerializeField] private List<CinemachineCamera> _registeredCameras;
    [SerializeField] private int _priorityBoost = 10;

    private CinemachineCamera _activeCamera;
    private int _basePriority = 0; 

    private void Awake()
    {
        if (_cinemachineBrain == null)
            _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    private void Start()
    {
        // Default any available cameras to base priority 
        foreach (CinemachineCamera cam in _registeredCameras)
        {
            if (cam != null)
                cam.Priority = _basePriority;
        }

        // Default to first cam if set in editor
        if (_registeredCameras.Count > 0)
        {
            SwitchToCamera(_registeredCameras[0]);
        }
    }

    public void RegisterCamera(CinemachineCamera cam)
    {
        if (cam != null && !_registeredCameras.Contains(cam))
        {
            _registeredCameras.Add(cam);
            cam.Priority = _basePriority;
        }
    }

    public void SwitchToCamera(CinemachineCamera newCam, bool blend = true)
    {
        if (newCam == null || newCam == _activeCamera) return;

        // Reset old camera’s priority
        if (_activeCamera != null)
            _activeCamera.Priority = _basePriority;

        // Boost new camera’s priority
        newCam.Priority = _basePriority + _priorityBoost;
        _activeCamera = newCam;

        // // Optional: adjust blend style
        CinemachineBlendDefinition.Styles style = blend
            ? CinemachineBlendDefinition.Styles.EaseInOut
            : CinemachineBlendDefinition.Styles.Cut;
        
        _cinemachineBrain.DefaultBlend = new CinemachineBlendDefinition(
            style,
            _cinemachineBrain.DefaultBlend.BlendTime
        );
    }

    public void SwitchToCamera(string cameraName, bool blend = true)
    {
        CinemachineCamera cam = _registeredCameras.Find(c => c != null && c.name == cameraName);
        if (cam != null)
            SwitchToCamera(cam, blend);
    }

    public void SetFOV(float fov)
    {
        if (_activeCamera != null)
            _activeCamera.Lens.FieldOfView = fov;
    }

    public CinemachineCamera GetActiveCamera() => _activeCamera;
}

using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField] private GameObject _elevatorPlatform;
    [SerializeField] private Transform _floorBottom, _floorTop;
    [SerializeField] private bool _isRaised;


    
    public void MoveElevator()
    {
        if (_isRaised)
        {
            _elevatorPlatform.transform.DOMoveY(_floorBottom.position.y, 7, false).onComplete = () => _isRaised = false;
        }
        else
        {
            _elevatorPlatform.transform.DOMoveY(_floorTop.position.y, 7, false).onComplete = () => _isRaised = true;
        }
    }
     
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTurret : MonoBehaviour
{
    public bool EnableTurret;
    [SerializeField]
    private GameObject _projectile;
    [SerializeField]
    private Transform _shotPoint;

    [SerializeField] float velocity;
    [SerializeField] float waitTime;
    private IEnumerator FireProjectile()
    {
        while(EnableTurret)
        {
            yield return new WaitForSeconds(waitTime);
            Vector3 startPos = _shotPoint.position;
            Quaternion startRot = _shotPoint.rotation;
           // float velocity = 5000f;
            GameObject projectile = Instantiate(_projectile, startPos, startRot);
            projectile.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, velocity));
        }
    }

    private void Start()
    {
        StartCoroutine(FireProjectile());
    }
}

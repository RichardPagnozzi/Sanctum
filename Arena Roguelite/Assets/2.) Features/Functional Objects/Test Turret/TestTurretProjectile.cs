using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTurretProjectile : MonoBehaviour
{
    private float projectileDamge = 10f;
    private float lifeTime = 0;
    private bool collisionDetected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collisionDetected == false)
        {
            if (other.gameObject.tag == "Player")
            {
                GameManager.Instance.ServiceLocator.EventManager.OnPlayerRecieveDamage.Invoke(projectileDamge);
            }
            else if (other.gameObject.tag == "Enemy")
            {
                GameManager.Instance.ServiceLocator.EventManager.OnEnemyRecieveDamage(projectileDamge, other.gameObject, other.transform.position, false);
            }
        }
        collisionDetected = true;
        Destroy(this.gameObject);
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= 1.5f)
            Destroy(this.gameObject);
    }
}

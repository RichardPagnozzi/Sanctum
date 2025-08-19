using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSelfDestruct : MonoBehaviour
{
    float lifeTime = 0;


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= 3)
            Destroy(this.gameObject);
    }
}

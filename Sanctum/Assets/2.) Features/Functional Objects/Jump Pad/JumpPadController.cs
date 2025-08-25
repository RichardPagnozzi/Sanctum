using Player;

using UnityEngine;

public class JumpPadController : MonoBehaviour
{
    private float _launchHeight = 20;
    private float _gravityValue = -9.81f;
    private Vector3 moveDirection;

    private void Start()
    {
        moveDirection = Vector3.zero;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovementController>()?.TriggerJumpPadAction(_launchHeight);
        }
    }
}

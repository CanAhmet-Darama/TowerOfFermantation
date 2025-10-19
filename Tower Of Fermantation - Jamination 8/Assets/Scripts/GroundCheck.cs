using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    int layerMask = (0 << 6);
    RaycastHit hitInfo;
    public PlayerController player;

    private void OnTriggerStay(Collider other)
    {
        player.isGrounded = true;
        player.jumped = false;
    }
    private void OnTriggerExit(Collider other)
    {
        player.isGrounded = false;
    }
    private void Update()
    {
        Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hitInfo, 1, layerMask);
        player.groundSlope = hitInfo.normal;
    }
}

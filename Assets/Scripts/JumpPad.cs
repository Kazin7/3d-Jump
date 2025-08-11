using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPanel : MonoBehaviour
{
    public float jumpPower = 400;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Rigidbody playerRb = col.gameObject.GetComponent<Rigidbody>();

            if (playerRb != null)
            {
                playerRb.velocity = Vector3.zero;
                playerRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }
    }
}

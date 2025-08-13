using UnityEngine;
using System.Collections.Generic;

public class MovingBlock : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] float speed = 3f;
    [SerializeField] float minZ = -10f;
    [SerializeField] float maxZ = 6f;

    [Header("Carrier (Empty)")]
    [SerializeField] Transform carrierRoot;

    Rigidbody blockRb;
    Vector3 startLocalPosOfBlock;
    Vector3 lastPosition;

    private HashSet<Transform> riders = new HashSet<Transform>();

    void Awake()
    {
        if (carrierRoot == null)
            carrierRoot = transform.parent != null ? transform.parent : transform;

        blockRb = carrierRoot.GetComponent<Rigidbody>();

        //방어코드
        if (blockRb == null)
            blockRb = carrierRoot.gameObject.AddComponent<Rigidbody>();

        blockRb.isKinematic = true;
        blockRb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Start()
    {
        startLocalPosOfBlock = carrierRoot.localPosition;
        lastPosition = carrierRoot.position;
    }

    void FixedUpdate()
    {
        float zRange = Mathf.Abs(maxZ - minZ);
        float z = Mathf.PingPong(Time.time * speed, zRange) + Mathf.Min(minZ, maxZ);

        Vector3 lp = startLocalPosOfBlock;
        lp.z = z;

        Vector3 worldPos = carrierRoot.parent ? carrierRoot.parent.TransformPoint(lp) : lp;

        Vector3 deltaMove = worldPos - lastPosition;

        blockRb.MovePosition(worldPos);

        foreach (var rider in riders)
        {
            rider.position += deltaMove;
        }

        lastPosition = worldPos;
    }


    // private void OnCollisionEnter(Collision collision)
    // {
    //     riders.Add(collision.transform);
    // }

    // private void OnCollisionExit(Collision collision)
    // {
    //     riders.Remove(collision.transform);
    // }
}

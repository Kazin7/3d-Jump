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
    public Vector3 PlatformVelocity { get; private set; }
    Vector3 lastWorldPos; 

    // private HashSet<Transform> riders = new HashSet<Transform>();

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
        lastWorldPos = carrierRoot.position;
    }

    void FixedUpdate()
    {
        float zRange = Mathf.Abs(maxZ - minZ);
        float z = Mathf.PingPong(Time.time * speed, zRange) + Mathf.Min(minZ, maxZ);

        Vector3 lp = startLocalPosOfBlock;
        lp.z = z;

        Vector3 worldPos = carrierRoot.parent ? carrierRoot.parent.TransformPoint(lp) : lp;

        Vector3 delta = worldPos - lastWorldPos;
        PlatformVelocity = delta / Time.fixedDeltaTime;

        blockRb.MovePosition(worldPos);
        lastWorldPos = worldPos;


    }

    //충돌시 플레이어 자식으로 연결
    // MovingBlock.cs
    void OnCollisionEnter(Collision c)
    {
        var pc = c.transform.GetComponentInParent<PlayerController>();
        if (pc != null) pc.SetPlatform(this);
    }

    void OnCollisionExit(Collision c)
    {
        var pc = c.transform.GetComponentInParent<PlayerController>();
        if (pc != null) pc.ClearPlatform(this);
    }

}

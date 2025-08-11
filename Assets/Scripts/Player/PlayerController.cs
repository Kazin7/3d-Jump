using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;                 // 기본 이동속도(버프는 이 값에 가산)
    public float runMultiplier = 1.6f;           // 달리기 배수
    public float staminaUsePerSecond = 20f;       // 초당 스태미너 소모
    private bool isRunning;

    private Coroutine speedBoostCor;
    private Coroutine jumpBoostCor;
    private Vector2 curMovementInput;
    public float jumpPower = 80f;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook = -85f;
    public float maxXLook = 85f;
    private float camCurXRot;
    public float lookSensitivity = 0.2f;

    private Vector2 mouseDelta;

    [HideInInspector] public bool canLook = true;
    private Rigidbody rb;
    private PlayerCondition condition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        condition = GetComponent<PlayerCondition>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }
    public void OnRunInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)   isRunning = true;
        if (context.phase == InputActionPhase.Canceled)  isRunning = false;
    }
    // Move() 변경
    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        float targetSpeed = moveSpeed;
        if (isRunning && condition != null)
        {
            bool consumed = condition.UseStamina(staminaUsePerSecond * Time.fixedDeltaTime);
            if (consumed)
                targetSpeed = moveSpeed * runMultiplier;
            else
                isRunning = false;
        }

        dir *= targetSpeed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;
    }


    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }

    public void ApplySpeedBoost(float addAmount, float duration)
    {
        if (speedBoostCor != null) StopCoroutine(speedBoostCor);
        speedBoostCor = StartCoroutine(SpeedBoostRoutine(addAmount, duration));
    }

    private IEnumerator SpeedBoostRoutine(float add, float dur)
    {
        float original = moveSpeed;
        moveSpeed = original + add;
        yield return new WaitForSeconds(dur);
        moveSpeed = original;
        speedBoostCor = null;
    }
    public void ApplyJumpBoost(float addAmount, float duration)
    {
        if (jumpBoostCor != null) StopCoroutine(jumpBoostCor);
        jumpBoostCor = StartCoroutine(JumpBoostRoutine(addAmount, duration));
    }

    private IEnumerator JumpBoostRoutine(float add, float dur)
    {
        float original = jumpPower;
        jumpPower = original + add;
        yield return new WaitForSeconds(dur);
        jumpPower = original;
        jumpBoostCor = null;
    }
}
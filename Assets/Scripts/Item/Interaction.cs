using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float checkRadius = 2f;        // OverlapSphere 반경
    public LayerMask layerMask;           // 감지할 레이어

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            // 플레이어 주변에서 구 형태로 감지
            Collider[] hits = Physics.OverlapSphere(transform.position, checkRadius, layerMask);

            if (hits.Length > 0)
            {
                // 화면 중앙에 가까운 오브젝트 선택 (또는 가장 가까운 오브젝트)
                Transform closest = null;
                float minAngle = float.MaxValue;

                foreach (Collider col in hits)
                {
                    Vector3 dirToTarget = (col.transform.position - cam.transform.position).normalized;
                    float angle = Vector3.Angle(cam.transform.forward, dirToTarget);

                    // 시야 중심에 가장 가까운 대상 선택
                    if (angle < minAngle)
                    {
                        minAngle = angle;
                        closest = col.transform;
                    }
                }
                // 상호작용 처리
                if (closest != null && closest.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = closest.gameObject;
                    curInteractable = closest.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }
    //아이템 정보 띄우기
    private void SetPromptText()
    {
        if (curInteractable == null) return;
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }
    //e키 눌러서 상호작용시
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}

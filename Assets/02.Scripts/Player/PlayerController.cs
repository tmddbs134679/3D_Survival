using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerStat stat;
    private Vector2 curMovementInput;

    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform camContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    [Header("Dash")]
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public bool isDashing = false;
    private Vector3 dashDir;

    public float jumpPower;

    private Rigidbody rb;

    public bool CanLook = true;
    public Action inventroy;

    [Header("Hanging")]
    public LedgeDetector LedgeDetector;
    private bool isHanging = false;
    public Transform ledgeGrabPoint; // 손 위치 기준 (선택적)

    [Header("View")]
    private bool isTrans = true;
    public Camera mainCam;
    public Vector3 mainCamOffset;
    public CinemachineFreeLook cinemachineCam;

    private void Awake()
    {
        stat = GetComponent<PlayerStat>();  
        rb = GetComponent<Rigidbody>();
        mainCam.transform.localPosition = mainCamOffset;
    }
    // Start is called before the first frame update
    void Start()
    {
        //커서 숨기기
        Cursor.lockState = CursorLockMode.Locked;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isHanging)
        Move();
    }

    private void LateUpdate()
    {
        if(CanLook)
        {
            CamLook();
        }

    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= stat.moveSpeed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;  
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    void CamLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        camContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isHanging)
        {
            DropFromLedge();
        }
        else if (IsGround())
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    private void DropFromLedge()
    {
        transform.SetParent(null); 

        rb.useGravity = true;
        rb.velocity = Vector3.zero;

        Vector3 pushDir = transform.forward * -1f; 
        transform.position += pushDir * 0.2f;


        rb.AddForce(Vector3.up * 5f + pushDir * 2f, ForceMode.Impulse);

        isHanging = false;
    }

    bool IsGround()
    {
        float rayLength = 0.2f; // 더 긴 거리로 설정
        Vector3 originOffset = Vector3.down * 0.8f;// Collider 바깥으로 이동

        Ray[] rays = new Ray[4]
        {
        new Ray(transform.position + originOffset + (transform.forward * 0.1f), Vector3.down),
        new Ray(transform.position + originOffset + (-transform.forward * 0.1f), Vector3.down),
        new Ray(transform.position + originOffset + (transform.right * 0.1f), Vector3.down),
        new Ray(transform.position + originOffset + (-transform.right * 0.1f), Vector3.down),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            Debug.DrawRay(rays[i].origin, rays[i].direction * rayLength, Color.red, 1f);

            if (Physics.Raycast(rays[i], rayLength, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            inventroy?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        CanLook = !toggle;
    }


    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !isDashing)
        {
            StartCoroutine(DashForce());
        }
    }

    IEnumerator DashForce()
    {
        isDashing = true;

        rb.velocity = Vector3.zero;


        rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);

        yield return new WaitForSeconds(dashDuration);


        isDashing = false;
    }

    public void OnLedge(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !isHanging && LedgeDetector.ledgeAvailable)
        {
            Vector3 ledgePoint = LedgeDetector.ledgePoint;
            Vector3 wallForward = LedgeDetector.ledgeNormal;
            Transform wallTransform = LedgeDetector.ledgeTransform;


            isHanging = true;
            rb.velocity = Vector3.zero;
            rb.useGravity = false;

            Vector3 hangPos = ledgePoint - wallForward.normalized;
            transform.position = hangPos;

            transform.SetParent(wallTransform, true);
    
        }
    }

    public void OnCamera(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            isTrans = !isTrans;
            Transition(isTrans);

        }
    }

    private void Transition(bool isView)
    {
       if(isView)
       {
            cinemachineCam.gameObject.SetActive(false);
            mainCam.gameObject.transform.localPosition = mainCamOffset;
            mainCam.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
       else
       {
            cinemachineCam.Follow = CharacterManager.Inst.Player.transform;
            cinemachineCam.LookAt = CharacterManager.Inst.Player.transform;

            // 2. 플레이어와 FPS 카메라 기준으로 회전 계산
            Vector3 camForward =  mainCam.transform.transform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 playerForward = mainCam.transform.forward;
            playerForward.y = 0;
            playerForward.Normalize();

            float signedAngle = Vector3.SignedAngle(playerForward, camForward, Vector3.up);
            float normalized = Mathf.Repeat(signedAngle, 360f) / 360f;

            // 3. 시야 정렬
            cinemachineCam.m_XAxis.Value = normalized;
            cinemachineCam.m_YAxis.Value = 0.5f;
            cinemachineCam.gameObject.SetActive(true);
       }
    }

    public void Knockback(Vector3 hitDirection, float force)
    {
        hitDirection.Normalize();
        hitDirection.y = 0;
        rb.velocity = Vector3.zero; 
        rb.AddForce(hitDirection * force, ForceMode.Impulse);
    }


}

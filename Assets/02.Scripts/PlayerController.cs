using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;

    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform camContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    public float jumpPower;

    private Rigidbody rb;

    public bool CanLook = true;
    public Action inventroy;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        dir *= moveSpeed;
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
       if(context.phase == InputActionPhase.Started && IsGround())
       {
           rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
       }
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
}

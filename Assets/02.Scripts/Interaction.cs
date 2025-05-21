using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDis;
    public LayerMask layerMask;

    public GameObject currentInteractObj;
    private IInteractable currentInteractable;

    public TextMeshProUGUI promptText;
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            

            if (Physics.Raycast(ray, out hit, maxCheckDis, layerMask))
            {
                if (hit.collider.gameObject != currentInteractObj)
                {
                    currentInteractObj = hit.collider.gameObject;
                    currentInteractable = hit.collider.GetComponent<IInteractable>();
                    SetProemtText();
                }
            }
            else
            {
                currentInteractObj = null;
                currentInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }

    }

    private void SetProemtText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = currentInteractable.GetInteractPrompt();
       
    }

    public void OnInteractionInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed && currentInteractable != null)
        {
            currentInteractable.OnInteract();

            currentInteractObj = null;
            currentInteractable = null;
            promptText.gameObject.SetActive(false);  
        }
    }
}

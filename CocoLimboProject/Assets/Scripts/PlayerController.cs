using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float lookSensitivity = 2f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;
    
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    
    [Header("Interaction Settings")]
    [SerializeField] private float interactRange = 3f; // How far the player can interact
    private bool canInteract = true;
    private GameObject currentInteractable;

    private const float INTERACTION_COOLDOWN = 0.5f;

    [SerializeField] private float interactionCooldown = INTERACTION_COOLDOWN;
    

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private CharacterController characterController;
    private Vector3 verticalVelocity;
    private bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();
        
        // If no camera is assigned, try to find the main camera
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if grounded
        isGrounded = characterController.isGrounded;
        
        // Reset vertical velocity when grounded
        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f; // Small negative value to keep grounded
        }

        if(canInteract)
        {
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            RaycastHit hit;
            
            Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.yellow);

            if (Physics.Raycast(ray, out hit, interactRange))
            {
                if (hit.collider.CompareTag("Interactable"))
                {
                    currentInteractable = hit.collider.gameObject;
                }
                else
                {
                    currentInteractable = null;
                }
            }
            else
            {
                currentInteractable = null;
            }
        } else if (!canInteract)
        {
            interactionCooldown -= Time.deltaTime;
            if (interactionCooldown <= 0f)
            {
                canInteract = true;
                interactionCooldown = INTERACTION_COOLDOWN;
            }
        }

                

        // Handle movement
        Vector3 movement = transform.right * moveInput.x + transform.forward * moveInput.y;
        characterController.Move(movement * moveSpeed * Time.deltaTime);

        // Apply gravity
        verticalVelocity.y += gravity * Time.deltaTime;
        characterController.Move(verticalVelocity * Time.deltaTime);

        // Handle looking
        // Horizontal rotation (player body)
        transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);

        // Vertical rotation (camera only)
        xRotation -= lookInput.y * lookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    // Input System callback for movement
    public void OnMove(InputAction.CallbackContext value)
    {
        moveInput = value.ReadValue<Vector2>();
    }

    // Input System callback for looking
    public void OnLook(InputAction.CallbackContext value)
    {
        lookInput = value.ReadValue<Vector2>();
    }

    // Input System callback for interact
    public void OnInteract(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed && currentInteractable != null && canInteract)
        {
            Debug.Log("Interacting with: " + currentInteractable.name);
            canInteract = false;
            // Add interaction logic here
            currentInteractable.GetComponent<DiagHandler>().BeginDisplay();
        }
    }

    // Input System callback for jump
    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed && isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }
}

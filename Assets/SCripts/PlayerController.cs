using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float rotationSpeed = 10f;

    public MeshRenderer playerRenderer;
    public Color normalColor = Color.white;
    public Color attachedColor = Color.cyan;

    private CharacterController characterController;
    private bool isInAttachArea = false;
    private bool isAttached = false;
    
    private Material playerMat;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        playerMat = playerRenderer.material;
        playerMat.color = normalColor;
    }

    void Update()
    {
        if (!isAttached)
        {
            HandleMovement();
        }

        if (isInAttachArea && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ToggleAttachment();
        }
    }

    private void HandleMovement()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) vertical += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) vertical -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal += 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal -= 1f;
        }

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Find the angle we need to face based purely on the keys pressed (W = Up/North, D = Right/East)
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Smoothly rotate the character to face the movement direction
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move the character in the global direction pressed
            characterController.Move(direction * moveSpeed * Time.deltaTime);
        }

        // Apply constant simple gravity
        characterController.Move(Vector3.down * 9.81f * Time.deltaTime);
    }

    private void ToggleAttachment()
    {
        isAttached = !isAttached;
        
        playerMat.color = isAttached ? attachedColor : normalColor;
        
        if (isAttached)
            Debug.Log("Attached! Press Space again to detach.");
        else
            Debug.Log("Detached! Ready to move.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttachArea"))
        {
            isInAttachArea = true;
            Debug.Log("Entered Attach Box Region.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AttachArea"))
        {
            isInAttachArea = false;
            
            if (isAttached)
            {
                isAttached = false;
                playerMat.color = normalColor;
            }
            
            Debug.Log("Left Attach Box Region.");
        }
    }
}

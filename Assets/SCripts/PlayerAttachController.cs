using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerAttachController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float rotationSpeed = 10f;
    public float attachDuration = 7f;
    public float knockdownDuration = 2f;

    public MeshRenderer playerRenderer;
    public Color normalColor = Color.white;
    public Color attachingColor = Color.cyan;

  
    public bool isAttaching = false;
    public bool isInAttachArea = false;
    public bool attachComplete = false;
    public float attachProgress = 0f;

    private CharacterController characterController;
    private Material playerMat;

    private float attachTimer = 0f;
    private bool isKnockedDown = false;
    private float knockdownTimer = 0f;
    private Quaternion standingRotation;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerMat = playerRenderer.material;
        playerMat.color = normalColor;
    }

    void Update()
    {
        
        if (isKnockedDown)
        {
            knockdownTimer += Time.deltaTime;
            if (knockdownTimer >= knockdownDuration)
            {
                RecoverFromKnockdown();
            }
            return;
        }

        
        bool movementPressed = false;
        if (Keyboard.current != null)
        {
            movementPressed = Keyboard.current.wKey.isPressed ||
                              Keyboard.current.sKey.isPressed ||
                              Keyboard.current.aKey.isPressed ||
                              Keyboard.current.dKey.isPressed ||
                              Keyboard.current.upArrowKey.isPressed ||
                              Keyboard.current.downArrowKey.isPressed ||
                              Keyboard.current.leftArrowKey.isPressed ||
                              Keyboard.current.rightArrowKey.isPressed;
        }

        
        if (isAttaching && movementPressed)
        {
            CancelAttach();
        }

        // If in attach area and holding space and not moving, progress attach
        if (isInAttachArea && !isAttaching && !attachComplete &&
            Keyboard.current != null && Keyboard.current.spaceKey.isPressed && !movementPressed)
        {
            StartAttach();
        }

        if (isAttaching)
        {
            // Stop holding space = cancel
            if (Keyboard.current == null || !Keyboard.current.spaceKey.isPressed)
            {
                CancelAttach();
            }
            else
            {
                attachTimer += Time.deltaTime;
                attachProgress = Mathf.Clamp01(attachTimer / attachDuration);

                if (attachTimer >= attachDuration)
                {
                    CompleteAttach();
                }
            }
        }
        else if (!attachComplete)
        {
            HandleMovement();
        }

        // Apply gravity always
        characterController.Move(Vector3.down * 9.81f * Time.deltaTime);
    }

    private void StartAttach()
    {
        isAttaching = true;
        attachTimer = 0f;
        attachProgress = 0f;
        playerMat.color = attachingColor;
    }

    private void CancelAttach()
    {
        isAttaching = false;
        attachTimer = 0f;
        attachProgress = 0f;
        playerMat.color = normalColor;
    }

    private void CompleteAttach()
    {
        isAttaching = false;
        attachComplete = true;
        attachProgress = 1f;
        playerMat.color = Color.green;
        Debug.Log("Attach complete!");
    }

    public void TriggerKnockdown()
    {
        if (isKnockedDown) return;

        CancelAttach();
        isKnockedDown = true;
        knockdownTimer = 0f;
        standingRotation = transform.rotation;

        // Rotate 90 degrees on X axis (fall forward)
        transform.rotation = standingRotation * Quaternion.Euler(90f, 0f, 0f);
        playerMat.color = Color.red;
    }

    private void RecoverFromKnockdown()
    {
        isKnockedDown = false;
        knockdownTimer = 0f;
        transform.rotation = standingRotation;
        playerMat.color = normalColor;
        Debug.Log("Recovered from knockdown!");
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
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            characterController.Move(direction * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AttachArea")
        {
            isInAttachArea = true;
        }

        // Guard bot reached the player - knockdown
        if (other.gameObject.tag == "GuardBot" && isAttaching)
        {
            TriggerKnockdown();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "AttachArea")
        {
            isInAttachArea = false;
            if (isAttaching)
            {
                CancelAttach();
            }
        }
    }
}

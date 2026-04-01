using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform;

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

        if (isInAttachArea && Input.GetKeyDown(KeyCode.Space))
        {
            ToggleAttachment();
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            
            targetAngle += cameraTransform.eulerAngles.y;

            float smoothedAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
            characterController.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
        }

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

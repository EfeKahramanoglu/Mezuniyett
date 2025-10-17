using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 300f;
    public Transform cameraTransform;
    public GameObject weapon1;
    public GameObject weapon2;


    private Rigidbody rb;
    private float xRotation = 0f;
    private bool isGrounded = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

    // Rastgele doğma noktası seçip düşman spawnlar
    
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce);
            isGrounded = false;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 5f;
        }
        else
        {
            moveSpeed = 3f;
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon2.SetActive(true);
            weapon1.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon1.SetActive(true);
            weapon2.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            weapon1.SetActive(false);
            weapon2.SetActive(false);
        }
        
        
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 newPos = rb.position + move * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPos);
        
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }



    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
    }
}
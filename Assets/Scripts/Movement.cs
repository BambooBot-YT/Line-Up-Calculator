using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 4.0f;
    private float jumpHeight = 0.8f;
    private float gravityValue = -9.81f * 1.5f;
    private Transform head;

    private float yaw;
    private float pitch;

    public float mouseSensitivity = 1.0f;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public bool autoSprint = false;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        head = transform.GetChild(0);
        transform.GetChild(1).gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // move direction is relative to head rotation, and horizontal only
        move = head.TransformDirection(move);
        move.y = 0;

        //Debug.Log((Input.GetKeyDown(KeyCode.LeftShift) && groundedPlayer));
        controller.Move(playerSpeed * Time.deltaTime * move.normalized * (Input.GetKeyDown(KeyCode.LeftShift) && groundedPlayer ? 2 : 1));

        yaw += mouseSensitivity * Input.GetAxis("Mouse X");
        pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
        yaw = yaw % 360;
        pitch = Mathf.Clamp(pitch, -90, 90);

        head.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        // get scroll down 
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
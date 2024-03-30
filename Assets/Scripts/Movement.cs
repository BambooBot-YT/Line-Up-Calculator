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
    public KeyCode jumpKey = KeyCode.Space;
    public bool autoSprint = false;

    private void Start()
    {
        // Lock and hide cursor when the game begins
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controller = gameObject.GetComponent<CharacterController>();
        head = transform.GetChild(0);
        transform.GetChild(1).gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
    }

    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            Vector3 move = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            move = head.TransformDirection(move);
            move.y = 0;

            //Debug.Log((Input.GetKeyDown(KeyCode.LeftShift) && groundedPlayer));
            controller.Move(playerSpeed * Time.deltaTime * move.normalized * (Input.GetKeyDown(KeyCode.LeftShift) && groundedPlayer ? 2 : 1));


            yaw += mouseSensitivity * Input.GetAxis("Mouse X");
            pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
            yaw = yaw % 360;
            pitch = Mathf.Clamp(pitch, -90, 90);
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);

            head.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

            if (Input.GetKey(jumpKey) && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }
        }

        if (Input.GetKey(KeyCode.Escape) && Cursor.lockState == CursorLockMode.Locked) // unlock cursoe with escape
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetMouseButtonDown(0)) // lock cursor with left click
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
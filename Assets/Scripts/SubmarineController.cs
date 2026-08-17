using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class SubmarineController : MonoBehaviour
{
    [Header("Movement Bounds")]
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    [Header("Movement")]
    [SerializeField] private float moveSmoothTime = 0.12f;
    [SerializeField] private float maxMoveSpeed = 8f;

    private const string CONTROLLER_MODE_KEY = "ControllerMode";

    private Rigidbody2D rb;
    private Camera mainCamera;

    private Vector2 input;
    private Vector2 targetPosition;
    private Vector2 smoothVelocity;

    public float SpeedMultiplier { get; set; } = 1f;

    private bool CursorMode
    {
        get
        {
            return PlayerPrefs.GetInt(CONTROLLER_MODE_KEY, 0) == 1;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        targetPosition = rb.position;
        ApplyCursorState();
    }

    private void Update()
    {
        if (CursorMode)
            ReadMousePosition();
        else
            ReadKeyboardInput();

        ApplyCursorState();
    }

    private void FixedUpdate()
    {
        if (CursorMode)
            MoveTowardsMouse();
        else
            MoveWithKeyboard();
    }

    private void ReadKeyboardInput()
    {
        if (Keyboard.current == null)
        {
            input = Vector2.zero;
            return;
        }

        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
        {
            horizontal = -1f;
        }

        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            horizontal = 1f;
        }

        if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.upArrowKey.isPressed)
        {
            vertical = 1f;
        }

        if (Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed)
        {
            vertical = -1f;
        }

        input = new Vector2(horizontal, vertical).normalized;
    }

    private void ReadMousePosition()
    {
        if (Mouse.current == null || mainCamera == null)
            return;

        Vector2 mouseScreenPosition =
            Mouse.current.position.ReadValue();

        Vector3 mouseWorldPosition =
            mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        targetPosition = new Vector2(
            Mathf.Clamp(mouseWorldPosition.x, minX, maxX),
            Mathf.Clamp(mouseWorldPosition.y, minY, maxY)
        );
    }

    private void MoveWithKeyboard()
    {
        Vector2 targetVelocity =
            input * maxMoveSpeed * SpeedMultiplier;

        Vector2 newVelocity = Vector2.SmoothDamp(
            rb.linearVelocity,
            targetVelocity,
            ref smoothVelocity,
            moveSmoothTime
        );

        rb.linearVelocity = newVelocity;

        Vector2 newPosition =
            rb.position +
            newVelocity * Time.fixedDeltaTime;

        newPosition.x = Mathf.Clamp(
            newPosition.x,
            minX,
            maxX
        );

        newPosition.y = Mathf.Clamp(
            newPosition.y,
            minY,
            maxY
        );

        rb.MovePosition(newPosition);
    }

    private void MoveTowardsMouse()
    {
        Vector2 newPosition = Vector2.SmoothDamp(
            rb.position,
            targetPosition,
            ref smoothVelocity,
            moveSmoothTime,
            maxMoveSpeed * SpeedMultiplier
        );

        rb.MovePosition(newPosition);
    }

    private void ApplyCursorState()
    {
        if (CursorMode)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
            ApplyCursorState();
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}

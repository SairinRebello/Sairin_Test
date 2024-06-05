using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform playerBody;

    [SerializeField] private FixedJoystick _joystick;

    [SerializeField] private Transform playerBase;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpForce;

    [SerializeField] private float movementSpeed;

    private float rotationSmoother;
    private float rotationTime;
    private bool grounded;
    private bool jumped;
    void Start()
    {
        Time.timeScale = 1;
        GameManager.Instance.InitializeStart(1.25f);
    }
    private void Update()
    {
        Move();
        GroundCheck();
        UpdateAnimations();
    }

    private void Move()
    {
        if (!grounded) return;
        Vector3 direction = new Vector3(_joystick.Horizontal,0, _joystick.Vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {
            _animator.SetBool("readyToRun", true);
            _animator.SetBool("readyToJump", false);
            transform.Translate(direction * movementSpeed * Time.deltaTime);
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDamp(playerBody.eulerAngles.y, angle, ref rotationSmoother, rotationTime);
            playerBody.rotation = Quaternion.Euler(0,smoothAngle,0);
        }
        else
        {
            _animator.SetBool("readyToRun", false);
        }

    }

    private void GroundCheck()
    {
        grounded = Physics.CheckSphere(playerBase.position, 1f, groundMask);
    }

    private void UpdateAnimations()
    { 
        if(!grounded)
        {
            _animator.SetFloat("yVelocity", rb.velocity.y);
        }
        if(rb.velocity.y < .1f && rb.velocity.y > -.1f && jumped)
        {
            _animator.SetBool("readyToJump", false);
            jumped = false;
        }
    }
    public void Jump()
    {
        if(grounded && !jumped)
        {
            _animator.SetBool("readyToRun", false);
            _animator.SetBool("readyToJump", true);
            rb.velocity = Vector3.up * jumpForce;
            jumped = true;
        }
    }
}

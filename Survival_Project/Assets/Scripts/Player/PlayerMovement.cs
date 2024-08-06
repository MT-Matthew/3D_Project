using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance {get; set;}

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }



    public CharacterController controller;

    public float speed = 12f;
    public float runMultiplier = 2f;
    public float walkingGravity = -9.81f;
    public float gravity;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    Vector3 moveDirection = Vector3.zero;
    public Animator animator;
    public bool isGrounded;
    bool canMove = true;

    Vector3 lastPosition = new Vector3(0,0,0);
    public bool isMoving;

    // Swiming
    public bool isSwiming;
    public bool isUnderWater;
    public float swimingGravity = -0.5f;

    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if(InventorySystem.Instance.isOpen == false &&
            CraftingSystem.Instance.isOpen == false &&
            MenuManager.Instance.isMenuOpen == false &&
            StorageManager.Instance.storageUIOpen == false &&
            CampfireUIManager.Instance.isUiOpen == false) 
        {
            HandleSwiming();
            HandleMovement();
            HandleJumping();
            canMove = true;
        }
        else{
            canMove = false;
            SoundManager.Instance.grassWalkSound.Stop();
        }
        HandleGravity();
        HandleAnimator();

        // Debug.Log(velocity.y);
        
    }

    void HandleMovement()
    {


        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Lấy hướng nhìn của camera
        Vector3 move = Vector3.zero;
        if (Camera.main != null)
        {
            // Lấy hướng forward và right của camera (không cần y)
            Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;

            // Tính toán hướng di chuyển dựa trên hướng nhìn của camera
            move = camRight * x + camForward * z;

            if (Input.GetMouseButton(0)) // Kiểm tra nếu chuột trái đang được giữ
            {
                Vector3 targetDirection = camForward;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Điều chỉnh tốc độ xoay mặt
            }
            else if (move != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Điều chỉnh tốc độ xoay mặt
            }
        }

        float currentSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= runMultiplier;
        }

        moveDirection = move; // Lưu hướng di chuyển hiện tại

        controller.Move(move * currentSpeed * Time.deltaTime);



        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
            SoundManager.Instance.PlaySound(SoundManager.Instance.grassWalkSound);
        }
        else
        {
            isMoving = false;
            SoundManager.Instance.grassWalkSound.Stop();
        }

        lastPosition = gameObject.transform.position;

    }

    void HandleSwiming()
    {
        if (isSwiming)
        {
            if (isUnderWater)
            {
                gravity = swimingGravity;
                velocity.y = -2;
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    velocity.y = -5;
                }
                if (Input.GetKey(KeyCode.Space))
                {
                    velocity.y = 5;
                }
            }
            else
            {
                velocity.y = 0;
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    velocity.y = -5;
                }
            }
        }
        else
        {
            // velocity.y = -2;
            gravity = walkingGravity;
        }
    }

    void HandleJumping()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            animator.SetTrigger("Jump");
        }
    }

    void HandleGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleAnimator()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (canMove && (x != 0 || z != 0))
        {
            
            // if(z < 0 && GetComponent<ThrowBall>().isThrowing)
            // {
            //     animator.SetBool("isBackWard", true);
            // }
            // else{
            //     animator.SetBool("isBackWard", false);
            // }

            float angle = Vector3.Angle(transform.forward, moveDirection);

            if (angle > 50)
            {
                animator.SetBool("isBackWard", true);
            }
            else
            {
                animator.SetBool("isBackWard", false);
            }

            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (!isGrounded && velocity.y < 0)
        {
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }

        if (isSwiming)
        {
            if (canMove && (x != 0 || z != 0))
            {
                animator.SetBool("isSwimming", true);
                animator.SetBool("isTreading", false);
            }
            else
            {
                animator.SetBool("isTreading", true);
                animator.SetBool("isSwimming", false);
            }
        }
        else
        {
            animator.SetBool("isSwimming", false);
            animator.SetBool("isTreading", false);
        }
    }
}

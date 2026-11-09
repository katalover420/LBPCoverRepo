using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SackboyController : MonoBehaviour
{
    public bool interactionPressed;

    InputAction interactionAction;
    InputAction moveAction;
    InputAction jumpAction;
    CharacterController controller;
    public Animator animator;
    public CinemachineCamera cam;

    bool onGround;
    public float gravityValue;
    public float speedValue;
    public float minJumpHeight;
    public float maxJumpHeight;
    public float jumpTimeWindow;
    public float rotationSpeed;
    float jumpedTime; // same purpose as grounded time but for a jump buffer

   // public float jumpLength; // time of jump
  //  public float maxJumpLength; // max time of jump...
    
    public float yVelocity;

    Vector2 moveInput;
    float yMovement;
    float xMovement;
    Vector3 playerVelocity;
    float groundedTime;

    // AUDIO SFX

    public AudioSource walkingSFX;
    public AudioSource jumpingSFX;
    float pitchMinRange = 1.4f;
    float pitchMaxRange = 1.8f;
    public enum PlayerStates
    {
        moving,
        jumping,
        idle
    }
    public PlayerStates currentState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionAction = InputSystem.actions.FindAction("interaction");
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        controller = GetComponent<CharacterController>();
        jumpedTime = -1f;
        
    }
    
   
    // Update is called once per frame
    void Update()
    {
        //animator.SetBool("isgrounded", controller.isGrounded);

        // MOVING STATE


        // JUMPING STATE
        switch (currentState)
        {
            case PlayerStates.moving:
                MovementFunctions();
                break;
            case PlayerStates.jumping:
                JumpFunctions();
                break;
            case PlayerStates.idle:
                IdleFunctions();
                break;
        }


        Debug.Log(moveInput.ToString());
       moveInput = moveAction.ReadValue<Vector2>();
        xMovement = moveInput.x;
        yMovement = moveInput.y;
        //onGround = controller.isGrounded;
        
       Vector3 move = Vector3.ClampMagnitude(new Vector3(xMovement, 0f, yMovement), 1.0f)*speedValue; 
        //  moveAction.
        Camera mainCam = Camera.main;
        move = Quaternion.Euler(0f, mainCam.transform.eulerAngles.y, 0f) * move;


        // CHANGING MOVING AND IDLE STATE
        if (controller.isGrounded && moveInput != new Vector2(0, 0))
        {
            Debug.Log("currently moving");
            animator.SetBool("isgrounded", true);
            currentState = PlayerStates.moving;
            
        }
        if (controller.isGrounded && moveInput == new Vector2(0, 0))
        {
            Debug.Log("IDLE");
            animator.SetBool("isgrounded", true);

            currentState = PlayerStates.idle;
        }

        
        if (jumpAction.WasPressedThisFrame()) /// the float number at the end is coyote time in code!! and not having to do it with colliders
        {
            jumpedTime = Time.time; // saves the exact time.time position that u jumped at
            // speed + max press time 
        }

        // CHANGING DA JUMPING STATE
        if (Time.time - jumpedTime < jumpTimeWindow && Time.time - groundedTime < 0.2f)
        {
            currentState = PlayerStates.jumping;
            
            
            Debug.Log("JUMPED");
            walkingSFX.Stop();
            if (!jumpingSFX.isPlaying)
            {
                jumpingSFX.pitch = Random.Range(pitchMinRange, pitchMaxRange);
                jumpingSFX.Play();
            }
            // checks the elapsed time since you last jumped versus the jump buffer and the grounded window
            yVelocity = Mathf.Sqrt(maxJumpHeight * -2f * gravityValue);
            groundedTime = -1f; 
            jumpedTime = -1f;
        }

        // or just make the player start falling when jump is released
        if (jumpAction.WasReleasedThisFrame() && yVelocity > 0f)
        {
            Debug.Log("SHORT JUMPED");

            // ok so calculate to change to the minimum jump height 
            float minimumJumpVelocity = Mathf.Sqrt(minJumpHeight * -2f * gravityValue); 
            
            if (yVelocity > minimumJumpVelocity)
            {
                yVelocity = minimumJumpVelocity;
            }
        }
        // trying to get the player to fall when falling, but ummm not working and controller.isgrounded doesnt work much
        //if (yVelocity < -4f)
        //{

        //    animator.SetBool("isrunning", false);
        //    animator.SetBool("isgrounded", false);
        //    animator.SetBool("isfalling", true);


        //}
        /// theres probably a better way to do this but this works to transition to falling when running off a ledge 
        if (yVelocity <-4f)
        {
            animator.SetBool("isgrounded", false);
        }
        else if (controller.isGrounded)
        {
            animator.SetBool("isgrounded", true);
        }
        //else if (yVelocity >-4f && controller.isGrounded)
        //{
        //    //nimator.SetBool("isfalling", false);
        //    //nimator.SetBool("isfalling", false);

        //}
        if (controller.isGrounded)
        {
            groundedTime = Time.time;
            if (yVelocity < 0f) yVelocity = -2f;
   
            // making sure u cant spam jump
        }
        
        yVelocity += gravityValue * Time.deltaTime;
        move.y = yVelocity;
        controller.Move(move*Time.deltaTime);




        // INTERACTION PRESS
       
    }

    void MovementFunctions()
    {
        animator.SetBool("isrunning", true);

        if (!walkingSFX.isPlaying)
        {
            walkingSFX.pitch = Random.Range(pitchMinRange, pitchMaxRange);

            walkingSFX.Play();

        }

        // use slerps
        if (moveInput.x > 0f)
        {
            Quaternion targetRotation = Quaternion.Euler(0, -90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else if (moveInput.x < 0f)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else if (moveInput.y > 0f)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 180, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else if (moveInput.y < 0f) 
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }
        //sfx + animations n stuff. no need to put the actual movement handling in here? also ensures that stuff doesnt overlap since the states have to switch.
    }

    void JumpFunctions()
    {
        animator.SetBool("isgrounded", false);
        if (yVelocity < 0f)
        {
            animator.SetBool("isfalling", true);
        }



        // also rotate mid air

        if (moveInput.x > 0f)
        {
            Quaternion targetRotation = Quaternion.Euler(0, -90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else if (moveInput.x < 0f)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else if (moveInput.y > 0f)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 180, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else if (moveInput.y < 0f)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }

    }

    void IdleFunctions()
    {
        animator.SetBool("isrunning", false);

        walkingSFX.Stop();
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "cameraTrigger")
        {
            //Transform sackboyPos = transform;
            CameraScript camScript = other.GetComponentInParent<CameraScript>();
            cam = camScript.camera;          //  cam.PreviousStateIsValid = false;
           // cam.Follow = sackboyPos;
            cam.Priority = 10;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "cameraTrigger")
        {
            CameraScript camScript = other.GetComponentInParent<CameraScript>();
            cam = camScript.camera;
            //cam.transform.position = camScript.originalPos;
            //cam.Follow = null;
            cam.Priority = 0;
           
          //  cam = null;
        }
    }
}

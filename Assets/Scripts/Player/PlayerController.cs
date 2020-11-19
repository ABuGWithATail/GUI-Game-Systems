using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private float walkingSpeed = 7.5f;
    private float runningSpeed = 11.5f;
    private float crouchSpeed = 3.5f;
    private float jumpSpeed = 8.0f;
    private float gravity = 20.0f;    
    private float lookSpeed = 2.0f;
    private float lookXLimit = 45.0f;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    

    [Header("Attachments")]
    public PlayerStats playerStats;
    CharacterController characterController;
    public GameObject pauseObject;
    public GameObject hudObject;
    public GameObject deathObject;
    public GameObject keyBindObject;
    public Camera playerCamera;
    public GameObject spawn;
    AudioSource audioData;
    public KeyBind inputManager;

    [HideInInspector]
    public bool canMove = true;
    public bool isPaused = false;
    
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        audioData = GetComponent<AudioSource>();
        // this will lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //the API tells me that in locked cursor mode, the cursor is invisible regardless but i'll add it here for more clarity.
    }


    void Update()
    {
        // we are grounded so recalculate move direction based off axis
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = inputManager.GetButtonDown("Sprint");
        bool isCrouching = inputManager.GetButtonDown("Crouch");
        float curSpeedX = canMove ? (isRunning ? runningSpeed : (isCrouching ? crouchSpeed : walkingSpeed)) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : (isCrouching ? crouchSpeed : walkingSpeed)) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        //jumping
        if(inputManager.GetButtonDown("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
        //apply gravity.
        if(!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        if(isRunning == true)
        {
            playerStats.currentStamina -= 4 * Time.deltaTime;
        }
        if(isRunning == false)
        {
            playerStats.currentStamina += 2 * Time.deltaTime;
        }

        //move the controller
        characterController.Move(moveDirection * Time.deltaTime);
        OutOfStamina();

        // player and camera rotation
        if(canMove)
        {
            // looking movement with both a mouse and controller.
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX += -Input.GetAxis("Vertical2") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Horizontal2") * lookSpeed, 0);
        }

        //pausing part
        if (inputManager.GetButtonDown("Pause"))
        {
            Pausing();
        }
    }

    void OutOfStamina()
    {
        if(playerStats.currentStamina <= 0)
        {
            runningSpeed = 7.5f;
        }
        else
        {
            runningSpeed = 11.5f;
        }
    }
    public void Pausing()
    {
        isPaused = !isPaused;
        
        Debug.Log("The bool is now set to " + isPaused);

        if(isPaused == true)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            canMove = false;
            Cursor.visible = true;
            hudObject.SetActive(false);
            pauseObject.SetActive(true);
        }
        else if(isPaused == false)
        {
            Time.timeScale = 1;
            canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            hudObject.SetActive(true);
            pauseObject.SetActive(false);
            keyBindObject.SetActive(false);
        }

    }

    public bool IsDead()
    {
        if (playerStats.currentHealth <= 0)
        {
            audioData.Play();
            PlayerDeath();
            return true;
        }
        else
        {
            return false;
        }
    }
    public void PlayerDeath()
    {     
        Cursor.lockState = CursorLockMode.None;
        canMove = false;
        Cursor.visible = true;
        hudObject.SetActive(false);
        deathObject.SetActive(true);       
    }

    public void RemoveLossUI()
    {
        canMove = true;
        transform.position = spawn.transform.position;
        hudObject.SetActive(true);
        deathObject.SetActive(false);
        pauseObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

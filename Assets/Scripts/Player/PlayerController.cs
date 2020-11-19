using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    #region Variables
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
    #endregion

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
        // this here activates sprinting, crouching and directional movement.
        bool isRunning = Input.GetButton("Sprint");
        bool isCrouching = Input.GetButton("Crouch");
        float curSpeedX = canMove ? (isRunning ? runningSpeed : (isCrouching ? crouchSpeed : walkingSpeed)) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : (isCrouching ? crouchSpeed : walkingSpeed)) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        //jumping, character must be grounded.
        if (inputManager.GetButtonDown("Jump") && canMove && characterController.isGrounded)
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
        //these two functions take away and bring back stamina depending on if the character is running or not.
        if(isRunning == true)
        {
            playerStats.currentStamina -= 4 * Time.deltaTime;
        }
        if(isRunning == false)
        {
            playerStats.currentStamina += 2 * Time.deltaTime;
        }


        // player and camera rotation
        if (canMove)
        {
            // looking movement with both a mouse and controller.
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX += -Input.GetAxis("Vertical2") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Horizontal2") * lookSpeed, 0);
        }

        //move the controller
        characterController.Move(moveDirection * Time.deltaTime);
        OutOfStamina();

        //pausing part
        if (inputManager.GetButtonDown("Pause"))
        {
            Pausing();
        }
    }

    //this function affects sprint speed when the player runs out of stamnina.
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

    //activates the UI for the pause menu and stops time and activates the cursor. just changes a pause bool between true or not.
    public void Pausing()
    {
        isPaused = !isPaused;
        
        //Debug.Log("The bool is now set to " + isPaused);

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

    #region DeathStuff
    //these functions are the bool that activates player death, and the circumstances around that and respawning. they are called by functions in player stats for the most part.
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
    #endregion
}

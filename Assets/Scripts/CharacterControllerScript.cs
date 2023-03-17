using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterControllerScript : MonoBehaviour
{
    //movement
    private float horizontal;
    private float vertical;
    private Vector3 moveDirection;
    public float moveSpeed;
    public CharacterController controller;

    //look variables
    public float sensX;
    public float sensY;
    public Camera cam;
    float mouseX;
    float mouseY;
    float multiplier = .01f;
    float xRotation;
    float yRotation;

    //Animation
    public Animator anim;

    //raycast
    Ray RayOrigin;
    RaycastHit HitInfo;
    public bool pickUp;
    public bool holding;
    private bool insert;
    public Image crosshair;
    public GameObject itemPos;
    public GameObject lastObject;
    public GameObject objectHolding;
    public LayerMask mask;

    //keys
    public KeyCode pick = KeyCode.Mouse0;

    //animation
    public Animator axeAnim;
    public axeBreak axeBreak1;

    // Start is called before the first frame update
    void Start()
    {
        pickUp = false;
        holding = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        moveDirection = transform.forward * vertical + transform.right * horizontal;
        controller.SimpleMove(moveDirection.normalized * moveSpeed);

        //looking
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);



        //crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(controller.height == 2)
            {
                controller.height = 1;
                moveSpeed /= 2;
            }
            else
            {
                controller.height = 2;
                moveSpeed *= 2;
            }
            
        }

        if (objectHolding != null && objectHolding.tag == "axe" && Input.GetKeyDown(KeyCode.Mouse0) && axeAnim.GetBool("Swinging") == false)
        {
            axeAnim.SetBool("Swinging", true);
        }
        else
        {
            axeAnim.SetBool("Swinging", false);
        }
        

        //checks if player picks up object
        if(pickUp == true && Input.GetKey(pick) && holding == false)
        {
            holding = true;
            objectHolding = lastObject;
        }
        

        //sets holding to false if player is holding something and wants to drop it
        if(Input.GetKeyDown(pick) && holding == true)
        {
            holding = false;
            objectHolding = null;
        }
        
        
        //check if opening door
        if (insert == true && Input.GetKeyDown(KeyCode.Mouse0) && holding == true && objectHolding.tag == "key")
        {
            Debug.Log("Open Door");
        }

        //moves the object in hand that the player wants to hold
        if(holding == true)
        {
            lastObject.transform.position = itemPos.transform.position;
            lastObject.transform.rotation = cam.transform.rotation;
        }
        //Raycast
        RayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(RayOrigin, out HitInfo,2f, mask))
        {
            
            if((HitInfo.collider.gameObject.tag == "pickUp" || HitInfo.collider.gameObject.tag == "key" ||HitInfo.collider.gameObject.tag == "axe") && !holding)
            {
                
                pickUp = true;
                crosshair.color = Color.green;
                lastObject = HitInfo.collider.gameObject;
            }
            else if(HitInfo.collider.gameObject.tag == "door" && objectHolding.tag == "key")
            {
                crosshair.color = Color.green;
                insert = true;
            }
            else
            {
                pickUp = false;
                insert = false;
                crosshair.color = Color.white;
            }
        }
        else
        {
            pickUp = false;
            insert = false;
            crosshair.color = Color.white;
        }
        
    }
    
}

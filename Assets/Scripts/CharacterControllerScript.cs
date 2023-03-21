using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
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
    public GameObject door = null;

    //animation
    public Animator axeAnim;
    public Animator keyAnim;

    public bool stop = true;

    //axe
    public GameObject glassBroken;
    public bool glassCanBreak = false;
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

        
        if (objectHolding != null  && Input.GetKeyDown(KeyCode.Mouse0) && axeAnim.GetBool("Swinging") == false && objectHolding.tag == "axe")
        {
            axeAnim.SetBool("Swinging", true);
        }
        else
        {
            if(axeAnim.GetBool("Swinging") == true)
            {
                axeAnim.SetBool("Swinging", false);
            }
        }

        //checks if player picks up object
        if(pickUp == true && Input.GetKey(pick) && holding == false)
        {
            holding = true;
            objectHolding = lastObject;
            objectHolding.GetComponent<BoxCollider>().enabled = false;
        }
        

        //sets holding to false if player is holding something and wants to drop it
        if(Input.GetKeyDown(pick) && holding == true)
        {
            holding = false;
            objectHolding.GetComponent<BoxCollider>().enabled = true;
            objectHolding = null;
        }

        if (glassCanBreak == true && Input.GetKeyDown(pick) && holding == false)
        {
            Vector3 spawnPos = lastObject.transform.position;
            Destroy(lastObject);
            Instantiate(glassBroken, spawnPos, Quaternion.identity);
        }
        
        
        //check if opening door
        if (insert == true && Input.GetKeyDown(KeyCode.Mouse0) && holding == true && objectHolding != null && objectHolding.tag == "key")
        {
            
            Debug.Log("Open Door");
        }

        //moves the object in hand that the player wants to hold
        if(holding == true)
        {
            
            lastObject.transform.position = itemPos.transform.position;
            lastObject.transform.rotation = itemPos.transform.rotation;
            if (objectHolding.tag == "key")
            {
                if(objectHolding.name == "Red")
                {
                    objectHolding.GetComponent<ParentConstraint>().enabled = false;
                }
                keyAnim.SetBool("holdingKey", true);

            }
            else
            {
                keyAnim.SetBool("holdingKey", false);
            }
            
            
        }
        
        //Raycast
        RayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(RayOrigin, out HitInfo,2f, mask))
        {
            stop = true;
            //Debug.Log(HitInfo.collider.gameObject.name);
            if ((HitInfo.collider.gameObject.tag == "pickUp" || HitInfo.collider.gameObject.tag == "key" || HitInfo.collider.gameObject.tag == "axe") && holding == false)
            {

                pickUp = true;
                crosshair.color = Color.green;
                lastObject = HitInfo.collider.gameObject;
            }
            else if (HitInfo.collider.gameObject.tag == "door" && objectHolding != null && objectHolding.tag == "key")
            {

                door = HitInfo.collider.gameObject;
                if (door.GetComponent<door>().GetKey() == objectHolding)
                {
                    crosshair.color = Color.green;
                    insert = true;
                }
            }
            else if (HitInfo.collider.gameObject.tag == "glass" && holding == false) 
            {
                crosshair.color = Color.green;
                glassCanBreak = true;
                lastObject = HitInfo.collider.gameObject;
            }
            else
            {
                pickUp = false;
                insert = false;
                glassCanBreak = false;
                crosshair.color = Color.white;
            }
            if(objectHolding != null && objectHolding.tag == "axe" && HitInfo.distance <= 1)
            {
                axeAnim.SetBool("tooClose", true);
            }
            else
            {
                if(axeAnim.GetBool("tooClose") == true)
                {
                    axeAnim.SetBool("tooClose", false);
                }
            }
            
        }
        else
        {
            if(stop == true)
            {
                
                pickUp = false;
                insert = false;
                glassCanBreak = false;
                crosshair.color = Color.white;
                axeAnim.SetBool("tooClose", false);
                door = null;
                stop = false;
            }
            
        }
        
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
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
    public bool wrongKey = false;


    //keys
    public KeyCode pick = KeyCode.Mouse0;
    public GameObject door = null;

    //animation
    public Animator axeAnim;
    public Animator keyAnim;
    public Animator worldAnim;
    public Animator endAnim;
    public GameObject endCam;
    

    public bool stop = true;

    //axe
    public GameObject glassBroken;
    public bool glassCanBreak = false;

    public GameObject key;

    public GameObject note;

    public bool lookingAtPad = false;
    public keypad keyPad;

    public bool isPaused = false;
    public bool isDead = false;
    public GameObject pauseMenu;
    public GameObject deathMenu;

    public bool win;

    public GameObject spawnPos;
    public GameObject Tutorial;

    public GameManager gm;
    public AudioSource select;

    public AudioSource music;
    // Start is called before the first frame update
    void Start()
    {
        pickUp = false;
        holding = false;

        cursorDisable();

        note.SetActive(false);

        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);

        wrongKey = false;
    }


    // Update is called once per frame
    void Update()
    {
        if(win == true)
        {
            cursorEnable();
            
            crosshair.enabled = false;
            cam.GetComponent<Camera>().enabled = false;
            endCam.GetComponent<Camera>().enabled = true;
            worldAnim.SetBool("winning", true);
            endAnim.SetBool("winning", true);
        }

        if(isPaused != true && isDead != true)
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
        }
        


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                resume();
            }
            else
            {
                Pause();
            }
        }
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
            if(objectHolding.name == "Teal")
            {
                worldAnim.SetBool("rugDown", true);
            }
            if(objectHolding.tag == "key")
            {
                gm.keyPlay();
            }
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
            gm.glassPlay();
            Destroy(lastObject);
            Instantiate(glassBroken, spawnPos, Quaternion.identity);
        }
        
        
        //check if opening door
        if (insert == true && Input.GetKeyDown(KeyCode.Mouse0) && holding == true && objectHolding != null && objectHolding.tag == "key")
        {

            key = objectHolding;
            holding = false;
            objectHolding.GetComponent<BoxCollider>().enabled = true;
            objectHolding = null;
            if (key.name != "Key1")
            {
                if (key.name == "Teal")
                {
                    worldAnim.SetBool("door1Open", true);
                }
                if (key.name == "Red")
                {
                    worldAnim.SetBool("door2Open", true);
                }
                if (key.name == "Gold")
                {
                    worldAnim.SetBool("safeOpen", true);
                }
            }
            else
            {
                music.Play();
                this.gameObject.GetComponent<CharacterController>().enabled = false;
                this.gameObject.transform.position = spawnPos.transform.position;
                this.gameObject.transform.rotation = spawnPos.transform.rotation;
                this.gameObject.GetComponent<CharacterController>().enabled = true;
                gm.playing = true;
                EndTutorial();
            }
            
            
            Debug.Log("Open Door");
        }
        if(wrongKey == true && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("wrongKey");
            
            gm.wrongKeyPlay();
        }

        //moves the object in hand that the player wants to hold
        if(holding == true)
        {
            objectHolding.transform.position = itemPos.transform.position;
            objectHolding.transform.rotation = itemPos.transform.rotation;


            if (objectHolding.tag == "key")
            {
                if (objectHolding.name == "Red")
                {
                    objectHolding.GetComponent<Rigidbody>().useGravity = true;
                }

                keyAnim.SetBool("holdingKey", true);

            }
            else
            {
                keyAnim.SetBool("holdingKey", false);
            }
        }
        else
        {
            keyAnim.SetBool("holdingKey", false);
        }
        if(lastObject != null && crosshair.color == Color.green && lastObject.tag == "note" && Input.GetKeyDown(pick))
        {
            Destroy(lastObject);
            gm.paperPlay();
            note.SetActive(true);
        }
        if (lookingAtPad == true && Input.GetKeyDown(KeyCode.Mouse0) && holding != true) 
        {
            gm.keyPressPlay();
            if (lastObject.name == "enter")
            {
                keyPad.enter();
            }
            else if (lastObject.name == "clear")
            {
                keyPad.clearInput();
            }
            else 
            {
                keyPad.inputKey(lastObject.name);
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
            else if(HitInfo.collider.gameObject.tag == "door" && objectHolding == null)
            {
                wrongKey = true;
            }
            else if (HitInfo.collider.gameObject.tag == "glass" && holding == false) 
            {
                crosshair.color = Color.green;
                glassCanBreak = true;
                lastObject = HitInfo.collider.gameObject;
            }
            else if (HitInfo.collider.gameObject.tag == "note" && holding == false)
            {
                crosshair.color = Color.green;
                
                lastObject = HitInfo.collider.gameObject;
            }
            else if (HitInfo.collider.gameObject.tag == "pad" && holding == false)
            {
                crosshair.color = Color.green;
                lookingAtPad = true;
                lastObject = HitInfo.collider.gameObject;
            }
            else
            {
                pickUp = false;
                insert = false;
                glassCanBreak = false;
                crosshair.color = Color.white;
                wrongKey = false;
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
                wrongKey = false;
                insert = false;
                glassCanBreak = false;
                crosshair.color = Color.white;
                axeAnim.SetBool("tooClose", false);
                door = null;
                stop = false;
            }
            
        }

        
    }
    public void cursorDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void cursorEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Pause()
    {
        select.Play();
        pauseMenu.SetActive(true);
        isPaused = true;
        cursorEnable();
        Time.timeScale = 0f;
    }
    public void resume()
    {
        select.Play();
        pauseMenu.SetActive(false);
        isPaused = false;
        cursorDisable();
        Time.timeScale = 1f;
    }
    public void loadScene(string name) {
        select.Play();
        SceneManager.LoadScene(name);
    }
    public void Dead()
    {
        isDead = true;
        deathMenu.SetActive(true);
        cursorEnable();
    }
    public void EndTutorial()
    {
        
        StartCoroutine(destroyTutorial());
    }
    IEnumerator destroyTutorial()
    {
        yield return new WaitForSeconds(2);
        Destroy(Tutorial);
    }
}

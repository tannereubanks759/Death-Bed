using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class CharacterControllerScriptOptimized : MonoBehaviour
{
    public GameManager gm;
    public AudioSource music;

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


    public GameObject spawnPos;

    //raycast
    Ray RayOrigin;
    RaycastHit HitInfo;
    
    
    public Image crosshair;
    public GameObject itemPos;
    public GameObject objectHolding;
    public LayerMask mask;
    
    public AudioSource select;
    public bool isPaused;
    public GameObject pauseMenu;
    public GameObject Tutorial;
    public bool isDead;
    public GameObject deathMenu;

    public door door;
    public Animator worldAnim;
    
    public GameObject keyPos;
    public ConstraintSource source;
    public GameObject brokenGlass;
    public GameObject note;

    private bool holdingAxe;
    public keypad pad;
    // Start is called before the first frame update
    void Start()
    {
        cursorDisable();
    }


    // Update is called once per frame
    void Update()
    {
        if(isPaused == false)
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
        //crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (controller.height == 2)
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
        if (Input.GetKeyDown(KeyCode.Escape) && isDead == false) 
        {
            if(isPaused == true)
            {
                resume();
            }
            else
            {
                Pause();
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && objectHolding != null)
        {
            if(objectHolding.tag == "axe")
            {
                objectHolding.GetComponent<Animator>().SetBool("Swinging", true);
            }
            
        }
        //Raycast when press E
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (objectHolding != null)
            {
                objectHolding.GetComponent<ParentConstraint>().enabled = false;
                objectHolding.GetComponent<ParentConstraint>().constraintActive = false;
                objectHolding = null;
                holdingAxe = false;
            }
            else
            {
                RayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                if (Physics.Raycast(RayOrigin, out HitInfo, 2f, mask))
                {

                    if (HitInfo.collider.gameObject.tag == "key" || HitInfo.collider.gameObject.tag == "axe")
                    {
                        if(HitInfo.collider.gameObject.tag == "key")
                        {
                            gm.keyPlay();
                            
                            if(HitInfo.collider.gameObject.name == "Teal")
                            {
                                worldAnim.SetBool("rugDown",true);
                            }
                        }
                        else if(HitInfo.collider.gameObject.tag == "axe")
                        {
                            holdingAxe = true;
                        }
                        
                        objectHolding = HitInfo.collider.gameObject;
                        objectHolding.GetComponent<Rigidbody>().useGravity = true;
                        objectHolding.GetComponent<ParentConstraint>().enabled = true;
                        objectHolding.GetComponent<ParentConstraint>().constraintActive = true;
                        objectHolding.GetComponent<ParentConstraint>().enabled = true;
                        objectHolding.GetComponent<ParentConstraint>().locked = true;
                        objectHolding.GetComponent<ParentConstraint>().SetTranslationOffset(0, Vector3.zero);
                        Debug.Log("pickup");
                    }
                    if(HitInfo.collider.gameObject.tag == "note")
                    {
                        Destroy(HitInfo.collider.gameObject);
                        gm.paperPlay();
                        note.SetActive(true);
                    }

                }
            }
            
        }
        //Raycast when pressing mouse 0
        else if (Input.GetKeyDown(KeyCode.Mouse0) && holdingAxe == false)
        {
            RayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(RayOrigin, out HitInfo, 2f, mask))
            {
                if(HitInfo.collider.gameObject.tag == "door")
                {
                    door = HitInfo.collider.gameObject.GetComponent<door>();
                    if(door.GetKey() == objectHolding)
                    {
                        objectHolding.GetComponent<ParentConstraint>().constraintActive = false;
                        objectHolding.GetComponent<ParentConstraint>().enabled = false;
                        objectHolding.GetComponent<ParentConstraint>().locked = true;
                        string name = objectHolding.name;
                        if (name == "Key1")
                        {
                            music.Play();
                            gm.playing = true;
                            EndTutorial();
                            this.gameObject.GetComponent<CharacterController>().enabled = false;
                            this.gameObject.transform.position = spawnPos.transform.position;
                            this.gameObject.GetComponent<CharacterController>().enabled = true;
                        }
                        else if (name == "Teal")
                        {
                            Destroy(objectHolding);
                            worldAnim.SetBool("door1Open", true);
                        }
                        else if (name == "Red")
                        {
                            Destroy(objectHolding);
                            worldAnim.SetBool("door2Open", true);
                        }
                        else if (name == "Gold")
                        {
                            Destroy(objectHolding);
                            worldAnim.SetBool("safeOpen", true);
                        }
                        objectHolding = null;
                    }
                    else
                    {
                        if(objectHolding == null)
                        {
                            gm.wrongKeyPlay();
                        }
                        
                    }
                }
                else if(HitInfo.collider.gameObject.tag == "glass" && objectHolding == null)
                {
                    Vector3 spawnPos = HitInfo.collider.gameObject.transform.position;
                    gm.glassPlay();

                    Destroy(HitInfo.collider.gameObject);
                    Instantiate(brokenGlass, spawnPos, Quaternion.identity);
                    
                }
                else if(HitInfo.collider.gameObject.tag == "pad")
                {
                    gm.keyPressPlay();
                    if (HitInfo.collider.gameObject.name == "enter")
                    {
                        pad.enter();
                    }
                    else if (HitInfo.collider.gameObject.name == "clear")
                    {
                        pad.clearInput();
                    }
                    else
                    {
                        pad.inputKey(HitInfo.collider.gameObject.name);
                    }
                }
                
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
        Time.timeScale = 0f;
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
    public void loadScene(string name)
    {
        select.Play();
        SceneManager.LoadScene(name);
    }
    public void Dead()
    {
        Time.timeScale = 0f;
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

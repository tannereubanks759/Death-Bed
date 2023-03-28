using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public bool win;

    public TMP_Text timer;
    public float minutes = 5;

    public bool playing;

    public float nextTime;

    public CharacterControllerScriptOptimized player;

    public bool one = false;

    public GameObject EndCam;
    public Animator WorldAnim;
    //sounds
    public AudioSource source;
    public AudioClip wrongKeyClip;
    public AudioClip openDoorClip;
    public AudioClip keyClip;
    public AudioClip safeClip;
    public AudioClip glassClip;
    public AudioClip paper;
    public AudioClip keyPressClip;
    public AudioClip cageClip;
    public AudioClip eleopen;
    public AudioClip elemove;
    public AudioClip eleding;
    // Start is called before the first frame update
    void Start()
    {
        minutes = 4;
        nextTime = 0;
        timer.text = minutes.ToString() + " minutes"; 
        playing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playing == true && one == false)
        {
            nextTime = Time.time + 60;
            one = true;
        }
        if (playing == true)
        {
            if (win != true)
            {
                if(timer.enabled == false)
                {
                    timer.enabled = true;
                }
                
                
                if (Time.time > nextTime && win != true)
                {
                    minutes -= 1;
                    nextTime = Time.time + 60;
                    timer.text = minutes.ToString() + " minutes";

                }
                if (minutes <= 0)
                {
                    player.Dead();
                }
            }
            else
            {
                if(timer.enabled == true)
                {
                    timer.enabled = false;
                }
                

            }
        }
        
        
    }

    public void wrongKeyPlay()
    {
        source.clip = wrongKeyClip;
        source.Play();
    }
    public void openDoorPlay()
    {
        source.clip = openDoorClip;
        source.Play();
    }
    public void keyPlay()
    {
        source.clip = keyClip;
        source.Play();
    }
    public void safePlay()
    {
        source.clip = safeClip;
        source.Play();
    }
    public void glassPlay()
    {
        source.PlayOneShot(glassClip, 1f);
    }
    public void paperPlay()
    {
        source.PlayOneShot(paper, 1f);
    }
    public void keyPressPlay()
    {
        source.PlayOneShot(keyPressClip, 1f);
    }
    public void cagePlay()
    {
        source.PlayOneShot(cageClip, 1f);
    }
    public void elevatorOpen()
    {
        source.PlayOneShot(eleopen, 1f);
    }
    public void elevatorMove()
    {
        source.PlayOneShot(elemove, 1f);
    }
    public void elevatorDing()
    {
        source.PlayOneShot(eleding, 1f);
    }
    public void winAnim()
    {
        EndCam.SetActive(true);
        player.cursorEnable();
        Destroy(player.gameObject);
        EndCam.GetComponent<Animator>().SetBool("winning", true);
        WorldAnim.SetBool("winning", true);

    }
}

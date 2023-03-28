using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class axeBreak : MonoBehaviour
{
    public GameObject boxPref;

    public AudioSource axeAudio;

    public bool swing;

    public GameManager gm;

    
    // Start is called before the first frame update
    
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "box" && swing == true)
        {
            box box = collision.gameObject.GetComponent<box>();
            if(box.brokenBox != null)
            {
                if(collision.gameObject.name == "Z_Keypad Cage")
                {
                    gm.cagePlay();
                    
                }
                else
                {
                    axeAudio.Play();
                }
                
                
                boxPref = box.brokenBox;
                Vector3 position1 = collision.gameObject.transform.position;
                Destroy(collision.gameObject);
                Instantiate(boxPref, position1, Quaternion.identity);
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }
    
}

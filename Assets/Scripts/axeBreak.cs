using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class axeBreak : MonoBehaviour
{
    public GameObject boxPref;
    public Animator axe;
    public bool swing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "box" && swing == true)
        {
            Destroy(collision.gameObject);
        }
    }
    public void swingFalse()
    {
        swing = false;
    }
    public void swingTrue()
    {
        swing = true;
    }
}

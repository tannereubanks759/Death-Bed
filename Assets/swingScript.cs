using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swingScript : MonoBehaviour
{
    public void swingingFalse()
    {
        this.gameObject.GetComponent<Animator>().SetBool("Swinging", false);
    }
}

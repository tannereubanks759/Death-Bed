using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class axeAnim : MonoBehaviour
{
    public axeBreak axeBreak1;
    public Animator axe;
    public bool swing;
    // Start is called before the first frame update
    
    
    public void swingFalse()
    {
        swing = false;
        axeBreak1.swing = false;
    }
    public void swingTrue()
    {
        swing = true;
        axeBreak1.swing = true;
    }
}

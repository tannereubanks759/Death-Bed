using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class world : MonoBehaviour
{
    public GameManager gm;
    public AudioSource source;
    public AudioClip open;
    public AudioClip move;
    // Start is called before the first frame update
    
    public void openDoor()
    {
        gm.openDoorPlay();
    }
    public void safeDoor()
    {
        gm.safePlay();
    }
    public void elevatorOpen()
    {
        gm.elevatorOpen();
    }
    public void elevatorMove()
    {
        gm.elevatorMove();
    }
    public void elevatorDing()
    {
        gm.elevatorDing();
    }
}

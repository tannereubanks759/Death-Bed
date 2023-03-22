using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public TMP_Text timer;
    public float minutes = 5;

    public float nextTime;

    public CharacterControllerScript player;
    // Start is called before the first frame update
    void Start()
    {
        minutes = 5;
        nextTime = Time.time + 60;
        timer.text = minutes.ToString() + " minutes";
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextTime)
        {
            minutes -= 1;
            nextTime = Time.time + 60;
            timer.text = minutes.ToString() + " minutes";
        }
        if(minutes <= 0)
        {
            player.Dead();
        }
    }
}

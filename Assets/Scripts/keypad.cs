using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class keypad : MonoBehaviour
{
    public string key = "3214";
    public string input = "";
    public TMP_Text text;
    public CharacterControllerScript player;
    // Start is called before the first frame update
    void Start()
    {
        text.faceColor = new Color(0, 0, 1);
    }

    public void inputKey(string num)
    {
        if (input.Length > 3)
        {
            enter();
        }
        else
        {
            input += num;
        }
        
        text.text = input;
    }
    public void clearInput()
    {
        text.faceColor = new Color(0, 0, 1);
        input = "";
        text.text = "";
    }
    public void enter()
    {
        if(input == key)
        {
            text.faceColor = new Color(0, 1, 0);
            text.text = input;
            Debug.Log("correct");
            player.win = true;
        }
        else
        {
            Debug.Log("incorrect");
            clearInput();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI : MonoBehaviour
{
    public AudioSource select;
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadLevel(string name)
    {
        select.Play();
        StartCoroutine(load(name));
    }
    public void Quit()
    {
        select.Play();
        Application.Quit();
    }
    IEnumerator load(string name)
    {
        yield return new WaitForSeconds(.1f);
        SceneManager.LoadScene(name);
    }
}

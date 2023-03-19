using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{
    public GameObject brokenBox;
    public float nextFire;
    private void Start()
    {
        nextFire = Time.time + 10;
    }
    private void Update()
    {
        if(this.gameObject.tag != "box")
        {
            if (Time.time > nextFire)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

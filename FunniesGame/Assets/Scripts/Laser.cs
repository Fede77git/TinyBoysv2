using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer line;
    public Transform startPoint;
    public int timer;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        Invoke("InvokeObject", timer);
      
    }
    void Update()
    {
       
        //line.SetPosition(0, startPoint.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit))
        {
            if (hit.collider)
            {
                line.SetPosition(1, hit.point);
            }
            if (hit.transform.tag == "Player")
            {
                Destroy(hit.transform.gameObject);
            }
        }
        else
        {
            line.SetPosition(1, transform.right * 5000);
        }
    }

    void InvokeObject()
    {
        line.SetPosition(0, startPoint.position);
    }
}

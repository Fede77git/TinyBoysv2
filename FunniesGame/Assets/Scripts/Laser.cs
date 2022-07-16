using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Laser : MonoBehaviour
{
    private LineRenderer line;
    public Transform startPoint;
    public int timer;
    public Text textWin;
    public Text textEsc;
    public Text txtOrange;
    public Text txtCut;
    private void Start()
    {
        line = GetComponent<LineRenderer>();
        Invoke("InvokeObject", timer);
        textWin.text = "";
        textEsc.text = "";
    }
    void Update()
    {

        line.SetPosition(0, startPoint.position);
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
            else if (hit.transform.tag == "Head1")
            {
                Destroy(hit.transform.gameObject);
                txtOrange.text = "Orange Player Wins";
                txtCut.text = "Purple player was cut into pieces";
                textEsc.text = "Press Escape to continue";
                

                Time.timeScale = 0;
            }
            else if (hit.transform.tag == "Head2")
            {
                Destroy(hit.transform.gameObject);
                textWin.text = "Purple Player Wins";
                txtCut.text = "Orange player was cut into pieces";

                textEsc.text = "Press Escape to continue";

                Time.timeScale = 0;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Laser2 : MonoBehaviour
{
    private LineRenderer line;
    public Transform startPoint;
    public int timer;
    public Text textWin;
    public Text textEsc;

    public Text txtCut;

    public bool dead1;
    public bool dead2;
    public bool dead3;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        Invoke("InvokeObject", timer);

        dead1 = false;
        dead2 = false;
        dead3 = false;

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
                dead2 = true;
                 //Destroy(hit.transform.gameObject);
                //txtOrange.text = "Orange Player Wins";
                //txtCut.text = "Purple player was cut into pieces";
                //textEsc.text = "Press Escape to continue";


                //Time.timeScale = 0;
            }
            else if (hit.transform.tag == "Head2")
            {
                dead1 = true;
                


            }
            else if (hit.transform.tag == "Head3")
            {
                dead3 = true;
                


            }
        }
        else
        {
            line.SetPosition(1, transform.right * 5000);
        }


        if (dead1 == true && dead2 == true && dead3 == false)
        {
            textWin.text = "Green Player Wins";
            textEsc.text = "Press Escape to continue";
            txtCut.text = "Purple and Orange player was cut into pieces";
            Time.timeScale = 0;
        }
        else if (dead1 == true && dead2 == false && dead3 == true)
        {
            textWin.text = "Purple Player Wins";
            textEsc.text = "Press Escape to continue";
            txtCut.text = "Green and Orange player was cut into pieces";
            Time.timeScale = 0;
        }
        else if (dead1 == false && dead2 == true && dead3 == true)
        {
            textWin.text = "Orange Player Wins";
            textEsc.text = "Press Escape to continue";
            txtCut.text = "Purple and Green player was cut into pieces";
            Time.timeScale = 0;
        }
    }

    void InvokeObject()
    {
        line.SetPosition(0, startPoint.position);

    }
}

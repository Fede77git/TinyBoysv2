using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class gamemanager3 : MonoBehaviour
{
   

    public Text textWin;
    public Text textEsc;
    
    public Laser laser;
    public Text txtCut;
    void Start()
    {

    }

    private void Update()
    {
        if (laser.dead1 == true && laser.dead2 == true && laser.dead3 == false)
        {
            textWin.text = "Green Player Wins";
            textEsc.text = "Press Escape to continue";
            txtCut.text = "Purple and Orange player was cut into pieces";
            Time.timeScale = 0;
        }
        else if (laser.dead1 == true && laser.dead2 == false && laser.dead3 == true)
        {
            textWin.text = "Purple Player Wins";
            textEsc.text = "Press Escape to continue";
            txtCut.text = "Green and Orange player was cut into pieces";
            Time.timeScale = 0;
        }
        else if (laser.dead1 == false && laser.dead2 == true && laser.dead3 == true)
        {
            textWin.text = "Orange Player Wins";
            textEsc.text = "Press Escape to continue";
            txtCut.text = "Purple and Green player was cut into pieces";
            Time.timeScale = 0;
        }
    }
}

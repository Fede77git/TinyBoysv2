using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Gamemanager2 : MonoBehaviour
{
    public PlayerController playerController;
    public Controller2Pj controller2Pj;
    public Controller3 controller3;
    public Controller4 controller4;

    public Text textWin;
    public Text textEsc;
    public Text textFell;
    void Start()
    {
        
    }

    
    void Update()
    {


        if (playerController.dead1 == true && controller2Pj.dead2 == true && controller3.dead3 == false && controller4.dead4 == true)
        {
            textWin.text = "Green Player Wins";
            textEsc.text = "Press Escape to continue";
            textFell.text = "Purple and Orange player fell of the map";
            Time.timeScale = 0;
        }
        else if (playerController.dead1 == true && controller2Pj.dead2 == false && controller3.dead3 == true && controller4.dead4 == true)
        {
            textWin.text = "Purple Player Wins";
            textEsc.text = "Press Escape to continue";
            textFell.text = "Green and Orange player fell of the map";
            Time.timeScale = 0;
        }
        else if (playerController.dead1 == false && controller2Pj.dead2 == true && controller3.dead3 == true && controller4.dead4 == true)
        {
            textWin.text = "Orange Player Wins";
            textEsc.text = "Press Escape to continue";
            textFell.text = "Purple and Green player fell of the map";
            Time.timeScale = 0;
        }
        else if (playerController.dead1 == true && controller2Pj.dead2 == true && controller3.dead3 == true && controller4.dead4 == false)
        {
            textWin.text = "Blue Player Wins";
            textEsc.text = "Press Escape to continue";
            textFell.text = "Purple and Green player fell of the map";
            Time.timeScale = 0;
        }



    }
}

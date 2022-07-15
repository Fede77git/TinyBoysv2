
using UnityEngine;
using UnityEngine.UI;

public class WinLvl1Player2 : MonoBehaviour
{
    private int cont;
    public Text textCollected;
    public Text textWin;
    public Text textEsc;
    void Start()
    {
        cont = 0;
        textWin.text = "";
        textEsc.text = "";
        SetText();
    }

    private void SetText()
    {
        textCollected.text = " " + cont.ToString();
        if (cont >= 5)
        {

            textWin.text = "Orange Player Wins";
            textEsc.text = "Press Escape to continue";
            Time.timeScale = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("coleccionable"))
        {
            cont = cont + 1;
            SetText();
            other.gameObject.SetActive(false);
        }
    }


}

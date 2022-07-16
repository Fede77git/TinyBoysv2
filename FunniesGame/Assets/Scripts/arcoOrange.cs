
using UnityEngine;
using UnityEngine.UI;

public class arcoOrange : MonoBehaviour
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

            textWin.text = "Purple Player Wins";
            textEsc.text = "Press Escape to continue";

            Time.timeScale = 0f;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("egg"))
        {
            cont = cont + 1;
            SetText();
            other.gameObject.SetActive(false);
        }
    }
}

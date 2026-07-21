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
            
            PlayerController pc = hit.collider.GetComponentInParent<PlayerController>();
            if (pc != null && !pc.isDead)
            {
                pc.Dead();
                pc.gameObject.SetActive(false);
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

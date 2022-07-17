using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUplvl4Pl2 : MonoBehaviour
{
    public Controller2Pj controller2Pj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerUp2") == true)
        {
            
           controller2Pj.speed += 5f;
            other.gameObject.SetActive(false);
        }
    }
}

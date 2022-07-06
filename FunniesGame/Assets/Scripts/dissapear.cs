using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dissapear : MonoBehaviour
{
   
    void Update()
    {
        Destroy(gameObject, 5);
    }
}

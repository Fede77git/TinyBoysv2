
using UnityEngine;

public class PlatformGhost : MonoBehaviour
{
    bool isFalling = false;
    float speedD = 0f;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")  
        {
            isFalling = true;
            Destroy(gameObject, 10);
        }
    }
    

    private void Update()
    {
        if (isFalling)
        {
            speedD += Time.deltaTime / 75;
           
            transform.position = new Vector3(transform.position.x, transform.position.y - speedD, transform.position.z);
        }
    }

}

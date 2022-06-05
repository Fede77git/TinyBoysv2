
using UnityEngine;

public class CopyAnim : MonoBehaviour
{

    public Transform targetAnim;
    public bool mirror;
    ConfigurableJoint joint;
    
    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();

    }

    
    void Update()
    {
        if (!mirror)
        {
            joint.targetRotation = targetAnim.rotation;
        }
        else
        {

            joint.targetRotation = Quaternion.Inverse(targetAnim.rotation);
        }
        
    }
}

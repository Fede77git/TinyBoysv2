using UnityEngine;

public class SlipperySurface : MonoBehaviour
{
    void Start()
    {
       
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            
            PhysicMaterial slipperyMat = new PhysicMaterial("Slippery");
            slipperyMat.dynamicFriction = 0f;
            slipperyMat.staticFriction = 0f;
              
            slipperyMat.frictionCombine = PhysicMaterialCombine.Minimum;
            slipperyMat.bounciness = 0f;
            
            col.material = slipperyMat;
        }
    }
}

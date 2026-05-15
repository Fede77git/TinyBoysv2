using UnityEngine;

public class ActiveRagdollSync : MonoBehaviour
{
    public Transform animatedRoot;
    public Transform physicalRoot;

    private ConfigurableJoint[] joints;
    private Transform[] animatedBones;
    private Quaternion[] initialJointRotations;

    void Start()
    {
        joints = physicalRoot.GetComponentsInChildren<ConfigurableJoint>();
        animatedBones = new Transform[joints.Length];
        initialJointRotations = new Quaternion[joints.Length];

        for (int i = 0; i < joints.Length; i++)
        {
            Transform physicalBone = joints[i].transform;
            animatedBones[i] = FindChildByName(animatedRoot, physicalBone.name);
            initialJointRotations[i] = physicalBone.localRotation;
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < joints.Length; i++)
        {
            if (animatedBones[i] != null)
            {
                joints[i].targetRotation = Quaternion.Inverse(animatedBones[i].localRotation) * initialJointRotations[i];
            }
        }
    }

    Transform FindChildByName(Transform parent, string name)
    {
        if (parent.name == name) return parent;

        foreach (Transform child in parent)
        {
            Transform result = FindChildByName(child, name);
            if (result != null) return result;
        }

        return null;
    }
}

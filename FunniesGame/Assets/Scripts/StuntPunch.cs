using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntPunch : MonoBehaviour
{
    public float stunDuration = 2f;
    private Rigidbody[] ragdollRigidbodies;
    private ConfigurableJoint[] ragdollJoints;
    private CopyAnim[] copyAnims;
    private bool isStunned = false;

    private Dictionary<ConfigurableJoint, float> origXSpring = new Dictionary<ConfigurableJoint, float>();
    private Dictionary<ConfigurableJoint, float> origYZSpring = new Dictionary<ConfigurableJoint, float>();
    private Dictionary<ConfigurableJoint, float> origSlerpSpring = new Dictionary<ConfigurableJoint, float>();

    private Dictionary<Rigidbody, RigidbodyConstraints> origConstraints = new Dictionary<Rigidbody, RigidbodyConstraints>();

    public AudioClip punchSound;
    [Range(0f, 1f)] public float punchVolume = 1f;
    public AudioClip stunSound;
    [Range(0f, 1f)] public float stunVolume = 1f;
    private AudioSource audioSource;

    private PlayerController playerController;

    void Start()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollJoints = GetComponentsInChildren<ConfigurableJoint>();
        copyAnims = GetComponentsInChildren<CopyAnim>();
        playerController = GetComponent<PlayerController>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        foreach (var joint in ragdollJoints)
        {
            origXSpring[joint] = joint.angularXDrive.positionSpring;
            origYZSpring[joint] = joint.angularYZDrive.positionSpring;
            origSlerpSpring[joint] = joint.slerpDrive.positionSpring;
        }

        foreach (var rb in ragdollRigidbodies)
        {
            origConstraints[rb] = rb.constraints;
        }
    }

    public void ReceivePunch(Vector3 direction, float force, bool shouldStun)
    {
        bool wasHolding = false;
        Pick[] picks = GetComponentsInChildren<Pick>();
        if (picks != null)
        {
            foreach (Pick p in picks)
            {
                if (p.grabbedRb != null)
                {
                    wasHolding = true;
                    p.ForceDrop();
                }
            }
        }

        if (wasHolding)
        {
            shouldStun = true;
        }

        float distributedForce = force * 0.20f;
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            if (rb != null)
            {
                if (shouldStun && !isStunned)
                    rb.constraints = RigidbodyConstraints.None;

                rb.AddForce(direction * distributedForce, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * distributedForce * 0.5f, ForceMode.Impulse);
            }
        }

        if (shouldStun && !isStunned)
        {
            if (stunSound != null && audioSource != null)
                audioSource.PlayOneShot(stunSound, stunVolume);
            
            StartCoroutine(StunRoutine());
        }
        else
        {
            if (punchSound != null && audioSource != null)
                audioSource.PlayOneShot(punchSound, punchVolume);
        }
    }

    private IEnumerator StunRoutine()
    {
        isStunned = true;

        if (playerController != null)
            playerController.enabled = false;

        foreach (var copy in copyAnims)
            copy.enabled = false;

        foreach (var joint in ragdollJoints)
        {
            float multiplier = joint.gameObject.name.Contains("Head") ? 0.8f : 0.25f;

            JointDrive driveX = joint.angularXDrive;
            driveX.positionSpring = origXSpring[joint] * multiplier;
            joint.angularXDrive = driveX;

            JointDrive driveYZ = joint.angularYZDrive;
            driveYZ.positionSpring = origYZSpring[joint] * multiplier;
            joint.angularYZDrive = driveYZ;

            JointDrive driveSlerp = joint.slerpDrive;
            driveSlerp.positionSpring = origSlerpSpring[joint] * multiplier;
            joint.slerpDrive = driveSlerp;
        }

        yield return new WaitForSeconds(stunDuration);

        foreach (var joint in ragdollJoints)
        {
            JointDrive driveX = joint.angularXDrive;
            driveX.positionSpring = origXSpring[joint];
            joint.angularXDrive = driveX;

            JointDrive driveYZ = joint.angularYZDrive;
            driveYZ.positionSpring = origYZSpring[joint];
            joint.angularYZDrive = driveYZ;

            JointDrive driveSlerp = joint.slerpDrive;
            driveSlerp.positionSpring = origSlerpSpring[joint];
            joint.slerpDrive = driveSlerp;
        }

        if (playerController != null && playerController.pelvis != null)
        {

            playerController.pelvis.position += Vector3.up * 0.5f;
            playerController.pelvis.rotation = Quaternion.Euler(0, playerController.pelvis.rotation.eulerAngles.y, 0);
            playerController.pelvis.velocity = Vector3.zero;
            playerController.pelvis.angularVelocity = Vector3.zero;
        }

        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            if (rb != null && origConstraints.ContainsKey(rb))
            {
                rb.constraints = origConstraints[rb];
            }
        }

        foreach (var copy in copyAnims)
            copy.enabled = true;

        if (playerController != null)
            playerController.enabled = true;

        isStunned = false;
    }
}

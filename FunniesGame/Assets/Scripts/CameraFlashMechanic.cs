using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LightTriggerForwarder : MonoBehaviour
{
    public CameraFlashMechanic mainScript;
    public int lightIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (mainScript != null) mainScript.HandleTriggerEnter(other, lightIndex);
    }

    private void OnTriggerExit(Collider other)
    {
        if (mainScript != null) mainScript.HandleTriggerExit(other, lightIndex);
    }
}

public class CameraFlashMechanic : MonoBehaviour
{
    public enum RandomEvent { Normal, Blackout, CrazyLight, TripleLight, ZeroGravity }

    public Light spotlight;
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;
    public BoxCollider movementArea;
    public Transform cameraModel;
    public Light lensFlashLight;
    public Light mainLevelLight;
    public float blackoutDuration = 5f;
    public float moveSpeed = 2f;
    public float minMoveSpeed = 1.5f;
    public float maxMoveSpeed = 5f;
    public float minSpotAngle = 25f;
    public float maxSpotAngle = 55f;
    public float cycleTime = 15f;
    public float warningTime = 3f;
    public float freezeDuration = 3f;
    public float trackingOffset = 0f;
    public float boundsMargin = 1.5f;
    public float chargeSpeed = 15f;
    public Material grayMaterial;
    
    public AudioClip beepSound;
    [Range(0f, 1f)] public float beepVolume = 1f;
    public AudioClip flashSound;
    [Range(0f, 1f)] public float flashVolume = 1f;
    public AudioClip chargeSound;
    [Range(0f, 1f)] public float chargeVolume = 1f;
    private AudioSource audioSource;
    private AudioSource chargeAudioSource;

    public TextMeshProUGUI eventAnnouncementText;
    public float announcementDuration = 2f;

    private Vector3 currentTargetPosition;
    private Dictionary<int, HashSet<GameObject>> playersPerLight = new Dictionary<int, HashSet<GameObject>>();
    private bool isFlashing = false;
    private float currentMoveSpeed;
    private float targetSpotAngle;
    private bool hasFlashedOnce = false;

    private RandomEvent currentEvent = RandomEvent.Normal;
    private RandomEvent previousEvent = RandomEvent.Normal;
    private float defaultSpotAngle;
    private Vector3 originalCameraLocalPos;

    
    private Light[] extraLights = new Light[2];
    private Transform[] extraTransforms = new Transform[2];
    private Vector3[] extraTargets = new Vector3[2];
    private float[] extraSpeeds = new float[2];
    private float[] extraTargetAngles = new float[2];
    private Vector3 lastCameraShakeOffset = Vector3.zero;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        chargeAudioSource = gameObject.AddComponent<AudioSource>();
        chargeAudioSource.loop = true;
        chargeAudioSource.spatialBlend = 0f;

        currentMoveSpeed = moveSpeed;
        if (spotlight != null)
        {
            spotlight.color = normalColor;
            targetSpotAngle = spotlight.spotAngle;
            defaultSpotAngle = spotlight.spotAngle;
            
            for (int i = 0; i < 2; i++)
            {
                GameObject clone = Instantiate(this.gameObject, transform.parent);
                
                CameraFlashMechanic cloneScript = clone.GetComponent<CameraFlashMechanic>();
                if (cloneScript != null) Destroy(cloneScript);
                
                foreach (AudioSource a in clone.GetComponentsInChildren<AudioSource>()) Destroy(a);

                LightTriggerForwarder forwarder = clone.AddComponent<LightTriggerForwarder>();
                forwarder.mainScript = this;
                forwarder.lightIndex = i + 1;

                Light[] cloneLights = clone.GetComponentsInChildren<Light>();
                Light foundLight = null;
                foreach (Light l in cloneLights)
                {
                    if (l.type == LightType.Spot) 
                    {
                        foundLight = l;
                        break;
                    }
                }
                if (foundLight == null && cloneLights.Length > 0) foundLight = cloneLights[0];

                extraLights[i] = foundLight;
                
                extraTransforms[i] = clone.transform;
                extraTransforms[i].position = new Vector3(0, -1000, 0);
                extraTransforms[i].gameObject.SetActive(false);
            }
        }
        
        if (cameraModel != null)
        {
            originalCameraLocalPos = cameraModel.localPosition;
        }

        if (lensFlashLight != null)
        {
            lensFlashLight.enabled = false;
        }
        PickNewRandomTarget(0);
        StartCoroutine(FlashCycleRoutine());
    }

    private void Update()
    {
        if (spotlight != null)
        {
            spotlight.spotAngle = Mathf.MoveTowards(spotlight.spotAngle, targetSpotAngle, Time.deltaTime * 10f);
        }
        
        if (currentEvent == RandomEvent.TripleLight)
        {
            for (int i = 0; i < 2; i++)
            {
                if (extraLights[i] != null && extraLights[i].enabled)
                {
                    extraLights[i].spotAngle = Mathf.MoveTowards(extraLights[i].spotAngle, extraTargetAngles[i], Time.deltaTime * 10f);
                }
            }
        }

        if (!isFlashing && movementArea != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, currentMoveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, currentTargetPosition) < 0.1f)
            {
                PickNewRandomTarget(0);
            }

            if (currentEvent == RandomEvent.TripleLight)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (extraLights[i] != null && extraLights[i].enabled && extraTransforms[i] != null)
                    {
                        extraTransforms[i].position = Vector3.MoveTowards(extraTransforms[i].position, extraTargets[i], extraSpeeds[i] * Time.deltaTime);
                        if (Vector3.Distance(extraTransforms[i].position, extraTargets[i]) < 0.1f)
                        {
                            PickNewRandomTarget(i + 1);
                        }
                    }
                }
            }
        }

        if (cameraModel != null)
        {
            Vector3 targetCamPos = new Vector3(originalCameraLocalPos.x, originalCameraLocalPos.y, transform.localPosition.z + trackingOffset);
            cameraModel.localPosition = targetCamPos;
        }

        if (!isFlashing && LevelManager10.Instance != null)
        {
            bool anyPlayerScored = false;
            float maxChargeThisFrame = 0f;
            int maxChargePlayer = -1;

            for (int i = 0; i < 3; i++)
            {
                if (i > 0 && currentEvent != RandomEvent.TripleLight) continue;
                
                if (playersPerLight.ContainsKey(i))
                {
                    List<PlayerController> aliveInThisLight = new List<PlayerController>();
                    foreach (GameObject player in playersPerLight[i])
                    {
                        if (player != null)
                        {
                            PlayerController pc = player.GetComponent<PlayerController>();
                            if (pc != null && !pc.isDead)
                            {
                                aliveInThisLight.Add(pc);
                            }
                        }
                    }

                    if (aliveInThisLight.Count == 1)
                    {
                        int pIndex = aliveInThisLight[0].playerIndex;
                        LevelManager10.Instance.AddPlayerCharge(pIndex, chargeSpeed * Time.deltaTime);
                        
                        anyPlayerScored = true;
                        float charge = LevelManager10.Instance.GetPlayerCharge(pIndex);
                        if (charge > maxChargeThisFrame)
                        {
                            maxChargeThisFrame = charge;
                            maxChargePlayer = pIndex;
                        }
                    }
                }
            }

            if (anyPlayerScored)
            {
                if (chargeSound != null && chargeAudioSource != null)
                {
                    if (!chargeAudioSource.isPlaying)
                    {
                        chargeAudioSource.clip = chargeSound;
                        chargeAudioSource.volume = 0f;
                        chargeAudioSource.Play();
                    }
                    chargeAudioSource.volume = Mathf.MoveTowards(chargeAudioSource.volume, chargeVolume, Time.deltaTime * 5f);
                    chargeAudioSource.pitch = 1f + (maxChargeThisFrame / 100f);
                }
            }
            else
            {
                if (chargeAudioSource != null && chargeAudioSource.isPlaying)
                {
                    chargeAudioSource.volume = Mathf.MoveTowards(chargeAudioSource.volume, 0f, Time.deltaTime * 3f);
                    if (chargeAudioSource.volume <= 0f)
                    {
                        chargeAudioSource.Stop();
                    }
                }
            }
        }
        else
        {
            if (chargeAudioSource != null && chargeAudioSource.isPlaying)
            {
                chargeAudioSource.volume = Mathf.MoveTowards(chargeAudioSource.volume, 0f, Time.deltaTime * 5f);
                if (chargeAudioSource.volume <= 0f)
                {
                    chargeAudioSource.Stop();
                }
            }
        }
    }



    private void OnTriggerEnter(Collider other) { HandleTriggerEnter(other, 0); }
    private void OnTriggerExit(Collider other) { HandleTriggerExit(other, 0); }

    public void HandleTriggerEnter(Collider other, int lightIndex)
    {
        PlayerController pc = other.GetComponentInParent<PlayerController>();
        if (pc != null)
        {
            if (!playersPerLight.ContainsKey(lightIndex)) playersPerLight[lightIndex] = new HashSet<GameObject>();
            playersPerLight[lightIndex].Add(pc.gameObject);
        }
    }

    public void HandleTriggerExit(Collider other, int lightIndex)
    {
        PlayerController pc = other.GetComponentInParent<PlayerController>();
        if (pc != null && playersPerLight.ContainsKey(lightIndex))
        {
            playersPerLight[lightIndex].Remove(pc.gameObject);
        }
    }

    private void EndCurrentEvent()
    {
        if (currentEvent == RandomEvent.Blackout && mainLevelLight != null)
        {
            Shader.SetGlobalFloat("_GlobalBlackoutOutlineEnabled", 0f);
            mainLevelLight.enabled = true;
        }

        if (currentEvent == RandomEvent.TripleLight)
        {
            for (int i = 0; i < 2; i++)
            {
                if (extraTransforms[i] != null)
                {
                    extraTransforms[i].position = new Vector3(0, -1000, 0);
                    StartCoroutine(DisableNextFrame(extraTransforms[i].gameObject));
                }
            }
        }

        if (currentEvent == RandomEvent.ZeroGravity)
        {
            Physics.gravity = new Vector3(0, -25f, 0);
        }

        currentEvent = RandomEvent.Normal;
        currentMoveSpeed = moveSpeed;
        targetSpotAngle = defaultSpotAngle;
    }

    private IEnumerator DisableNextFrame(GameObject obj)
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        if (obj != null) obj.SetActive(false);
    }

    private void PickRandomEvent()
    {
        int randomEv;
        do
        {
            randomEv = Random.Range(1, 5); 
        } 
        while (hasFlashedOnce && (RandomEvent)randomEv == previousEvent);

        previousEvent = (RandomEvent)randomEv;
        string eventName = "";

        switch (randomEv)
        {
            case 0:
                currentEvent = RandomEvent.Normal;
                break;
            case 1:
                currentEvent = RandomEvent.Blackout;
                eventName = "Blackout!";
                if (mainLevelLight != null)
                {
                    mainLevelLight.enabled = false;
                    Shader.SetGlobalFloat("_GlobalBlackoutOutlineEnabled", 1f);
                    Shader.SetGlobalColor("_GlobalBlackoutOutlineColor", Color.cyan);
                }
                break;
            case 2:
                currentEvent = RandomEvent.CrazyLight;
                eventName = "Crazy Light!";
                PickNewRandomTarget(0);
                break;
            case 3:
                currentEvent = RandomEvent.TripleLight;
                eventName = "Triple Light!";
                for (int i = 0; i < 2; i++)
                {
                    if (extraTransforms[i] != null)
                    {
                        extraTransforms[i].gameObject.SetActive(true);
                        extraTransforms[i].position = transform.position;
                        if (extraLights[i] != null) extraLights[i].color = normalColor;
                        PickNewRandomTarget(i + 1);
                    }
                }
                break;
            case 4:
                currentEvent = RandomEvent.ZeroGravity;
                eventName = "Zero Gravity!";
                Physics.gravity = new Vector3(0, -1.5f, 0);
                break;
        }

        if (eventAnnouncementText != null && !string.IsNullOrEmpty(eventName))
        {
            StartCoroutine(ShowEventAnnouncement(eventName));
        }
    }

    private IEnumerator ShowEventAnnouncement(string text)
    {
        eventAnnouncementText.text = text;
        eventAnnouncementText.gameObject.SetActive(true);
        yield return new WaitForSeconds(announcementDuration);
        eventAnnouncementText.gameObject.SetActive(false);
    }

    private IEnumerator FlashCycleRoutine()
    {
        while (true)
        {
            isFlashing = false;
            if (spotlight != null) spotlight.color = normalColor;
            
            if (currentEvent == RandomEvent.TripleLight)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (extraLights[i] != null) extraLights[i].color = normalColor;
                }
            }

            yield return new WaitForSeconds(cycleTime);

            float timer = 0f;
            bool wasLightOn = false;

            while (timer < warningTime)
            {
                timer += Time.deltaTime;
                
                float progress = timer / warningTime;
                float blinkFrequency = Mathf.Lerp(10f, 50f, progress);
                float wave = Mathf.Sin(timer * blinkFrequency);
                bool isLightOn = wave > 0f;
                
                if (isLightOn && !wasLightOn)
                {
                    if (beepSound != null && audioSource != null)
                    {
                        audioSource.PlayOneShot(beepSound, beepVolume);
                    }
                }
                
                if (spotlight != null)
                {
                    spotlight.color = isLightOn ? warningColor : Color.black;
                }
                
                if (currentEvent == RandomEvent.TripleLight)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (extraLights[i] != null && extraTransforms[i].gameObject.activeSelf)
                        {
                            extraLights[i].color = isLightOn ? warningColor : Color.black;
                        }
                    }
                }
                
                wasLightOn = isLightOn;

                yield return null;
            }
            
            if (spotlight != null) spotlight.color = warningColor;

            if (currentEvent == RandomEvent.TripleLight)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (extraLights[i] != null && extraTransforms[i].gameObject.activeSelf)
                    {
                        extraLights[i].color = warningColor;
                    }
                }
            }

            isFlashing = true;

            if (flashSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(flashSound, flashVolume);
            }

            HashSet<GameObject> allPlayersToFreeze = new HashSet<GameObject>();
            foreach (var kvp in playersPerLight)
            {
                if (kvp.Key > 0 && currentEvent != RandomEvent.TripleLight) continue;
                foreach (GameObject player in kvp.Value)
                {
                    if (player != null) allPlayersToFreeze.Add(player);
                }
            }

            foreach (GameObject player in allPlayersToFreeze)
            {
                StartCoroutine(FreezePlayerRoutine(player));
            }

            if (lensFlashLight != null) lensFlashLight.enabled = true;
            
            yield return new WaitForSeconds(0.5f);
            
            if (lensFlashLight != null) lensFlashLight.enabled = false;

            if (hasFlashedOnce)
            {
                EndCurrentEvent();
            }

            hasFlashedOnce = true;
            PickRandomEvent();
            PickNewRandomTarget(0);
        }
    }

    private void PickNewRandomTarget(int lightIndex)
    {
        if (hasFlashedOnce && lightIndex == 0)
        {
            if (currentEvent == RandomEvent.CrazyLight)
            {
                currentMoveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
                targetSpotAngle = Random.Range(minSpotAngle, maxSpotAngle);
            }
            else
            {
                currentMoveSpeed = moveSpeed;
                targetSpotAngle = defaultSpotAngle;
            }
        }
        else if (hasFlashedOnce && lightIndex > 0)
        {
            extraSpeeds[lightIndex - 1] = moveSpeed;
            extraTargetAngles[lightIndex - 1] = defaultSpotAngle;
        }

        if (movementArea != null)
        {
            Bounds bounds = movementArea.bounds;
            
            float minX = bounds.min.x + boundsMargin;
            float maxX = bounds.max.x - boundsMargin;
            float minZ = bounds.min.z + boundsMargin;
            float maxZ = bounds.max.z - boundsMargin;

            if (minX > maxX) { minX = bounds.center.x; maxX = bounds.center.x; }
            if (minZ > maxZ) { minZ = bounds.center.z; maxZ = bounds.center.z; }

            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);
            Vector3 pos = new Vector3(randomX, transform.position.y, randomZ);

            if (lightIndex == 0) currentTargetPosition = pos;
            else extraTargets[lightIndex - 1] = pos;
        }
    }

    private IEnumerator FreezePlayerRoutine(GameObject player)
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null) pc.enabled = false;

        Animator[] animators = player.GetComponentsInChildren<Animator>();
        foreach (Animator anim in animators)
        {
            anim.speed = 0;
        }

        Rigidbody[] rigidbodies = player.GetComponentsInChildren<Rigidbody>();
        Renderer[] renderers = player.GetComponentsInChildren<Renderer>();
        
        Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
        }

        foreach (Renderer rend in renderers)
        {
            if (rend is ParticleSystemRenderer || rend is TrailRenderer) continue;

            originalMaterials[rend] = rend.materials;
            Material[] grayMats = new Material[rend.materials.Length];
            for (int i = 0; i < grayMats.Length; i++)
            {
                grayMats[i] = grayMaterial;
            }
            rend.materials = grayMats;
        }

        yield return new WaitForSeconds(freezeDuration);

        if (player != null)
        {
            if (pc != null) pc.enabled = true;

            foreach (Animator anim in animators)
            {
                if (anim != null) anim.speed = 1;
            }

            foreach (Rigidbody rb in rigidbodies)
            {
                if (rb != null)
                {
                    rb.isKinematic = false;
                }
            }

            Pick pick = player.GetComponentInChildren<Pick>();
            bool isGrabbing = (pick != null && pick.grabbedRb != null);

            foreach (Renderer rend in renderers)
            {
                if (rend != null && originalMaterials.ContainsKey(rend))
                {
                    Material[] restoredMats = originalMaterials[rend];
                    foreach (Material m in restoredMats)
                    {
                        if (m.HasProperty("_OutlineEnabled"))
                        {
                            m.SetFloat("_OutlineEnabled", isGrabbing ? 1f : 0f);
                        }
                    }
                    rend.materials = restoredMats;
                }
            }
        }
    }

    private void OnDestroy()
    {
        Shader.SetGlobalFloat("_GlobalBlackoutOutlineEnabled", 0f);
        Physics.gravity = new Vector3(0, -25f, 0);
    }
}

using System.Collections;
using UnityEngine;

public class LaserKill : MonoBehaviour
{
    private LineRenderer line;
    public Transform startPoint;
    public float rotationDelay = 3f;
    public float rotationSpeed = 20f;
    public float speedAcceleration = 0.5f;
    public float gameOverDelay = 1.5f;
    public GameObject deathParticle;
    public AudioSource deathSound;

    private bool isActive = false;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        Vector3 o = startPoint != null ? startPoint.position : transform.position;
        line.SetPosition(0, o);
        line.SetPosition(1, o + transform.right * 5000);
        line.enabled = true;
        Invoke(nameof(Activate), rotationDelay);
    }

    void Activate()
    {
        isActive = true;
        InvokeRepeating(nameof(IncreaseSpeed), 1f, 1f);
    }

    void IncreaseSpeed()
    {
        rotationSpeed += speedAcceleration;
    }

    void Update()
    {
        Vector3 origin = startPoint != null ? startPoint.position : transform.position;
        line.SetPosition(0, origin);

        if (isActive)
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        RaycastHit hit;
        if (Physics.Raycast(origin, transform.right, out hit))
        {
            line.SetPosition(1, hit.point);

            if (isActive)
            {
                string tag = hit.transform.tag;
                if (tag == "Player" || tag.StartsWith("Head"))
                {
                    PlayerController pc = FindNearest(hit.point);
                    if (pc != null && !pc.isDead)
                    {
                        pc.isDead = true;

                        if (deathParticle != null)
                        {
                            GameObject fx = Instantiate(deathParticle, hit.point, Quaternion.identity);
                            Destroy(fx, 3f);
                        }

                        if (deathSound != null)
                        {
                            deathSound.pitch = Random.Range(0.9f, 1.1f);
                            deathSound.Play();
                        }

                        StartCoroutine(NotifyAfterDelay(pc.playerIndex));
                    }
                }
            }
        }
        else
        {
            line.SetPosition(1, origin + transform.right * 5000);
        }
    }

    PlayerController FindNearest(Vector3 point)
    {
        PlayerController[] all = FindObjectsOfType<PlayerController>();
        PlayerController nearest = null;
        float minDist = float.MaxValue;
        foreach (PlayerController p in all)
        {
            float d = Vector3.Distance(p.transform.position, point);
            if (d < minDist) { minDist = d; nearest = p; }
        }
        return nearest;
    }

    IEnumerator NotifyAfterDelay(int index)
    {
        yield return new WaitForSeconds(gameOverDelay);
        if (LevelManager3.Instance != null) LevelManager3.Instance.PlayerDied(index);
        else if (LevelManager.Instance != null) LevelManager.Instance.PlayerDied(index);
        else if (LevelManager2.Instance != null) LevelManager2.Instance.PlayerDied(index);
    }
}

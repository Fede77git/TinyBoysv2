using UnityEngine;

public class LaserKill : MonoBehaviour
{
    private LineRenderer line;
    public Transform startPoint;
    public int timerDelay = 3;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        Invoke(nameof(ActivateLaser), timerDelay);
    }

    void ActivateLaser()
    {
        line.enabled = true;
    }

    void Update()
    {
        if (!line.enabled) return;

        line.SetPosition(0, startPoint != null ? startPoint.position : transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit))
        {
            line.SetPosition(1, hit.point);

            string tag = hit.transform.tag;

            if (tag.StartsWith("Player"))
            {
                PlayerController pc = hit.transform.GetComponentInParent<PlayerController>();

                if (pc != null && !pc.isDead)
                {
                    pc.Dead();
                }

                Destroy(hit.transform.gameObject);
            }
        }
        else
        {
            line.SetPosition(1, transform.right * 5000);
        }
    }
}

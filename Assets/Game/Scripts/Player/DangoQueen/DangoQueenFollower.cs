using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangoQueenFollower : DangoQueenBehaviour
{
    public float distoffset;

    private DangoBehaviour dangoMain;
    private List<Platform> platformRecord;
    private Platform currentPlatform;
    private float distance;
    private bool following;
    private bool foundAButton;

    private void Start()
    {
        foundAButton = false;
        following = false;
        platformRecord = new List<Platform>();
        currentPlatform = transform.parent.GetComponent<Platform>();
        anim = GetComponent<Animator>();
        activePlatform = currentPlatform;
    }

    private void Update()
    {
        if (foundAButton) return;
        if (dangoMain == null) return;

        if (!following)
        {
            distance = Mathf.Sqrt(Mathf.Pow(this.transform.position.x - dangoMain.transform.position.x, 2) +
            Mathf.Pow(this.transform.position.y - dangoMain.transform.position.y, 2));
            if (distance < distoffset && dangoMain.HasItem())
            {
                following = true;
                anim.SetTrigger("Happy");
            }
        }
        else
        {
            Follow();
        }
    }

    private void Follow()
    {
        if (dangoMain != null)
        {
            currentPlatform = dangoMain.GetCurrentPlatform();
            if (!platformRecord.Contains(currentPlatform) && currentPlatform != activePlatform)
            {
                //cek posisi current sudah disimpan apa belum 
                platformRecord.Add(currentPlatform);
            }

            if (currentPlatform != activePlatform && platformRecord[0] != currentPlatform)
            {
                //ngecek apakah player masih di platform yang terekam sebelumnya atau belum , kalau sdh tidak ada baru jalan ke platform itu
                MoveToPlatform(platformRecord[0]);
                platformRecord.RemoveAt(0);
            }
        }
    }

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            dangoMain = collision.gameObject.GetComponent<DangoBehaviour>();
            currentPlatform = transform.parent.GetComponent<Platform>();
            activePlatform = currentPlatform;
        }
    }
}

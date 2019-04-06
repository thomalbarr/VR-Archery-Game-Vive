using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private bool isAttached = false;
    private bool isFired = false;
    private bool hasTriggered = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Target")
        {
            collision.collider.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(5, 5, 5), collision.contacts[0].point, ForceMode.Force);
            if (!hasTriggered)
            {
                ArrowManager.Instance.targetsHit++;
                float distance;
                distance = Vector3.Distance(collision.collider.transform.position, ArrowManager.Instance.transform.position);
                ArrowManager.Instance.AddScore(distance, false);
                hasTriggered = true;
            }
        } else if (collision.collider.tag == "Terrain"|| collision.collider.tag == "Platform")
        {
            Destroy(this);
        }
    }

    void DelayedDestroy()
    {
        Destroy(this.gameObject, 5);
    }

    void OnTriggerStay()
    {
        if (!isAttached)
        {
            AttachArrow();
        }
    }

    private void OnTriggerEnter()
    {
        if (!isAttached)
        {
            AttachArrow();
        }
    }

    private void Update()
    {
        if (isFired)
        {
            DelayedDestroy();
            transform.LookAt(transform.position + transform.GetComponent<Rigidbody>().velocity);
        }

    }

    public void Fired()
    {
        isFired = true;
    }

    private void AttachArrow()
    {

        var device = SteamVR_Controller.Input((int)ArrowManager.Instance.trackedObj.index);
        if (!isAttached && device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            ArrowManager.Instance.AttachBowToArrow();
            isAttached = true;
        }
    }
}

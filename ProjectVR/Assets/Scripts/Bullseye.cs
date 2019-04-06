using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullseye : MonoBehaviour {

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Arrow")
        {
            if (!hasTriggered)
            {
                float distance;
                distance = Vector3.Distance(collider.transform.position, ArrowManager.Instance.transform.position);
                ArrowManager.Instance.AddScore(distance * 2, true);
                hasTriggered = true;
            }
        }
    }
}

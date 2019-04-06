using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachPlatFormFromTarget : MonoBehaviour {

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "Platform")
        {
            transform.parent = null;
        }
    }


}

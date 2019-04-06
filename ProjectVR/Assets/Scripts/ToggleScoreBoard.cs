using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleScoreBoard : MonoBehaviour
{

    public SteamVR_TrackedObject trackedObj;
    private int active;

    // Use this for initialization
    void Start()
    {
        active = -1;
        gameObject.transform.Translate(new Vector3(0, active * 1000, 0));

    }

    // Update is called once per frame
    void Update()
    {
        var device = SteamVR_Controller.Input((int)trackedObj.index);
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            active *= -1;
            gameObject.transform.Translate(new Vector3(0, active * 1000, 0));
        }
    }
}

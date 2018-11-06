using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandManipulation : MonoBehaviour
{
    Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);

	void Start ()
    {

    }
	
	void Update ()
    {
        if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            //GameObject toCatch = null;
            //toCatch.transform.position = transform.position + offset;
            Debug.Log("Fuck off");
        }
    }
}

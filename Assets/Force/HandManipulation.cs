using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandManipulation : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject pickableObject;
    private GameObject pickedObject;
    public bool isRightHand;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pickableObject = null;
    }

    void Update()
    {

        if (GrabGripDown())
        {
            pickedObject = pickableObject;
            pickedObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if (GrabGripUp())
        {
            Debug.Log(pickedObject);
            if (pickedObject)
            {
                if (isRightHand)
                {
                    pickedObject.GetComponent<Rigidbody>().isKinematic = false;
                    pickedObject.GetComponent<Rigidbody>().velocity = SteamVR_Input._default.inActions.SkeletonRightHand.GetVelocity(SteamVR_Input_Sources.RightHand);
                }
                else
                {
                    pickedObject.GetComponent<Rigidbody>().isKinematic = false;
                    pickedObject.GetComponent<Rigidbody>().velocity = SteamVR_Input._default.inActions.SkeletonLeftHand.GetVelocity(SteamVR_Input_Sources.LeftHand);
                }
                pickedObject = null;
            }
            Debug.Log(pickedObject);
        }
        if (pickedObject)
        {
            pickedObject.transform.position = transform.position;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            pickableObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            pickableObject = null;
        }
    }

    bool GrabGripDown()
    {
        if (isRightHand)
        {
            return SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.RightHand);
        }
        return SteamVR_Input._default.inActions.GrabGrip.GetStateDown(SteamVR_Input_Sources.LeftHand);
    }

    bool GrabGripUp()
    {
        if (isRightHand)
        {
            return SteamVR_Input._default.inActions.GrabGrip.GetStateUp(SteamVR_Input_Sources.RightHand);
        }
        return SteamVR_Input._default.inActions.GrabGrip.GetStateUp(SteamVR_Input_Sources.LeftHand);
    }
}

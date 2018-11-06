using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public enum MANIPULATION_MODE
{
    HAND,
    FORCE
}

public class HandManipulation : MonoBehaviour
{
    public Transform LeftHand = null;
    public Transform RightHand = null;

    private MANIPULATION_MODE   m_ManipulationMode = MANIPULATION_MODE.HAND;
    private Transform           m_HandManipulating = null;
    private SteamVR_Input_Sources m_ManipulatingHandIndex;
    private List<Transform>     m_ObjectsUnderForce = new List<Transform>();

	void Start ()
    {

    }
	
	void Update ()
    {
        //dirty
        if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            m_HandManipulating = LeftHand;
            m_ManipulatingHandIndex = SteamVR_Input_Sources.LeftHand;
            StartCoroutine(ForceManipulationCoroutine());
        }
        if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            m_HandManipulating = RightHand;
            m_ManipulatingHandIndex = SteamVR_Input_Sources.RightHand;
            StartCoroutine(ForceManipulationCoroutine());
        }


    }

    private IEnumerator ForceManipulationCoroutine()
    {
        m_ManipulationMode = MANIPULATION_MODE.FORCE;

        while(m_ManipulationMode == MANIPULATION_MODE.FORCE)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
                if (hit.collider.CompareTag("Grabable"))
                {
                    if (hit.collider.GetComponent<Rigidbody>())
                    {
                        hit.collider.GetComponent<Rigidbody>().isKinematic = true;
                    }
                    m_ObjectsUnderForce.Add(hit.collider.transform);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("Did not Hit");
            }

            foreach (Transform obj in m_ObjectsUnderForce)
            {
                obj.transform.position = transform.position;
            }

            if (SteamVR_Input._default.inActions.GrabPinch.GetStateUp(m_ManipulatingHandIndex))
            {
                m_ManipulationMode = MANIPULATION_MODE.HAND;

                foreach (Transform obj in m_ObjectsUnderForce)
                {
                    if (obj.GetComponent<Rigidbody>())
                    {
                        obj.GetComponent<Rigidbody>().isKinematic = false;
                    }
                }
                m_ObjectsUnderForce.Clear();
            }

            yield return null; 
        }

        m_HandManipulating = null;
    }

}

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
    public Rigidbody ForceConnectedRb = null;

    private MANIPULATION_MODE   m_ManipulationMode = MANIPULATION_MODE.HAND;
    private Transform           m_HandManipulating = null;
    private SteamVR_Input_Sources m_ManipulatingHandIndex;
    private List<Transform>     m_ObjectsUnderForce = new List<Transform>();
    private Vector3 m_LastHandPosition = Vector3.zero;

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
        bool oops = false;
        m_ManipulationMode = MANIPULATION_MODE.FORCE;

        while(m_ManipulationMode == MANIPULATION_MODE.FORCE)
        {
            if (m_LastHandPosition == Vector3.zero)
            {
                m_LastHandPosition = m_HandManipulating.position;
            }
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Grabable"))
                {
                    if (hit.collider.GetComponent<Rigidbody>())
                    {
                        if (!hit.collider.GetComponent<SpringJoint>())
                        {
                            AddAndConfigureSpringJoint(hit.collider.GetComponent<Rigidbody>());
                        }
                    }
                    
                    m_ObjectsUnderForce.Add(hit.collider.transform);
                }
            }

            foreach (Transform obj in m_ObjectsUnderForce)
            {
                //obj.transform.position = transform.position;
                if (Vector3.Distance(obj.position, ForceConnectedRb.transform.position) < 1f)
                {
                    obj.GetComponent<Rigidbody>().isKinematic = true;

                    if (obj.GetComponent<SpringJoint>())
                    {
                        Destroy(obj.GetComponent<SpringJoint>());
                    }
                    oops = true;
                    //obj.parent = m_HandManipulating;
                    //obj.RotateAround(m_HandManipulating.position, Vector3.up, 500 * Time.deltaTime);
                }
            }

            if(oops)
            {
                foreach (Transform obj in m_ObjectsUnderForce)
                {
                    obj.position = m_HandManipulating.position;
                }

            }

            if (SteamVR_Input._default.inActions.GrabPinch.GetStateUp(m_ManipulatingHandIndex))
            {
                m_ManipulationMode = MANIPULATION_MODE.HAND;

                foreach (Transform obj in m_ObjectsUnderForce)
                {
                    if (obj.GetComponent<Rigidbody>())
                    {
                        obj.GetComponent<Rigidbody>().isKinematic = false;
                        //obj.parent = null;
                        if (obj.GetComponent<SpringJoint>())
                        {
                            Destroy(obj.GetComponent<SpringJoint>());
                        }
                        obj.GetComponent<Rigidbody>().AddForce(((m_HandManipulating.position - m_LastHandPosition) * 500) + (m_HandManipulating.forward * 50));
                        obj.GetComponent<Rigidbody>().useGravity = true;
                    }
                }
                m_ObjectsUnderForce.Clear();
            }


            m_LastHandPosition = m_HandManipulating.position;

            yield return null; 
        }

        m_HandManipulating = null;
        oops = false;
    }

    private void AddAndConfigureSpringJoint(Rigidbody iObjectRb)
    {
        iObjectRb.useGravity = false;
        SpringJoint AddedSpringJoint = iObjectRb.gameObject.AddComponent<SpringJoint>();
        AddedSpringJoint.connectedBody = ForceConnectedRb;
        AddedSpringJoint.anchor = Vector3.zero;
        AddedSpringJoint.autoConfigureConnectedAnchor = false;
        AddedSpringJoint.connectedAnchor = Vector3.zero;
        AddedSpringJoint.spring = 10;
        AddedSpringJoint.damper = 1;
        
    }

    private IEnumerator ManageDampingSpringJoint(Transform iObjectConnected)
    {
        SpringJoint joint = iObjectConnected.gameObject.GetComponent<SpringJoint>();
        while (m_ManipulationMode == MANIPULATION_MODE.FORCE)
        {
            float dist = Vector3.Distance(iObjectConnected.position, ForceConnectedRb.transform.position);

            //joint.damper = 

            yield return null;
        }
    }

}

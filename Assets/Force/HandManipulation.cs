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
    private Rigidbody   rb;
    private GameObject  pickableObject;
    private GameObject  pickedObject;
    public bool         isRightHand;

    public Transform LeftHand = null;
    public Transform RightHand = null;
    public int meanHandMovement = 40;

    private MANIPULATION_MODE       m_ManipulationMode = MANIPULATION_MODE.HAND;
    private Transform               m_HandManipulating = null;
    private SteamVR_Input_Sources   m_ManipulatingHandIndex;
    private List<Transform>         m_ObjectsUnderForce = new List<Transform>();
    private Vector3                 m_LastHandPosition = Vector3.zero;
    private Queue<Vector3>           m_LastHandPositions = new Queue<Vector3>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pickableObject = null;
    }

    void Update()
    {
        midiclorianManipulation();
        humanManipulation();
    }

    void midiclorianManipulation()
    {
        if (isRightHand)
        {
            if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                m_HandManipulating = RightHand;
                m_ManipulatingHandIndex = SteamVR_Input_Sources.RightHand;
                StartCoroutine(ForceManipulationCoroutine());
            }
        }
        else
        {
            if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                m_HandManipulating = LeftHand;
                m_ManipulatingHandIndex = SteamVR_Input_Sources.LeftHand;
                StartCoroutine(ForceManipulationCoroutine());
            }
        }

    }

    void humanManipulation()
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
            if (other.GetComponent<Collider>().bounds.size.x < 5 && other.GetComponent<Collider>().bounds.size.y < 5 && other.GetComponent<Collider>().bounds.size.z < 5)
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

    private IEnumerator ForceManipulationCoroutine()
    {
        m_ManipulationMode = MANIPULATION_MODE.FORCE;

        bool objectUsingGravity = false;
        while(m_ManipulationMode == MANIPULATION_MODE.FORCE)
        {

            //pour l'enregistrement du mouvement de la main
            if (m_LastHandPosition == Vector3.zero)
            {
                m_LastHandPosition = m_HandManipulating.position;
            }

            //on envoie un rayon devant la main.
            RaycastHit hit;
            //si le rayon touche un objet...
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                //si on n'est pas déjà en train de manipuler un objet par la force...
                if (m_ObjectsUnderForce.Count == 0)
                {
                    //si l'objet est attrapable
                    if (hit.collider.CompareTag("Pickable"))
                    {
                        if (hit.collider.GetComponent<Rigidbody>())
                        {
                            objectUsingGravity = hit.collider.GetComponent<Rigidbody>();
                            if (!hit.collider.GetComponent<SpringJoint>())
                            {
                                //on relie la main et l'objet par un spring joint.
                                AddAndConfigureSpringJoint(hit.collider.GetComponent<Rigidbody>());
                            }
                        }

                        m_ObjectsUnderForce.Add(hit.collider.transform);
                    }
                }
            }

            if (SteamVR_Input._default.inActions.GrabPinch.GetStateUp(m_ManipulatingHandIndex))
            {
                m_ManipulationMode = MANIPULATION_MODE.HAND;

                foreach (Transform obj in m_ObjectsUnderForce)
                {
                    if (obj.GetComponent<Rigidbody>())
                    {
                        if (obj.GetComponent<SpringJoint>())
                        {
                            Destroy(obj.GetComponent<SpringJoint>());
                        }

                        Vector3 forceDirection = Vector3.zero;
                        foreach (Vector3 handPosition in m_LastHandPositions)
                        {
                            forceDirection += handPosition;
                        }
                        forceDirection /= m_LastHandPositions.Count;

                        obj.GetComponent<Rigidbody>().velocity += (((m_HandManipulating.position - m_LastHandPosition) * 1000));
                        obj.GetComponent<Rigidbody>().useGravity = objectUsingGravity;
                    }
                }
                m_ObjectsUnderForce.Clear();
            }

            m_LastHandPositions.Enqueue(m_HandManipulating.position);
            while (m_LastHandPositions.Count > meanHandMovement)
            {
                m_LastHandPositions.Dequeue();
            }

            m_LastHandPosition = m_HandManipulating.position;

            yield return null; 
        }

        m_HandManipulating = null;
    }

    private void AddAndConfigureSpringJoint(Rigidbody iObjectRb)
    {
        iObjectRb.useGravity = false;
        SpringJoint AddedSpringJoint = iObjectRb.gameObject.AddComponent<SpringJoint>();
        LookUpTableSpringJoint objectSpringJointCaracteristics = iObjectRb.GetComponent<LookUpTableSpringJoint>();
        AddedSpringJoint.connectedBody = rb;
        AddedSpringJoint.anchor = Vector3.zero;
        AddedSpringJoint.autoConfigureConnectedAnchor = false;
        AddedSpringJoint.connectedAnchor = Vector3.zero;
        
        AddedSpringJoint.spring = objectSpringJointCaracteristics.StrengthJoint;
        AddedSpringJoint.damper = objectSpringJointCaracteristics.SpringDamping;
    }
}

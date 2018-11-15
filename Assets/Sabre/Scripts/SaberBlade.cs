using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberBlade : MonoBehaviour {

    public float BladeStrength = 10f;
    private Rigidbody m_Rb = null;

    private Vector3 m_LastPosition = Vector3.zero;
    private Vector3 m_Direction = Vector3.zero;

	void Start ()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_LastPosition = transform.position;
    }
	
    void Update ()
    {
        m_Direction = transform.position - m_LastPosition;
        m_LastPosition = transform.position;
    }

    void OnTriggerEnter(Collider collider)
    {
        Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
        if (rb)
        {
            collider.gameObject.GetComponent<Rigidbody>().velocity = m_Direction * BladeStrength;
        }
    }
}

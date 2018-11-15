using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SaberBlade : MonoBehaviour {

    public float BladeStrength = 10f;
    private Rigidbody m_Rb = null;
    //[SteamVR_DefaultAction("Haptic")]
    public SteamVR_Action_Vibration hapticSignal;
   // public SteamVR_Input_Sources controller;
    private Vector3 m_LastPosition = Vector3.zero;
    private Vector3 m_Direction = Vector3.zero;

    public float timeBeforeActivation = 0.0f;
    public float Duration = 0.1f;
    public float Frequency = 1.0f;
    public float Amplitude = 2.0f;

    void Start ()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_LastPosition = transform.position;
        //StartCoroutine(DebugVibrationCoroutine());
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

    private IEnumerator DebugVibrationCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(2);
            hapticSignal.Execute(timeBeforeActivation, Duration, Frequency, Amplitude, SteamVR_Input_Sources.RightHand);

        }
    }
}

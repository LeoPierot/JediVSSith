using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownWithDistance : MonoBehaviour {

    public float DistanceBeforSlowDown = 150;
    public float SlowDownStepDivisor = .8f;
    public float SlowDownStepTime = .2f;
    private Vector3 OriginalPosition = Vector3.zero;
    private bool m_SlowingDown = false;
    private Rigidbody m_Rb = null;

	void Start ()
    {
        OriginalPosition = transform.position;
        m_Rb = GetComponent<Rigidbody>();
    }
	
	void Update ()
    {
        if(!m_SlowingDown)
        {
            if (Vector3.Distance(transform.position, OriginalPosition) > DistanceBeforSlowDown)
            {
                StartCoroutine(SlowDownCoroutine());
            }
        }
        
	}

    private IEnumerator SlowDownCoroutine()
    {
        m_SlowingDown = true;
        while(m_Rb.velocity.magnitude > 10f)
        {
            yield return new WaitForSeconds(SlowDownStepTime);
            m_Rb.velocity -= m_Rb.velocity * SlowDownStepDivisor;
        }
        Destroy(this);
    }
}

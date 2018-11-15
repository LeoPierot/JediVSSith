using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour {

    public AudioSource AudioSource = null;
    public AudioClip[] Clips = null;
    

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("bang");
        AudioClip randomclip = Clips[Random.Range(0,3)];

        AudioSource.clip = randomclip;
        if (!AudioSource.isPlaying)
        {
            AudioSource.Play();
        }
    }
}

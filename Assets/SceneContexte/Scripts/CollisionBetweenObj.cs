using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class qui active le son de collision lorsque l'objet rencontre un autre
/// </summary>
public class CollisionBetweenObj : MonoBehaviour
{

    public AudioSource source;

    // Use this for initialization
    void Start()
    {
 
        // Fetch the AudioSource from the GameObjec
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// detection de la collision
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        // Todo : ecrire les différents conditions lors de la collision 
        if (col.gameObject.name == "Zone_Fight")
        {
            source.Play();
        }
        else
        {
            source.Play();
        }
    }
}

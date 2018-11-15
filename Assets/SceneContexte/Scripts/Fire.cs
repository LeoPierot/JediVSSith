using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

    public AudioSource Audio;
    public GameObject Vaisseau;
    public GameObject Ball;
    private GameObject  fireBall;
    Vector3 PosVaisseau;
    public float Speed;
    private Vector3 direction = Vector3.forward;
    private Vector3 velocity;
    private float time;
    public float ResetTimer;
 
    // Use this for initialization
    void Start () {
        PosVaisseau = new Vector3();
        fireBall =Instantiate(Ball);
        PosVaisseau = Vaisseau.transform.position;
        velocity = Speed * direction;
        fireBall.transform.position = PosVaisseau;
        Audio.Play();
        time = 0;
    }   

    // Update is called once per frame
    void Update () {
        time +=Time.deltaTime;
        if(time > ResetTimer)
        {
            PosVaisseau = Vaisseau.transform.position;
            velocity = Speed * direction;
            fireBall.transform.position = PosVaisseau;
            Audio.Play();
            time = 0;
        }
        fireBall.transform.position += velocity;
    }

    public void DoFire()
    {
        GameObject fireBall = AddBall(Ball);
        fireBall.transform.position = PosVaisseau;
        fireBall.transform.position += velocity;

        Debug.Log(velocity);
    }
     private GameObject AddBall(GameObject obj)
    {
        return obj;
    }
    private void SetPos(Vector3 pos)
    {
    }

    private void getPosVaisseau()
    {

    }
    private void SetSpeed(float speed)
    {
        this.Speed = speed;
    }

    

}

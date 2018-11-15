using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour {

	public Rigidbody projectile;
	public GameObject camera;

	public float bulletSpeed = 5f;
	public float spacing = 0.25f;

	void Start () {
		 InvokeRepeating("LaunchProjectile", 2.0f, 1.5f);
	}
	

	void LaunchProjectile() {
		this.transform.LookAt(camera.transform);
		float rand = Random.Range(-1, 2)* spacing;
		Rigidbody instance = Instantiate(projectile, this.transform.position + this.transform.forward + this.transform.right*rand, Quaternion.identity);
		instance.tag = "Pickable";
		instance.velocity = bulletSpeed*this.transform.forward;
	}
}

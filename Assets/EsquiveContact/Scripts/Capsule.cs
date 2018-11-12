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
	
	void update(){
		GameObject.Find("[CameraRig]/ColliderJambes").transform.rotation = Quaternion.Euler(0, this.transform.rotation.y,0);
	}

	void LaunchProjectile() {
		this.transform.LookAt(camera.transform);
		float rand = Random.Range(-1, 2)* spacing;
		Rigidbody instance = Instantiate(projectile, this.transform.position + this.transform.forward + this.transform.right*rand, Quaternion.identity);
		instance.tag = "damage";
		instance.velocity = bulletSpeed*this.transform.forward;
		Destroy(instance.gameObject, 10f);
	}
}

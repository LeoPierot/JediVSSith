using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPlayer : MonoBehaviour {
	
	private float life = 3;
	private float maxLife = 3;
	private float regenAmount = 1;
	private float timeToRegen = 0;
	private float timeRoRegenValue = 5; // in seconds

	public GameObject blood;

	// Use this for initialization
	void Start () {
		InvokeRepeating("reduceTimer", 0f, 1f);
	}
	
	// Update is called once per frame
	void Update () {


		if(life >= maxLife){
			blood.GetComponent<CanvasGroup>().alpha = 0f;
		}
		else if(life < maxLife && life >1){
			blood.GetComponent<CanvasGroup>().alpha = 0.5f;
		}
		else if(life < 2 && life > 0){
			blood.GetComponent<CanvasGroup>().alpha = 0.8f;
		}
		else if(life <= 0){
			Debug.Log("you died");
		}

		if(life > maxLife){
			life = maxLife;
		}

		if (timeToRegen > timeRoRegenValue){
			timeToRegen = timeRoRegenValue;
		}
	}

	void OnCollisionEnter(Collision collision){
		life -= 1;
		timeToRegen += timeRoRegenValue;
		Destroy(collision.rigidbody.gameObject);
	}

	private void reduceTimer(){
		if(timeToRegen > 0){
			timeToRegen -= 1;
		}
		if(timeToRegen <= 0){
			life += regenAmount;
		}
	}

}


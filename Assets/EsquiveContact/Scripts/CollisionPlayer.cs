using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPlayer : MonoBehaviour {
	
	private int life = 3;

	public GameObject blood;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(life < 3 && life >1){
			blood.GetComponent<CanvasGroup>().alpha = 0.5f;
		}
		else if(life < 2 && life > 0){
			blood.GetComponent<CanvasGroup>().alpha = 0.8f;
		}
		else if(life <= 0){
			Debug.Log("you died");
		}
	}

	void OnCollisionEnter(Collision collision){
		life -= 1;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : MonoBehaviour {

	public Transform camera;	
	
	private Vector3 prevPos;
	

	// Use this for initialization
	void Start () {
		StartCoroutine(WaitCoroutine());
	}

	
	// Update is called once per frame
	void Update () {
		float diffPosY = Mathf.Abs(prevPos.y - camera.position.y);
		if(prevPos.y > camera.position.y){
			this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y-2.5f*diffPosY,this.transform.localScale.z);
		}
		if(prevPos.y < camera.position.y){

			this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y+2.5f*diffPosY,this.transform.localScale.z);
		}
		
		prevPos = camera.position;
	}

	private IEnumerator WaitCoroutine()
	{
		yield return new WaitForSeconds(.2f);
		prevPos = camera.position;
		this.transform.position = new Vector3(camera.position.x, camera.position.y -0.9f,camera.position.z);
		this.transform.localScale = new Vector3(this.transform.localScale.x, 1,this.transform.localScale.z);
		
	}
}

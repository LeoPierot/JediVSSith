using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour {


	public Transform camera;

	private Vector3 prevPos;
	

	// Use this for initialization
	void Start () {
		StartCoroutine(WaitCoroutine());
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.LookAt(camera);
		float diffPosY = Mathf.Abs(prevPos.y - camera.position.y);
		if(prevPos.y > camera.position.y){
				this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y - diffPosY,this.transform.position.z);
		}
		if(prevPos.y < camera.position.y){
				this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y + diffPosY,this.transform.position.z);
		}
		
		prevPos = camera.position;
	}

	private IEnumerator WaitCoroutine()
	{
		yield return new WaitForSeconds(.2f);
		prevPos = camera.position;
		this.transform.position = new Vector3(camera.position.x, camera.position.y-0.5f,camera.position.z);
	}
}

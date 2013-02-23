using UnityEngine;
using System.Collections;

public class WorldMovement : MonoBehaviour {
	
	/*** Control del movimiento ***/
	//private bool rotating = false;
	//private bool gravitating = false;
	
	
	/*** Velocidad Giro ***/
	public float speed = 5;
	
	/*** Rotacion escenario ***/
	private Quaternion nextRotation;
	
	
	/*** Rotacion gravedad ***/
	private Vector3 gravity;

	
	
	// Use this for initialization
	void Start () {
		nextRotation = transform.rotation;
		gravity = Physics.gravity;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			//rotating = true;
			nextRotation = transform.rotation*Quaternion.Euler(0,0,10);
			

		}
		if(Input.GetKeyDown(KeyCode.LeftControl)) {
			//gravitating = true;
			gravity = new Vector3(9.8f,9.8f,0);
			

		}
		
		rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, nextRotation, Time.deltaTime*speed));	
		Physics.gravity = Vector3.Slerp(Physics.gravity, gravity,Time.deltaTime*speed);
		
	}
	
	/*
	public bool isRotating() {
		return rotating;	
	}
	
	public bool isGravitating() {
		return gravitating;	
	}
	*/
	
	public void rotateToAngle(float angle) {
		//rotating = true;
		nextRotation = transform.rotation*Quaternion.Euler(0,0,angle);
	}
	
	public void gravitateToAngle(Vector3 newGravity) {
		//gravitating = true;
		gravity = newGravity;
	}
	
}

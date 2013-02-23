using UnityEngine;
using System.Collections;

public class WorldMovement : MonoBehaviour {
	
	/*** Control del movimiento ***/
	//private bool rotating = false;
	//private bool gravitating = false;
	
	
	/*** Velocidad Giro ***/
	public float speed = 5;
	
	/*** Rotacion escenario ***/
	private Quaternion newRotation;
	private float newAngle;
	public float grados = 35;
	private float sentido = 1;
	
	
	/*** Rotacion gravedad ***/
	private Vector3 gravity;

	
	
	// Use this for initialization
	void Start () {
		newRotation = transform.rotation;
		gravity = Physics.gravity;
	}
	
	// Update is called once per frame
	void Update () {
		/*if(Input.GetKeyDown(KeyCode.Space)) {
			//rotating = true;
			nextRotation = transform.rotation*Quaternion.Euler(0,0,10);
			

		}
		if(Input.GetKeyDown(KeyCode.LeftControl)) {
			//gravitating = true;
			gravity = new Vector3(9.8f,9.8f,0);
			

		}*/
		Debug.Log ("newAngle :"+newAngle+", "+transform.eulerAngles.z + " sense " + sentido);
		Quaternion nextRotation;
		if(Mathf.Abs(newAngle - transform.eulerAngles.z) < 2*grados*Time.deltaTime) {
			nextRotation = Quaternion.Euler(0,0,newAngle);

		} else {
			nextRotation = transform.rotation*Quaternion.Euler(0,0,grados*Time.deltaTime*sentido);
		}
		rigidbody.MoveRotation(nextRotation);	
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
		newAngle = (angle < 0)? angle + 360f : angle;
		float ang = newAngle - transform.eulerAngles.z;
		if(ang < 0) ang +=360;
		if(ang > 180) sentido = -1;
		else sentido = 1;
	}
	
	public void gravitateToAngle(Vector3 newGravity) {
		//gravitating = true;
		gravity = newGravity;
	}
	
}

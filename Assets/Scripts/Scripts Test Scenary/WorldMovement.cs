using UnityEngine;
using System.Collections;

public class WorldMovement : MonoBehaviour {
	
	/*** Control del movimiento ***/
	private bool rotating = false;
	private bool gravitating = false;
	
	private float rotateDegree = 0;
	private float gravitateDegree = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(rotating) {
		//	rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, nextRotation, Time.deltaTime*speed));
		}
		
		if(gravitating) {
		//	Physics.gravity = Quaternion.Euler(0,0,control.deltaPitch[player]*gravitySensitivity)*Physics.gravity;
		}
	}
	
	public bool isRotating() {
		return rotating;	
	}
	
	public bool isGravitating() {
		return gravitating;	
	}
	
	public void rotateToAngle(float angle) {
		rotating = true;
		rotateDegree = angle;
	}
	
	public void gravitateToAngle(float angle) {
		gravitating = true;
		gravitateDegree = angle;
	}
	
}

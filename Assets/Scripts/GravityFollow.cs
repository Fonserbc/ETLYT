using UnityEngine;
using System.Collections;
[RequireComponent (typeof (Rigidbody))]

public class GravityFollow : MonoBehaviour {
	
	public Transform wantedPosition;
	public float rotationSpeed = 4.0f;
	// Update is called once per frame
	void FixedUpdate () {
		rigidbody.MovePosition(wantedPosition.position);
		
		Vector3 wantedRot = -Physics.gravity.normalized;
		rigidbody.MoveRotation(Quaternion.Slerp(rigidbody.rotation, Quaternion.FromToRotation(Vector3.up, wantedRot), Time.deltaTime*rotationSpeed));
	}
}

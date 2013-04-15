using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
	
	public float rotatingSpeed = 5.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(Vector3.forward, rotatingSpeed*Time.deltaTime);
	}
}

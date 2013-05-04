using UnityEngine;
using System.Collections;

public class RotateCometa : MonoBehaviour {
	
	public float sentido = 1;
	public float speed = 5;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0,0,speed*Time.deltaTime*sentido);
	}
	

}

using UnityEngine;
using System.Collections;

public class RotateMoon : MonoBehaviour {
	
	private float sentido = 1;
	public float speed = 20;
	public float time = 0.8f;
	private float cont = 0;
	
	// Update is called once per frame
	void Update () {
		cont += Time.deltaTime;
		if(cont >= time) {
			cont = 0;
			sentido = sentido*-1;
		}			
		
		transform.Rotate(0,speed*Time.deltaTime*sentido,0);
	}
	

}

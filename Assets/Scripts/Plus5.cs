using UnityEngine;
using System.Collections;

public class Plus5 : MonoBehaviour {
	
	private float time = 1.5f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		time -= Time.deltaTime;
		if(time <= 0) Destroy(gameObject);
		transform.position -= transform.forward*Time.deltaTime*0.4f;
	}
}

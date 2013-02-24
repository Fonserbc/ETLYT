using UnityEngine;
using System.Collections;

public class SelectCharacter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider c) {
		if (c.gameObject.tag == "Player") {
			renderer.material.color = Color.red;
		}
	}
	
	void OnTriggerExit(Collider c) {
		if (c.gameObject.tag == "Player") {
			renderer.material.color = Color.white;
		}
	}
}

using UnityEngine;
using System.Collections;

public class MeteoCollisions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
		
	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Clock") {
			Destroy(collider.gameObject);
		}
		else if (collider.gameObject.tag == "PowerUp") {
			Destroy(collider.gameObject);
		}
	}
}

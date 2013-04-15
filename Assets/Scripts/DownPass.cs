using UnityEngine;
using System.Collections;

public class DownPass : MonoBehaviour {
	
	private Collider parentCol;
	
	// Use this for initialization
	void Start () {
		parentCol = transform.parent.collider;
	}
	
	void OnTriggerEnter (Collider col) {
		if (col.tag == "Player") {
			Physics.IgnoreCollision(col, parentCol, true);
		}
	}
	
	void OnTriggerExit (Collider col) {
		if (col.tag == "Player") {
			Physics.IgnoreCollision(col, parentCol, false);
		}
	}
}

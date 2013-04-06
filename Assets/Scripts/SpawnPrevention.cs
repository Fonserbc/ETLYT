using UnityEngine;
using System.Collections;

public class SpawnPrevention : MonoBehaviour {
	
	private int someoneThere;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
		
	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Player1") ++someoneThere;
	}
	
	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "Player1") --someoneThere;
	}
	
	public bool isSomeoneThere() {
		return (someoneThere > 0);
	}
}

using UnityEngine;
using System.Collections;

public class GravityClock : MonoBehaviour {
	
	private Vector3 originalPos;
	
	// Use this fF.activeor initialization
	void Start () {
		originalPos = transform.position - transform.parent.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.LookRotation(Physics.gravity);
		transform.position = transform.parent.position+originalPos+(transform.forward*2);
	}
}

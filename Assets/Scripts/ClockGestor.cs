using UnityEngine;
using System.Collections;

public class ClockGestor : MonoBehaviour {
	
	private GameObject spawner;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnDestroy () {
		SphereGizmos sG = spawner.GetComponent<SphereGizmos>();
		sG.is_instanced(false);
	}
	
	public void setSpawner(Transform s) {
		spawner = s.gameObject;
		//Debug.Log(s.position.x);
	}
}

using UnityEngine;
using System.Collections;

public class Lifebar : MonoBehaviour {
	
	private float life = 0;
	// Use this for initialization
	void Start () {
		life = 100;
	}
	
	// Update is called once per frame
	void Update() {
		life -= Time.deltaTime;
		Debug.Log(life);
		if (life <= 0) BroadcastMessage("Death");
	}
	
	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Clock") {
			life += 5;
			Destroy(collider.gameObject);
		}
	}
	
	public void gotHit() {
		life -= 10;
	}
}

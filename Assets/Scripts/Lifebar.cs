using UnityEngine;
using System.Collections;

public class Lifebar : MonoBehaviour {

	public Texture2D[] Numbers;
	private float life = 0;
	// Use this for initialization
	void Start () {
		life = 100;
	}
	
	void OnGUI() {
	//	GUI.Label(new Rect(10, 10, 180, 45), "Tiempo: " + life);
		int centenes = ((int)life%1000)/100;
		int desenes = ((int)life%100)/10;
		int unitats = (int)life%10;
		if (centenes == 0) centenes = -1;
		if (desenes == 0 && centenes == -1) desenes = -1;
		if (unitats == 0 && desenes == -1) unitats = -1;
		GUI.DrawTexture(new Rect(10, 10, 60, 60), Numbers[centenes+1]);
		GUI.DrawTexture(new Rect(33, 10, 60, 60), Numbers[desenes+1]);
		GUI.DrawTexture(new Rect(56, 10, 60, 60), Numbers[unitats+1]);
	/*	nB1.setNumber(centenes);
		nB2.setNumber(desenes);
		nB3.setNumber(unitats);	*/	
	}
	// Update is called once per frame
	void Update() {
		life -= Time.deltaTime;
		if (life <= 0) BroadcastMessage("Death");
		//Debug.Log(centenes.ToString() + desenes.ToString() + unitats.ToString());
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

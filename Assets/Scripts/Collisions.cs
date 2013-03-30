using UnityEngine;
using System.Collections;

public class Collisions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
		
	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Clock") {
			GameObject.FindGameObjectWithTag("Control").SendMessage("addLife", 5);
			Destroy(collider.gameObject);
		}
		else if (collider.gameObject.tag == "PowerUp") {
			SimpleIDHandler iDH = collider.gameObject.GetComponent<SimpleIDHandler>();
			PowerUpHandler pUH = GetComponent<PowerUpHandler>();
			Debug.Log (iDH.getID ());
			pUH.stackPowerUp(iDH.getID());
			Destroy(collider.gameObject);
		}
	}
}

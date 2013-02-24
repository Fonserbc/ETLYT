using UnityEngine;
using System.Collections;

public class MenuCharacterControl : MonoBehaviour {
	

	public Rigidbody selector;
	private MenuMovement mov;
	
	private Rigidbody[] players;

	// Use this for initialization
	void Start () {
		Control control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		int c = WiiMoteControl.wiimote_count();
		if (c>0) {
			players = new Rigidbody[c];
			for (int i=0; i<=c-1; i++) {
				Rigidbody sel;
				sel = (Rigidbody) Instantiate(selector, new Vector3(0,0,-2), transform.rotation);
				players[i] = sel;
				control.RegisterPlayer(Control.ControllerType.WiiMote, i);
				mov = sel.GetComponent<MenuMovement>();
				mov.setPlayer(i);
			}		
		} else {
			Debug.Log ("No hay mandos, tolai!");		
		}
	}
	

}

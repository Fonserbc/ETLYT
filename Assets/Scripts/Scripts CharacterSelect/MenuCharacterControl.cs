using UnityEngine;
using System.Collections;

public class MenuCharacterControl : MonoBehaviour {
	

	public GameObject standardAvatar;
	
	// Use this for initialization
	void Start () {
		Physics.gravity = new Vector3(0,-9.81f,0);
		Time.timeScale = 1.0f;
		Control control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		BattleInformer bi = GameObject.FindGameObjectWithTag("BattleInformer").GetComponent<BattleInformer>();

		int c = WiiMoteControl.wiimote_count();
		if (c>0) {
			for (int i=0; i<=c-1; i++) {
				
				//control.RegisterPlayer(Control.ControllerType.WiiMote, i);
				bi.changePlayer(standardAvatar,i, 0);
			}		
		} else {
			Debug.Log ("No hay mandos, tolai!");		
		}
	}
	

}

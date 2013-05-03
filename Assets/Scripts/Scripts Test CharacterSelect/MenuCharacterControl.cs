using UnityEngine;
using System.Collections;

public class MenuCharacterControl : MonoBehaviour {
	

	public GameObject standardAvatar;
	
	// Use this for initialization
	void Start () {
		Control control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		BattleInformer bi = GameObject.FindGameObjectWithTag("BattleInformer").GetComponent<BattleInformer>();

		int c = WiiMoteControl.wiimote_count();
		if (c>0) {
			for (int i=0; i<=c-1; i++) {
				bi.changePlayer(standardAvatar,i);
				control.RegisterPlayer(Control.ControllerType.WiiMote, i);

			}		
		} else {
			Debug.Log ("No hay mandos, tolai!");		
		}
	}
	

}

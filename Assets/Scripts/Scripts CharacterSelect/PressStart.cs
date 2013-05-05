using UnityEngine;
using System.Collections;

public class PressStart : MonoBehaviour {
	
	private int count;
	private BattleInformer bi;
	
	public GameObject baseChar;
	
	// Use this for initialization
	void Start () {
		Control control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		count = WiiMoteControl.wiimote_count();

		bi = GameObject.FindGameObjectWithTag("BattleInformer").GetComponent<BattleInformer>();
	
	}
	
	// Update is called once per frame
	void Update () {
		int numP = bi.getNumPlayers(baseChar);
		if(numP == count) {
			gameObject.renderer.enabled = true;
			bi.setStart(true);
		} else {
			gameObject.renderer.enabled = false;
			bi.setStart(false);
		}
	}
}

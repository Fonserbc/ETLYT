using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
	
	public int players = 0;
	
	private int[] playerControllerId;
	
	private WiiMoteControl wiiControl;

	// Use this for initialization
	void Start () {
		wiiControl = (WiiMoteControl)GetComponent("WiiMoteControl");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/*
	 * Returns a value between -1 and 1
	 */
	float VerticalAxis (int player) {
		// TODO
		return 0;
	}
	
	/*
	 * Returns a value between -1 and 1
	 */
	float HorizontalAxis (int player) {
		// TODO
		return 0;
	}
	
	bool Jump (int player) {
		// TODO
		return false;
	}
	
	bool Attack (int player) {
		// TODO
		return false;
	}
	
	bool AbilityWorld (int player) {
		// TODO
		return false;
	}
	
	bool AbilityGravity (int player) {
		// TODO
		return false;
	}
	
	bool AbilityPowerUp (int player) {
		// TODO
		return false;
	}
	
	bool AbilitySkill (int player) {
		// TODO
		return false;
	}
	
	/*
	 * Returns a value between -180 and 180
	 * 
	 * 0 represents the static position
	 */
	float Slope (int player) {
		// TODO
		return 0;
	}
	
	bool Pause (int player) {
		// TODO
		return false;
	}
	
}

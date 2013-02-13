using UnityEngine;
using System.Collections;

/*
 * @author Fonserbc
 */
public class Control : MonoBehaviour {
	
	public enum ControllerType {
		Undefined,
		WiiMote,
		Controller,
		Keyboard
	};
	
	public int players = 0;
	
	private ControllerType[] types;
	private int[] playerControllerId;
	
	private WiiMoteControl wiiControl;

	// Use this for initialization
	void Start () {
		wiiControl = (WiiMoteControl)GetComponent("WiiMoteControl");
		
		types = new ControllerType[4];
		for (int i = 0; i < 4; ++i)
			types[i] = ControllerType.Undefined;
		
		playerControllerId = new int[4];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	int RegisterPlayer (ControllerType type, int id) {
		if (players < 4) {
			types[players] = type;
			
			//TODO diferent types of controller
			switch (type) {
			case ControllerType.WiiMote:
				playerControllerId[players] = id;
				break;
			default:
				break;
			}
			
			return players++;
		}
		else Debug.LogError("4 Players cap reached");
		
		return -1;
	}
	
	ControllerType GetType (int player) {
		if (player < players) {
			return types[player];
		}
		else {
			Debug.LogError("Player id does not exist. id "+player);
			return ControllerType.Undefined;
		}
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
		if (player < players) {
			switch (types[player]) {
			case ControllerType.WiiMote:
				if (WiiMoteControl.wiimote_isExpansionPortEnabled(playerControllerId[player])) {
					return WiiMoteControl.wiimote_getButtonA(playerControllerId[player]);
				}
				else {
					return WiiMoteControl.wiimote_getButton2(playerControllerId[player]);
				}
			default:
				break;
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
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

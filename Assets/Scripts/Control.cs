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
	
	public int RegisterPlayer (ControllerType type, int id) {
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
	
	public ControllerType GetType (int player) {
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
	public float VerticalAxis (int player) {
		if (player < players) {
			switch (types[player]) {
			case ControllerType.WiiMote:
				if (WiiMoteControl.wiimote_isExpansionPortEnabled(playerControllerId[player])) {
					
				}
				else {
					
				}
				break;
			default:
				break;
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
		return 0;
	}
	
	/*
	 * Returns a value between -1 and 1
	 */
	public float HorizontalAxis (int player) {
		// TODO
		return 0;
	}
	
	public bool Jump (int player) {
		if (player < players) {
			switch (types[player]) {
			case ControllerType.WiiMote:
				if (WiiMoteControl.wiimote_isExpansionPortEnabled(playerControllerId[player])) {
					return WiiMoteControl.wiimote_getButtonA(playerControllerId[player]);
				}
				else {
					return WiiMoteControl.wiimote_getButton2(playerControllerId[player]);
				}
				break;
			default:
				break;
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
		return false;
	}
	
	public bool Attack (int player) {
		if (player < players) {
			switch (types[player]) {
			case ControllerType.WiiMote:
				if (WiiMoteControl.wiimote_isExpansionPortEnabled(playerControllerId[player])) {
					return WiiMoteControl.wiimote_getButtonB(playerControllerId[player]);
				}
				else {
					return WiiMoteControl.wiimote_getButton1(playerControllerId[player]);
				}
				break;
			default:
				break;
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
		return false;
	}
	
	public bool AbilityWorld (int player) {
		if (player < players) {
			switch (types[player]) {
			case ControllerType.WiiMote:
				if (WiiMoteControl.wiimote_isExpansionPortEnabled(playerControllerId[player])) {
					return WiiMoteControl.wiimote_getButtonPlus(playerControllerId[player]);
				}
				else {
					return WiiMoteControl.wiimote_getButtonA(playerControllerId[player]);
				}
				break;
			default:
				break;
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
		return false;
	}
	
	public bool AbilityGravity (int player) {
		if (player < players) {
			switch (types[player]) {
			case ControllerType.WiiMote:
				if (WiiMoteControl.wiimote_isExpansionPortEnabled(playerControllerId[player])) {
					return WiiMoteControl.wiimote_getButtonMinus(playerControllerId[player]);
				}
				else {
					return WiiMoteControl.wiimote_getButtonB(playerControllerId[player]);
				}
				break;
			default:
				break;
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
		return false;
	}
	
	public bool AbilityPowerUp (int player) {
		if (player < players) {
			switch (types[player]) {
			case ControllerType.WiiMote:
				if (WiiMoteControl.wiimote_isExpansionPortEnabled(playerControllerId[player])) {
					return WiiMoteControl.wiimote_getButtonNunchuckZ(playerControllerId[player]);
				}
				else {
					return WiiMoteControl.wiimote_getButtonMinus(playerControllerId[player]);
				}
				break;
			default:
				break;
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
		return false;
	}
	
	public bool AbilitySkill (int player) {
		// TODO
		return false;
	}
	
	/*
	 * Returns a value between -180 and 180
	 * 
	 * 0 represents the static position
	 */
	public float Slope (int player) {
		// TODO
		return 0;
	}
	
	public bool Pause (int player) {
		if (player < players) {
			switch (types[player]) {
			case ControllerType.WiiMote:
				if (WiiMoteControl.wiimote_isExpansionPortEnabled(playerControllerId[player])) {
					return WiiMoteControl.wiimote_getButton1(playerControllerId[player]);
				}
				else {
					return WiiMoteControl.wiimote_getButtonPlus(playerControllerId[player]);
				}
				break;
			default:
				break;
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
		return false;
	}
	
}

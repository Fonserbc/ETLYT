using UnityEngine;
using System.Collections;

/*
 * @author Fonserbc
 */
public class Control : MonoBehaviour {
	
	private const float AXIS_SPEED = 5.0f;
	private const int NUNCHUCK_MARGIN = 30;
	private const int NUNCHUCK_RANGE = 256-2*NUNCHUCK_MARGIN;
	
	
	public enum ControllerType {
		Undefined,
		WiiMote,
		Controller,
		Keyboard
	};
	
	private struct axis {
		public float h;
		public float v;
		
		public void init() {
			h = v = 0.0f;
		}
	};
	
	
	public int players = 0;
	
	private ControllerType[] types;
	private int[] playerControllerId;
	private axis[] playerAxis;
	
	private WiiMoteControl wiiControl;

	// Use this for initialization
	void Awake () {
		wiiControl = (WiiMoteControl)GetComponent("WiiMoteControl");
		
		types = new ControllerType[4];
		for (int i = 0; i < 4; ++i)
			types[i] = ControllerType.Undefined;
		
		playerControllerId = new int[4];
		playerAxis = new axis[4];
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 4; ++i) {
			switch (types[i]) {
				case ControllerType.WiiMote:
					int id = playerControllerId[i];
					//Case not nunchuck
					if (!WiiMoteControl.wiimote_isExpansionPortEnabled(id)) {
						if (WiiMoteControl.wiimote_getButtonRight(id)) { // UP
							if (playerAxis[i].v < 0.0f) {
								playerAxis[i].v = 0.0f;
							}
							playerAxis[i].v = Mathf.Lerp(playerAxis[i].v, 1.0f, AXIS_SPEED*Time.deltaTime);
						}
						else if (WiiMoteControl.wiimote_getButtonLeft(id)) { // DOWN
							if (playerAxis[i].v > 0.0f) {
								playerAxis[i].v = 0.0f;
							}
							playerAxis[i].v = Mathf.Lerp(playerAxis[i].v, -1.0f, AXIS_SPEED*Time.deltaTime);
						}
						else {
							playerAxis[i].v = Mathf.Lerp(playerAxis[i].v, 0f, AXIS_SPEED*Time.deltaTime);
						}
						
						if (WiiMoteControl.wiimote_getButtonDown(id)) { // RIGHT
							if (playerAxis[i].h < 0.0f) {
								playerAxis[i].h = 0.0f;
							}
							playerAxis[i].h = Mathf.Lerp(playerAxis[i].h, 1.0f, AXIS_SPEED*Time.deltaTime);
						}
						else if (WiiMoteControl.wiimote_getButtonUp(id)) { // LEFT
							if (playerAxis[i].h > 0.0f) {
								playerAxis[i].h = 0.0f;
							}
							playerAxis[i].h = Mathf.Lerp(playerAxis[i].h, -1.0f, AXIS_SPEED*Time.deltaTime);
						}
						else {
							playerAxis[i].h = Mathf.Lerp(playerAxis[i].h, 0, AXIS_SPEED*Time.deltaTime);
						}
					}
					break;
				default:
					break;
			}
		}
	}
	
	public int RegisterPlayer (ControllerType type, int id) {
		if (players < 4) {
			types[players] = type;
			
			//TODO diferent types of controller
			switch (type) {
				case ControllerType.WiiMote:
					playerControllerId[players] = id;
					
					if (WiiMoteControl.wiimote_isExpansionPortEnabled(id))
						playerAxis[players].init();
					
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
	 * DOWN <= -1 .. 1 <= UP
	 */
	public float VerticalAxis (int player) {
		if (player < players) {
			switch (types[player]) {
				case ControllerType.WiiMote:
					if (WiiMoteControl.wiimote_isExpansionPortEnabled(playerControllerId[player])) {
						return Mathf.Clamp(((float)(WiiMoteControl.wiimote_getNunchuckStickY(playerControllerId[player])-NUNCHUCK_MARGIN-NUNCHUCK_RANGE/2))
								/ ((float)(NUNCHUCK_RANGE)/2f), -1f, 1f);
					}
					else {
						return playerAxis[player].v;
					}
				
				case ControllerType.Keyboard:
					return Input.GetAxis("Vertical");
				
				default:
					break;
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
		return 0;
	}
	
	/*
	 * Returns a value between -1 and 1
	 * LEFT <= -1 .. 1 <= RIGHT
	 */
	public float HorizontalAxis (int player) {
		if (player < players) {
			switch (types[player]) {
				case ControllerType.WiiMote:
					if (WiiMoteControl.wiimote_isExpansionPortEnabled(playerControllerId[player])) {
						return Mathf.Clamp(((float)(WiiMoteControl.wiimote_getNunchuckStickX(playerControllerId[player])-NUNCHUCK_MARGIN-NUNCHUCK_RANGE/2))
								/ ((float)(NUNCHUCK_RANGE)/2f), -1f, 1f);
					}
					else {
						return playerAxis[player].h;
					}
				
				case ControllerType.Keyboard:
					return -Input.GetAxis("Horizontal");
				
				default:
					break;
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
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
				
				case ControllerType.Keyboard:
					return Input.GetKeyDown(KeyCode.Space);
				
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
				
				case ControllerType.Keyboard:
					return Input.GetButtonDown("Fire1");
				
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
				
				case ControllerType.Keyboard:
					return Input.GetKeyDown(KeyCode.Q);
				
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
				
				case ControllerType.Keyboard:
					return Input.GetKeyDown(KeyCode.E);
					
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
				
				case ControllerType.Keyboard:
					return Input.GetButtonDown("Fire2");
					
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
		if (player < players) {
			switch (types[player]) {
				case ControllerType.WiiMote:
					if (WiiMoteControl.wiimote_isExpansionPortEnabled(playerControllerId[player])) {
						return (float)-WiiMoteControl.wiimote_getRoll(playerControllerId[player]);
					}
					else {
						return (float)(WiiMoteControl.wiimote_getPitch(playerControllerId[player]));
					}
				
				case ControllerType.Keyboard:
					Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical"), 0);
					dir.Normalize();
					return ((Vector3.Cross(Vector3.up, dir).z < 0)? 1f : -1f)*Vector3.Angle(Vector3.up, dir);
					
				default:
					break;
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
		return 0f;
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
				
				case ControllerType.Keyboard:
					return Input.GetKeyDown(KeyCode.P);
					
				default:
					break;
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
		return false;
	}
	
}

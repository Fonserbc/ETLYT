using UnityEngine;
using System.Collections;

/*
 * @author Fonserbc
 */
public class Control : MonoBehaviour {
	
	private const float AXIS_SPEED = 5.0f;
	private const int NUNCHUCK_MARGIN = 30;
	private const int NUNCHUCK_RANGE = 256-2*NUNCHUCK_MARGIN;
	private const float PRESSED_TIME = 0.2f;
	
	
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
	
	private float[,] pressedTime;
	private bool[,] lastPressed;
	
	private WiiMoteControl wiiControl;
	

	public int[] automaticRegisterId;
	public GameObject[] automaticRegisterPlayer;
	public ControllerType[] automaticRegisterType;
	
	private float[] actualSlope;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(transform.gameObject);
		wiiControl = (WiiMoteControl)GetComponent("WiiMoteControl");
		
		types = new ControllerType[4];
		
		playerControllerId = new int[4];
		playerAxis = new axis[4];
		pressedTime = new float[7,4];
		lastPressed = new bool[7,4];
		actualSlope = new float[4];
		
		for (int i = 0; i < 4; ++i) {
			types[i] = ControllerType.Undefined;
			for (int j = 0; j < 7; ++j) {
				pressedTime[j,i] = -1000;
				lastPressed[j,i] = false;
			}
			actualSlope[i] = 0;
		}
		
		if (automaticRegisterId.Length == automaticRegisterPlayer.Length && automaticRegisterType.Length == automaticRegisterId.Length) {
			for (int i = 0; i < automaticRegisterId.Length; ++i) {
				int p = RegisterPlayer(automaticRegisterType[i], automaticRegisterId[i]);
				automaticRegisterPlayer[i].BroadcastMessage("SetPlayer", p);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 4; ++i) {
			
			if (types[i] == ControllerType.Undefined) continue;
			
			switch (types[i]) {				
				case ControllerType.WiiMote:
					int id = playerControllerId[i];
					
					for (int j = 0; j < 7; ++j) {
						bool button = false;
						switch (j) {
							case 0:
								button = JumpButton(i); break;
							case 1:
								button = AttackButton(i); break;
							case 2:
								button = AbilityWorldButton(i); break;
							case 3:
								button = AbilityGravityButton(i); break;
							case 4:
								button = AbilityPowerUpButton(i); break;
							case 5:
								button = AbilitySkillButton(i); break;
							case 6:
								button = PauseButton(i); break;
						}
						if (button && !lastPressed[j,i]) {
							pressedTime[j,i] = Time.time;
						}
						lastPressed[j,i] = button;
					}
					//AXIS
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
							playerAxis[i].v = Mathf.Lerp(playerAxis[i].v, 0f, AXIS_SPEED*2f*Time.deltaTime);
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
							playerAxis[i].h = Mathf.Lerp(playerAxis[i].h, 0, AXIS_SPEED*2f*Time.deltaTime);
						}
					}
					break;
				default:
					break;
			}
			
			actualSlope[i] = SlopeButton(i);
			
		}
	}
	
	public bool isNunchuckEnabled(int player) {
		return WiiMoteControl.wiimote_isExpansionPortEnabled(playerControllerId[player]);
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
					return Input.GetAxis("Horizontal");
				
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
				return (Time.time - pressedTime[0,player]) < PRESSED_TIME;
			case ControllerType.Keyboard:
				return Input.GetKeyDown(KeyCode.Space);
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
		return false;
	}
	
	public bool JumpButton (int player) {
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
				return (Time.time - pressedTime[1,player]) < PRESSED_TIME;
			case ControllerType.Keyboard:
				return Input.GetButtonDown("Fire1");
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
		return false;
	}
	
	public bool AttackButton (int player) {
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
				return (Time.time - pressedTime[2,player]) < PRESSED_TIME;
			case ControllerType.Keyboard:
				return Input.GetKeyDown(KeyCode.Q);
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
		return false;
	}
	
	public bool AbilityWorldButton (int player) {
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
				return (Time.time - pressedTime[3,player]) < PRESSED_TIME;
			case ControllerType.Keyboard:
				return Input.GetKeyDown(KeyCode.E);
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
		return false;
	}
	
	public bool AbilityGravityButton (int player) {
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
				return (Time.time - pressedTime[4,player]) < PRESSED_TIME;
			case ControllerType.Keyboard:
				return Input.GetButtonDown("Fire2");
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
		return false;
	}
	
	public bool AbilityPowerUpButton (int player) {
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
	
	public bool AbilitySkillButton (int player) {
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
			return actualSlope[player];
		} else Debug.LogError("Player id does not exist. id "+player);
		
		return 0f;
	}
	
	public float SlopeButton (int player) {
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
				return (Time.time - pressedTime[6,player]) < PRESSED_TIME;
			case ControllerType.Keyboard:
				return Input.GetKeyDown(KeyCode.P);
			}
		}
		else Debug.LogError("Player id does not exist. id "+player);
		
		return false;
	}
	
	public bool PauseButton (int player) {
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

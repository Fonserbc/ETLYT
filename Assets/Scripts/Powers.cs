using UnityEngine;
using System.Collections;

public class Powers : MonoBehaviour {
	
	private int player;
	
	private bool rotating = false;
	private bool gravitating = false;
	
	public float gravitySensitivity = 0.1f;
	public float rotationSensitivity = 0.5f;
	
	public float slowMotionFactor = 5f;
	private float slowTimeScale;
	
	private GameObject rot;
	
	private WiiMoteControl control;
	
	private GameObject otherPlayer;
	private Powers otherPower;
	private GameObject world;
	//private GameObject camara;
	
	void Start () {
		control = (WiiMoteControl)(GameObject.FindGameObjectWithTag("Control").GetComponent("WiiMoteControl"));
		world = GameObject.FindGameObjectWithTag("World");
		//camara = GameObject.FindGameObjectWithTag("MainCamera");
		player = int.Parse(gameObject.tag) - 1;
		
		otherPlayer = GameObject.FindGameObjectWithTag(((int)((player+1)%2) + 1).ToString());
		otherPower = (Powers)(otherPlayer.GetComponent("Powers"));
		
		slowTimeScale = Time.timeScale/slowMotionFactor;
	}
	
	// Update is called once per frame
	void Update () {
		if (WiiMoteControl.wiimote_count() > player) {
			if (!otherPower.UsingPowers() && !rotating && !gravitating) {
				if (WiiMoteControl.wiimote_getButtonA(player)) {
					rotating = true;
					Time.timeScale = slowTimeScale;
				}
				else if (WiiMoteControl.wiimote_getButtonB(player)) {
					gravitating = true;
					Time.timeScale = slowTimeScale;
				}
			}
			else if (gravitating) {
				if (!WiiMoteControl.wiimote_getButtonB(player)) {
					gravitating = false;
					Time.timeScale = 1f;
				}
				else {
					Physics.gravity = Quaternion.Euler(0,0,control.deltaPitch[player]*gravitySensitivity)*Physics.gravity;
				}
			}
		}
	}
	
	void FixedUpdate() {
		if (WiiMoteControl.wiimote_count() > player) {
			if (rotating) {
				if (!WiiMoteControl.wiimote_getButtonA(player)) {
					rotating = false;
					Time.timeScale = 1f;
					 
					GameObject.Destroy(rot);
					world.transform.parent = null;
					otherPlayer.transform.parent = null;
				}
				else {
					if (rot == null) {
						rot = new GameObject();
						rot.AddComponent("Rigidbody");
						rot.transform.position = Vector3.zero;
						rot.tag = "World";
						world.transform.parent = rot.transform;
						//otherPlayer.transform.parent = rot.transform;
						
						rot.rigidbody.isKinematic = true;
						rot.rigidbody.useGravity = false;
						rot.rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
					}
					
					//rot.transform.Rotate(0,0,control.deltaPitch[player]*sensitivity);
					rot.rigidbody.MoveRotation(rot.rigidbody.rotation*Quaternion.Euler(0,0,control.deltaPitch[player]*rotationSensitivity));
					//camara.transform.rotation = Quaternion.Slerp(camara.transform.rotation, camara.rigidbody.rotation*Quaternion.Euler(0,0,control.deltaPitch[player]*rotationSensitivity), Time.deltaTime*slowMotionFactor);
				}
			}
		}
	}
	
	public bool UsingPowers() {
		return rotating || gravitating;
	}
	
	public bool Rotating() {
		return rotating;
	}
	
	public bool Gravitating() {
		return gravitating;
	}
	
	void OnDrawGizmos () {
		Gizmos.color = Color.red;
		if (control != null) Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0,0,control.pitch[player])*(-Vector3.up*2));
	}
}

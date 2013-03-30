using UnityEngine;
using System.Collections;

public class ControlAssignTest : MonoBehaviour {
	
	public GameObject[] players;
	public Control.ControllerType[] playerControls;
	public int[] playerControllerIds;
	private Control control;
	
	// Use this for initialization
	void Start () {
		control = GetComponent<Control>();
		
		for (int i = 0; i < players.Length; ++i) {
			players[i].BroadcastMessage("SetPlayer", control.RegisterPlayer(playerControls[i], playerControllerIds[i]));
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

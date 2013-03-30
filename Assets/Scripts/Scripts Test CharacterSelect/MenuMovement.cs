using UnityEngine;
using System.Collections;

public class MenuMovement : MonoBehaviour {
	
	private Control control;
	private Transform camara;
	private int player;
	public float acceleration = 5f;
	private GameObject selection = null;
	private GameObject selected = null;
	
	public void setPlayer(int p) {
		player = p;	
	}
	
	// Use this for initialization
	void Start () {
		camara = GameObject.FindGameObjectWithTag("MainCamera").transform;
		control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();

	}
	
	// Update is called once per frame
	void Update () {			
		
		/*if(control.Pause(player)) {
			Application.LoadLevel("Testing Movement");	
		}*/
		
		if(selection) {
			if(control.Attack(player)) {
				CharacterSelect Cs;				
				Cs = selection.GetComponent<CharacterSelect>();
				Cs.setSelected(player);	
				
				
				
			}
		}
	
	}
	
	void OnTriggerEnter(Collider c) {
		if (c.gameObject.tag == "Character") {
			selection = c.gameObject;
			c.renderer.material.color = Color.red;
		}
	}
	
	void OnTriggerExit(Collider c) {
		if (c.gameObject.tag == "Character") {
			selection = null;
			CharacterSelect Cs = c.GetComponent<CharacterSelect>();
			Cs.setColor();
		}
	}
}

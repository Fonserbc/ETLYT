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
		rigidbody.MovePosition(transform.position+control.HorizontalAxis(player)*camara.right*acceleration*Time.deltaTime+control.VerticalAxis(player)*camara.up*acceleration*Time.deltaTime);
		
		if(control.Pause(player)) {
			Application.LoadLevel("Scene Demo");	
		}
		
		if(selection) {
			if(control.Jump(player)) {
				CharacterSelect Cs;
				Cs = selection.GetComponent<CharacterSelect>();
				if(!Cs.isSelected()) {
					if(selected) {
						selected.renderer.material.color = Color.white;
						Cs = selected.GetComponent<CharacterSelect>();
						Cs.setSelected(false);
					}
					selected = selection;
					selected.renderer.material.color = Color.blue;
					Cs = selected.GetComponent<CharacterSelect>();
					Cs.setSelected(true);	
				}
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

			if(!Cs.isSelected()) c.renderer.material.color = Color.white;
			else c.renderer.material.color = Color.blue;
		}
	}
}

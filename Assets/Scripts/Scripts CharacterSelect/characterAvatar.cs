using UnityEngine;
using System.Collections;

public class characterAvatar : MonoBehaviour {
	
	public GameObject character; //tenemos guardado un avatar en cocreto
	public int idPlayer;
	public AudioSource selection;
	public GameObject text;
	
	public GameObject getCharacter() {
		selection.Play();
		return character;	
	}
	
	public int getIdPlayer() {
		return idPlayer;	
	}
	
	void OnTriggerStay(Collider c) {
		text.renderer.enabled = true;
	}
	
	void OnTriggerEnter(Collider c) {
		text.renderer.enabled = true;
	}
	
	void OnTriggerExit(Collider c) {
		text.renderer.enabled = false;
	}
}

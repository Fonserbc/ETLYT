using UnityEngine;
using System.Collections;

public class characterAvatar : MonoBehaviour {
	
	public GameObject character; //tenemos guardado un avatar en cocreto
	public int idPlayer;
	public AudioSource selection;
	
	public GameObject getCharacter() {
		selection.Play();
		return character;	
	}
	
	public int getIdPlayer() {
		return idPlayer;	
	}
}

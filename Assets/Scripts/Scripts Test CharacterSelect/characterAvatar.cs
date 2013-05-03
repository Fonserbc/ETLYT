using UnityEngine;
using System.Collections;

public class characterAvatar : MonoBehaviour {
	
	public GameObject character; //tenemos guardado un avatar en cocreto
	public int idPlayer;
	
	public GameObject getCharacter() {
		return character;
	}
	
	public int getIdPlayer() {
		return idPlayer;	
	}
}

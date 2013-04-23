using UnityEngine;
using System.Collections;

public class itemAvatar : MonoBehaviour {

	public int item; //tenemos guardado la id de la scena de la stage
	
	private bool active = true;
	
	public int getItem() {
		return item;
	}
	
	public bool changeActive() {
		active = !active;
		if(active) renderer.material.color = Color.white;
		else renderer.material.color = Color.red;
		return active;
	}
}

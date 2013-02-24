using UnityEngine;
using System.Collections;

public class CharacterSelect : MonoBehaviour {
	
	private bool selected = false;

	
	
	public void setSelected(bool s) {
		selected = s;	
	}
	
	public bool isSelected() {
		return selected;	
	}
}

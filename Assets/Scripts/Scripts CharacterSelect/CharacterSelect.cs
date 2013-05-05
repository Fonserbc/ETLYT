using UnityEngine;
using System.Collections;

public class CharacterSelect : MonoBehaviour {
	
	public GameObject player;
	private Color mat;
	
	void Start() {
		mat = renderer.material.color;	
	}

	
	
	public void setSelected(int i) {
		OldBattleInformer b = GameObject.FindGameObjectWithTag("Control").GetComponent<OldBattleInformer>();
		b.setPlayer(player,i);
	}
	
	public void setColor() {
		renderer.material.color = mat;	
	}

}

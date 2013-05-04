using UnityEngine;
using System.Collections;

public class Scenary : MonoBehaviour {
	
	public Vector3[] initPosition;
	
	// Use this for initialization
	void Start () {
		BattleInformer bi = GameObject.FindGameObjectWithTag("BattleInformer").GetComponent<BattleInformer>();
		bi.initFight(initPosition);
	}
	

}

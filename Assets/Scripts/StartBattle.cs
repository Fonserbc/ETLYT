using UnityEngine;
using System.Collections;

public class StartBattle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		BattleInformer Bi = GameObject.FindGameObjectWithTag("Control").GetComponent<BattleInformer>();
		Bi.startBattle();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

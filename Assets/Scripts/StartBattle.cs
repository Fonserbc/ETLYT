using UnityEngine;
using System.Collections;

public class StartBattle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		OldBattleInformer Bi = GameObject.FindGameObjectWithTag("Control").GetComponent<OldBattleInformer>();
		Bi.startBattle();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

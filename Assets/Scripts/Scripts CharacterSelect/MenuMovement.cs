using UnityEngine;
using System.Collections;

public class MenuMovement : MonoBehaviour {
	
	private Control control;
	private int player;
	private GameObject selection = null;
	private BattleInformer bi; //accederemos bastante a battleinformer
	
	private string selected;
	
	private int idPlayer;

	
	
	public void setPlayer(int p) {
		player = p;	
	}
	
	public void setIdPlayer(int id) {
		idPlayer = id;	
	}
	
	// Use this for initialization
	void Start () {
		control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		bi = GameObject.FindGameObjectWithTag("BattleInformer").GetComponent<BattleInformer>();

	}
	
	// Update is called once per frame
	void Update () {			
		
		if(control.Attack(player)) {
			if(selection){
				if(selected == "CharacterAvatar") {
					characterAvatar ca = selection.GetComponent<characterAvatar>();
					int idp = ca.getIdPlayer();
					if(idPlayer != idp) {
						GameObject character = ca.getCharacter();
						bi.changePlayer(character, player, idp);
					}
				} else if(selected == "StageAvatar") {
					stageAvatar sa = selection.GetComponent<stageAvatar>();
					int stage = sa.getStage();
					bi.changeStage(stage);
				} else if(selected == "ItemAvatar") {
					itemAvatar ia = selection.GetComponent<itemAvatar>();
					int item = ia.getItem();
					bool activate = ia.changeActive();
					bi.changeItem(item,activate);
				}
			}
		}
		
		if(control.Pause(player)) {
			bi.startFight();
		}

	
	}
	
	void OnTriggerEnter(Collider c) {
		if (c.gameObject.tag == "CharacterAvatar" || c.gameObject.tag == "StageAvatar" || c.gameObject.tag == "ItemAvatar" ) {
			selection = c.gameObject;
			selected = c.gameObject.tag;
		}
	}
	
	void OnTriggerExit(Collider c) {
		if (c.gameObject.tag == "CharacterAvatar" || c.gameObject.tag == "StageAvatar" || c.gameObject.tag == "ItemAvatar" ) {
			selection = null;
			selected = "";
		}
	}
}

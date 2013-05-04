using UnityEngine;
using System.Collections;

public class BattleInformer : MonoBehaviour {
	
	public int maxPlayers = 4; //numero maximo de pnjs en juego
	public Vector3 standardPosition = Vector3.zero; //Posicion standard para colocar pnjs
	private GameObject[] players; //personajes en juego
	private int stage = 1; //Pantalla
	public bool[] items;
	
	//datos que se traspasan
	private GameObject[] playersType;
	private Vector3[] scenary;

	
	void Start() {
		DontDestroyOnLoad(transform.gameObject);
		players = new GameObject[maxPlayers];
		playersType = new GameObject[maxPlayers];

	}
	
	public void startFight() {
		if(stage != -1) {
			Application.LoadLevel(stage);
		}
	}
	
	public void initFight(Vector3[] scn) {
		int size = playersType.Length;
		float delta = 1f/((float)playersType.Length + 1f);
		float initDelta = -1f + delta;
		for(int i = 0; i < size; ++i) {
			if(playersType[i] != null) {
				players[i] = Instantiate(playersType[i],scn[i] + new Vector3(0f,0f,initDelta), playersType[i].transform.rotation) as GameObject; 
				players[i].GetComponent<MenuMovement>().enabled = false;
				players[i].GetComponent<Movement>().enabled = true;
				players[i].GetComponent<BasicPowers>().enabled = true;
				//players[i].GetComponent<PoweUpHandler>().enabled = true;
				
				players[i].BroadcastMessage("SetPlayer", i);
				
				initDelta += delta;
			}
		}	
	}
	
	//El item i pasa a estado b
	public void changeItem(int i, bool b) {
	//Accede itemSwitch
		items[i] = b;
	}
	
	//Se cambia el escenario
	public void changeStage(int s) {
	//Accede stageSelect
		stage = s;	
	}
	
	//Se cambia el personaje
	public void changePlayer(GameObject playerType, int i, int idPlayer) {
	/* Acceden controllerActivate y characterAvatar
	 * i debe ser entre 1 y maxPlayers
	 *Si playerType = null simplemente destruye un jugador 
	 *Sino: Si jugador = null instancia playerType en standardPosition
	 *		Sino: instancia playerType en posicion de jugador
	 */
		if(i <= maxPlayers)	{

			Vector3 position = standardPosition;
			Vector3 velocity = Vector3.zero;
			if(players[i] != null) {
				position = players[i].transform.position;
				velocity = players[i].rigidbody.velocity;
				Destroy (players[i]);
				playersType[i] = null;
			}
			
			//Instantiate particles			
			if(playerType != null) { 
				players[i] = Instantiate(playerType,position, playerType.transform.rotation) as GameObject; 
				players[i].rigidbody.velocity = velocity;
				playersType[i] = playerType;
				MenuMovement mm = players[i].GetComponent<MenuMovement>();
				mm.setPlayer(i);
				mm.setIdPlayer(idPlayer);
				Movement move = players[i].GetComponent<Movement>();
				move.SetPlayer(i);
			}
		} 
	}
	
	public int getMaxPlayers() {
	//Accede controllerActivate
		return maxPlayers;
	}
	
}
using UnityEngine;
using System.Collections;

public class BattleInformer1 : MonoBehaviour {
	
	public int maxPlayers = 4; //numero maximo de pnjs en juego
	public Vector3 standardPosition = Vector3.zero; //Posicion standard para colocar pnjs
	private GameObject[] players; //personajes en juego
	private int stage; //Pantalla
	public bool[] items;	
	
	
	void Start() {
	
		DontDestroyOnLoad(transform.gameObject);
		players = new GameObject[maxPlayers];
		Debug.Log(players[1]);

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
	public void changePlayer(GameObject playerType, int i) {
	/* Acceden controllerActivate y characterAvatar
	 * i debe ser entre 1 y maxPlayers
	 *Si playerType = null simplemente destruye un jugador 
	 *Sino: Si jugador = null instancia playerType en standardPosition
	 *		Sino: instancia playerType en posicion de jugador
	 */
		if(i <= maxPlayers)	{
			Debug.Log(playerType);

			Vector3 position = standardPosition;
			if(players[i] != null) {
				position = players[i].transform.position;
				Destroy (players[i]);
			}
			
			//Instantiate particles			
			if(playerType != null) { 
				players[i] = Instantiate(playerType,position, playerType.transform.rotation) as GameObject; 
				MenuMovement mm = players[i].GetComponent<MenuMovement>();
				mm.setPlayer(i);
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
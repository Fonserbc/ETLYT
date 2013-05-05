using UnityEngine;
using System.Collections;

public class controllerActivate : MonoBehaviour {
	
	private int maxPlayers; //numero maximo de jugadores en juego
	private BattleInformer bi; //accederemos bastante a battleinformer
	
	private bool[] players; //iremos marcando que numero de mando esta en funcionamiento
	public GameObject standardAvatar; //Tenemos guardado el avatar standard
	
	Control control;

	void Start () {
		control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		bi = GameObject.FindGameObjectWithTag("BattleInformer").GetComponent<BattleInformer>();
		maxPlayers = bi.getMaxPlayers();
		players = new bool[maxPlayers];
	
	}
	
	void Update () {
		/*ir mirando que mandos estan activados y con que tipo
		 * Por cada posicion del vector players:
		 * Mirar si su mando esta activado o no.
		 * Si no esta activado y antes lo estaba enviar un bi.changePlayer(null, i);
		 * Si esta activado y ya no lo esta enviar un bi.changeType(standardAvatar, i);
		 */
		for (int i=0; i<maxPlayers; i++) {
			
		}

	
	}
}

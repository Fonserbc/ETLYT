#pragma strict

public var player1 : Transform;
public var player2 : Transform;

public var height : float = 0;
public var distance : float = 25;

function Start () {
	if (player1 == null || player2 == null) {
		Debug.Log("Error on camera, players not found");
		(gameObject.GetComponent(CameraMov)).enabled = false;
	}
}

function Update () {
	var mid : Vector3 = (player1.position + player2.position)/2;
	
	transform.position = mid + Vector3(0,height,-distance);
}
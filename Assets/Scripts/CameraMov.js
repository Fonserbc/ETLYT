#pragma strict

public var height : float = 0;
public var distance : float = 9;
//public var speed : float = 5;
public var margin : Vector2;

private var players : GameObject[];
private var offset : float = 0;

function Start () {
	players = GameObject.FindGameObjectsWithTag("Player");
}

function Update () {
	var mid : Vector3 = Vector3.zero;
	var mdx : float = 0;
	var mdy : float = 0;
	
	for (var p : GameObject in players) {
		mid += p.transform.position;
		
		var dx : float = Mathf.Abs(p.transform.position.x - transform.position.x);
		if (dx > mdx) 
			mdx = dx;
			
		var dy : float = Mathf.Abs(p.transform.position.y - transform.position.y);
		if (dy > mdy)
			mdy = dy;
	}
	mid /= players.Length;
	
	var maxY : float = Mathf.Abs(distance*Mathf.Tan(Mathf.Deg2Rad * camera.fov));
	var maxX : float = Mathf.Abs(distance*Mathf.Tan(Mathf.Deg2Rad * camera.fov * camera.aspect));
	
	var minDistY : float = Mathf.Max(mdy, maxY) / Mathf.Tan(Mathf.Deg2Rad * camera.fov);
	var minDistX : float = Mathf.Max(mdx, maxX) / Mathf.Tan(Mathf.Deg2Rad * (camera.fov * camera.aspect));
	
	Debug.Log(maxY+":"+mdy+ "/" + maxX+":"+mdx+" "+(Mathf.Tan(Mathf.Deg2Rad * camera.fov)));
	
	
	//transform.position = Vector3.Lerp(transform.position, mid + Vector3(0,height,-distance), Time.deltaTime*speed);
	//transform.Translate(Vector3.Lerp(transform.position, mid + Vector3(0,height,-distance), Time.deltaTime*speed) - transform.position);
	transform.position = mid + Vector3(0,height,Mathf.Min(-distance, -Mathf.Max(minDistY, minDistX)));
}
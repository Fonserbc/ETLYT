#pragma strict

public var height : float = 0;
//public var distance : float = 9;
public var speed : float = 5;
public var margin : Vector2;

private var players : GameObject[];
private var offset : float = 0;

function Start () {
	players = GameObject.FindGameObjectsWithTag("Player");
	camera.transparencySortMode = TransparencySortMode.Orthographic;
}

function FixedUpdate () {
	players = GameObject.FindGameObjectsWithTag("Player");
	var mid : Vector3 = Vector3.zero;
	var mx : float = 0;
	var my : float = 0;
	var mdx : float = 0;
	var mdy : float = 0;
	
	for (var p : GameObject in players) {
		mid += p.transform.position;
		
		mx = Mathf.Max(mx, Mathf.Abs(transform.position.x - p.transform.position.x));
		my = Mathf.Max(my, Mathf.Abs((transform.position.y - height) - p.transform.position.y));
		/*
		var dx : float = Mathf.Abs(p.transform.position.x - transform.position.x);
		if (dx > mdx) 
			mdx = dx;
			
		var dy : float = Mathf.Abs(p.transform.position.y - transform.position.y);
		if (dy > mdy)
			mdy = dy;*/
	}
	mid /= players.Length;
	
	var minDistY : float = (my + margin.y) / Mathf.Tan(Mathf.Deg2Rad * (camera.fov/2));
	var minDistX : float = (mx + margin.x) / Mathf.Tan(Mathf.Deg2Rad * ((camera.fov/2) * camera.aspect));
	
	//Debug.Log(mx + "," + my + ": " + minDistX + ", " + minDistY);
	//Debug.Log(Mathf.Tan(Mathf.Deg2Rad * (camera.fov/2)) + "," + Mathf.Tan(Mathf.Deg2Rad * ((camera.fov/2) * camera.aspect)));
	
	/*
	var maxY : float = Mathf.Abs(distance*Mathf.Tan(Mathf.Deg2Rad * camera.fov));
	var maxX : float = Mathf.Abs(distance*Mathf.Tan(Mathf.Deg2Rad * camera.fov * camera.aspect));
	
	var minDistY : float = Mathf.Max(mdy, maxY) / Mathf.Tan(Mathf.Deg2Rad * camera.fov);
	var minDistX : float = Mathf.Max(mdx, maxX) / Mathf.Tan(Mathf.Deg2Rad * (camera.fov * camera.aspect));*/
	
//	Debug.Log(maxY+":"+mdy+ "/" + maxX+":"+mdx+" "+(Mathf.Tan(Mathf.Deg2Rad * camera.fov)));
	
	
	//transform.position = Vector3.Lerp(transform.position, mid + Vector3(0,height,-distance), Time.deltaTime*speed);
	//transform.Translate(Vector3.Lerp(transform.position, mid + Vector3(0,height,-distance), Time.deltaTime*speed) - transform.position);
	rigidbody.MovePosition(Vector3.Lerp(transform.position, mid + Vector3(0,height, -Mathf.Max(minDistY, minDistX)), speed*Time.deltaTime));
}

function OnDrawGizmos () {
	var pos : Vector3 = transform.position;
	pos.z = 0;

	Gizmos.color = Color.cyan;
	Gizmos.DrawWireCube(pos, Vector3(2*margin.x, 2*margin.y, 1)); 
}
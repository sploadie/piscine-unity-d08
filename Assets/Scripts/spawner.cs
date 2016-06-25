using UnityEngine;
using System.Collections;

public class spawner : MonoBehaviour {

	private diabloUnit liveUnit;
	public diabloUnit unit;

	private bool spawning = false;
	public float spawnDelay = 3f;

	void OnDrawGizmos() {
		Gizmos.DrawIcon (transform.position, "death.png");
	}

	// Use this for initialization
	void Start () {
		spawn ();
	}
	
	// Update is called once per frame
	void Update () {
		if (liveUnit == null && !spawning) {
			spawning = true;
			Invoke("spawn", spawnDelay);
		}
	}

	private void spawn() {
		liveUnit = GameObject.Instantiate (unit, transform.position, transform.rotation) as diabloUnit;
		liveUnit.transform.parent = this.transform;
		spawning = false;
	}
}

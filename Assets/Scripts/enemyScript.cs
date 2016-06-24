using UnityEngine;
using System.Collections;

public class enemyScript : MonoBehaviour {

	public float aggroDistance = 15f;
	diabloUnit enemy;

	// Use this for initialization
	void Start () {
		enemy = GetComponent<diabloUnit> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (aggroDistance > Vector3.Distance (this.transform.position, diabloPlayer.instance.maya.transform.position)) {
			enemy.attack(diabloPlayer.instance.maya);
		}
	}
}

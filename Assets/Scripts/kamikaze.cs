using UnityEngine;
using System.Collections;

public class kamikaze : MonoBehaviour {

	public float deleteAfter;

	// Use this for initialization
	void Start () {
		GameObject.Destroy (this.gameObject, deleteAfter);
	}
}

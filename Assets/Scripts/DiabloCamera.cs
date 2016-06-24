using UnityEngine;
using System.Collections;

public class DiabloCamera : MonoBehaviour {

	public diabloUnit focus;
	public Vector3 offset;

	// Use this for initialization
	void Start () {
		transform.position = focus.transform.position + offset;
	}
	
	// Update is called once per frame
	void Update () {
//		transform.position = focus.transform.position + offset;
		transform.position = Vector3.Lerp (transform.position, focus.agent.nextPosition + offset, 0.3f);
		if (Input.GetKey (KeyCode.LeftShift)) {
			transform.Rotate (0f, -Input.GetAxis ("Horizontal") * 2, 0f);
		} else {
			transform.Rotate (0f, -Input.GetAxis ("Horizontal"), 0f);
		}
	}
}

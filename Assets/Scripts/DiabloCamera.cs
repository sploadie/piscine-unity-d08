using UnityEngine;
using System.Collections;

public class DiabloCamera : MonoBehaviour {

	public diabloUnit focus;
	public Vector3 offset;

	// Use this for initialization
	void Start () {
		if (focus && focus.agent)
			transform.position = focus.transform.position + offset;
	}
	
	// Update is called once per frame
	void Update () {
//		transform.position = focus.transform.position + offset;
		if (focus && focus.agent)
			transform.position = Vector3.Lerp (transform.position, focus.agent.nextPosition + offset, 0.3f);
		if (Input.GetKey (KeyCode.LeftShift)) {
			transform.Rotate (0f, -Input.GetAxis ("Horizontal") * 2, 0f);
		} else {
			transform.Rotate (0f, -Input.GetAxis ("Horizontal"), 0f);
		}
//		Camera.main.fieldOfView = Mathf.Clamp (Camera.main.fieldOfView - Input.GetAxis ("Vertical"), 20f, 80f);
		float scale = -Input.GetAxis ("Vertical") * Time.deltaTime;
		if (!(Camera.main.transform.localPosition.y <= 2.1f && scale < 0) && !(Camera.main.transform.localPosition.y >= 8f && scale > 0))
		Camera.main.transform.localPosition = Vector3.Scale (Camera.main.transform.localPosition, new Vector3 (1f+scale, 1f+(scale/2f), 1f+scale));
	}
}

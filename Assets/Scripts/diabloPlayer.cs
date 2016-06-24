using UnityEngine;
using System.Collections;

public class diabloPlayer : MonoBehaviour {

	public static diabloPlayer instance;

	public diabloUnit maya { get; private set; }

	void Awake() {
		if (!instance)
			instance = this;
		maya = GetComponent<diabloUnit> ();
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			Debug.DrawLine (mouseRay.origin, mouseRay.origin + mouseRay.direction * 100f, Color.red, 3f);
			if (Physics.Raycast (mouseRay, out hit)) {
				Debug.Log ("Clicked " + hit.collider);
				maya.runTo(hit.point);
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class diabloPlayer : MonoBehaviour {

	public static diabloPlayer instance;

	public diabloUnit maya;

	public Text pauseText;
	public bool paused { get; private set; }

	public genericMeter healthBar;
	public genericMeter XPBar;
	public DiabloCamera enemyCam;

	void Awake() {
		if (!instance)
			instance = this;
	}

	// Use this for initialization
	void Start () {
		paused = false;
		pauseText.gameObject.SetActive(false);
		Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			paused = !paused;
			if (paused) {
				Time.timeScale = 0;
				pauseText.gameObject.SetActive(true);
			} else {
				Time.timeScale = 1;
				pauseText.gameObject.SetActive(false);
			}
		}
		if (!paused && maya) {
			healthBar.setBar(maya.health / maya.maxHealth);
			XPBar.setBar((float)maya.playerXP / (float)(maya.Level * 10));
			if (maya.target != null) {
				enemyCam.focus = maya.target;
			} else if (enemyCam.focus == null) {
				enemyCam.focus = maya;
			}
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit hit;
				Ray mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
				Debug.DrawLine (mouseRay.origin, mouseRay.origin + mouseRay.direction * 100f, Color.red, 3f);
				if (Physics.Raycast (mouseRay, out hit, Mathf.Infinity, 1)) {
//				Debug.Log ("Clicked " + hit.collider);
					if (hit.collider.tag == "Zombie")
						maya.attack (hit.collider.GetComponent<diabloUnit> ());
					else
						maya.runTo (hit.point);
				}
			}
		}
	}

	void OnDestroy() {
		Application.LoadLevel(Application.loadedLevel);
	}
}

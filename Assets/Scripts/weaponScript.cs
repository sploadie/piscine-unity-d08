using UnityEngine;
using System.Collections;

public class weaponScript : MonoBehaviour {

	public diabloUnit unit;
	public Vector3 positionOffset;
	public Vector3 rotationOffset;

	public Transform hand { get; private set;}
	public GameObject weapon;

	// Use this for initialization
	void Start () {
		MeshRenderer weaponMesh = this.GetComponentInChildren<MeshRenderer> ();
		if (weaponMesh)
			weapon = weaponMesh.gameObject;
		if (weapon)
			weapon.SetActive(false);
		if (unit != null && unit.animator != null)
			hand = unit.animator.GetBoneTransform (HumanBodyBones.RightHand);
	}
	
	// Update is called once per frame
	void Update () {
		if (hand == null && unit != null && unit.animator != null)
			hand = unit.animator.GetBoneTransform (HumanBodyBones.RightHand);
		if (hand && weapon) {
			transform.position = hand.position + positionOffset;
			transform.rotation = hand.rotation * Quaternion.Euler (rotationOffset);
			if (unit.attackCooldown > 0)
				weapon.SetActive(true);
			else
				weapon.SetActive(false);
		}
	}
}

using UnityEngine;
using System.Collections;

public class niceFeet : MonoBehaviour {

	public float ikWeight = 1f;

	private Animator anim;
	
	private Transform leftFoot;
	private Transform rightFoot;

	private Transform hintLeft;
	private Transform hintRight;
	
	private Vector3    lFpos;
	private Quaternion lFrot;
	private Vector3    rFpos;
	private Quaternion rFrot;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		leftFoot = anim.GetBoneTransform (HumanBodyBones.LeftFoot);
		rightFoot = anim.GetBoneTransform (HumanBodyBones.RightFoot);
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if (Physics.Raycast (leftFoot.TransformPoint (Vector3.zero), -Vector3.up, out hit, 1f)) {
			lFpos = hit.point;
			lFrot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
		}
		if (Physics.Raycast (rightFoot.TransformPoint (Vector3.zero), -Vector3.up, out hit, 1f)) {
			rFpos = hit.point;
			rFrot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
		}
	}

	void OnAnimatorIK() {
		// FEET
		// Give weight
		anim.SetIKPositionWeight (AvatarIKGoal.LeftFoot, ikWeight);
		anim.SetIKPositionWeight (AvatarIKGoal.RightFoot, ikWeight);

		anim.SetIKRotationWeight (AvatarIKGoal.LeftFoot, ikWeight);
		anim.SetIKRotationWeight (AvatarIKGoal.RightFoot, ikWeight);

		// Move
		anim.SetIKPosition (AvatarIKGoal.LeftFoot, lFpos);
		anim.SetIKPosition (AvatarIKGoal.RightFoot, rFpos);

		anim.SetIKRotation (AvatarIKGoal.LeftFoot, lFrot);
		anim.SetIKRotation (AvatarIKGoal.RightFoot, rFrot);
	}
}

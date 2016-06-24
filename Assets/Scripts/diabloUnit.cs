using UnityEngine;
using System.Collections;

public class diabloUnit : MonoBehaviour {

	public NavMeshAgent agent { get; private set; }
	public Animator animator { get; private set; }
//	public float maxRemainingDistance = 1.0f;
//	public float maxIdleVelocity = 0.01f;

	public State state;
	public enum State { idle, running, attacking, dead }

	bool shouldMove = false;

	void OnDrawGizmos() {
		if (agent && agent.hasPath) {
			Gizmos.DrawIcon (agent.nextPosition, "enemy.png");
			Gizmos.color = Color.blue;
			Gizmos.DrawLine (agent.nextPosition, agent.destination);
		}
		if (animator)
			Gizmos.DrawIcon (animator.rootPosition, "death.png");
	}

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		agent.updateRotation = true;
		agent.updatePosition = false;
		animator = GetComponent<Animator> ();
		state = State.idle;
	}
	
	// Update is called once per frame
	void Update () {
		// PRE ANIMATION
		if (state < State.attacking) {
			// Get agent delta
			Vector3 worldDeltaPosition = agent.nextPosition - transform.position;
			// Update velocity if moving
			shouldMove = agent.hasPath;
			// Update animation parameters
			if (shouldMove) {
				state = State.running;
				animator.speed = agent.velocity.magnitude / 5.315f; // Must know average rootmotion velocity (Z axis)
			} else {
				state = State.idle;
				animator.applyRootMotion = false;
				if (animator.speed < 1) {
					animator.speed = Mathf.Clamp01(animator.speed + Time.deltaTime);
				}
			}

			// Pull character towards agent
			if (worldDeltaPosition.magnitude > agent.radius)
				transform.position = agent.nextPosition - 0.9f*worldDeltaPosition;
		}
		// ANIMATION
		animator.SetInteger ("state", (int)state);
		// POST ANIMATION
	}

	void OnAnimatorMove () {
		// Update position to agent position (along Y axis)
		if (shouldMove) {
			Vector3 position = animator.rootPosition;
			if (position.y < agent.nextPosition.y)
				position.y = agent.nextPosition.y; // This should be for positive or negative delta, but I'll settle for my character not sinking into rock
			transform.position = position;
		}// else {
//			transform.position = agent.nextPosition;
//		}
	}

	public void runTo(Vector3 destination) {
		state = State.running;
		agent.destination = destination;
	}
}

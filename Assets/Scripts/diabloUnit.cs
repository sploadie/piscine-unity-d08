using UnityEngine;
using System.Collections;

public class diabloUnit : MonoBehaviour {

	public float health = 100f;
	public float damage = 5f;
	public float attackSpeed = 2f;

	private float attackCooldown = 0f;

	public NavMeshAgent agent { get; private set; }
	public Animator animator { get; private set; }
//	public float maxRemainingDistance = 1.0f;
//	public float maxIdleVelocity = 0.01f;

	public State state;
	public enum State { idle, running, attacking, dead }

	private diabloUnit target;

	private bool shouldMove = false;

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
		if (health < 0) {
			if (state != State.dead) {
				state = State.dead;
				animator.SetInteger ("state", (int)state);
				GameObject.Destroy(this.gameObject, 5);
			}
			transform.position += new Vector3 (0f, -0.1f, 0f) * Time.deltaTime;
		}
		if (state != State.dead) {
			attackCooldown -= Time.deltaTime;
			if (target) {
				agent.destination = target.transform.position;
				if (Vector3.Distance(transform.position, target.transform.position) < 2f) {
					transform.LookAt(target.transform.position);
				}
			}
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
			// ANIMATION
			animator.SetInteger ("state", (int)state);
		}
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
		if (attackCooldown <= 0) {
			target = null;
			state = State.running;
			agent.destination = destination;
		} else if (!actionWasDelayed) {
			actionWasDelayed = true;
			delayedDestination = destination;
			delayedTarget = null;
			Invoke("delayedAction", attackCooldown);
		}
	}

	public void attack(diabloUnit victim) {
		if (attackCooldown <= 0) {
			target = victim;
		} else if (!actionWasDelayed) {
			actionWasDelayed = true;
			delayedTarget = victim;
			Invoke("delayedAction", attackCooldown);
		}
	}

	private bool actionWasDelayed = false;
	private Vector3 delayedDestination = Vector3.zero;
	private diabloUnit delayedTarget = null;
	private void delayedAction() {
		if (delayedTarget)
			attack (delayedTarget);
		else
			runTo (delayedDestination);
		actionWasDelayed = false;
	}

	void handleStrike(Collider collider) {
		if (!target)
			return;
		diabloUnit victim = collider.GetComponent<diabloUnit> ();
		if (victim == target) {
			attackCooldown = attackSpeed;
			transform.LookAt(target.transform.position);
			animator.SetTrigger("attack");
			target.health -= damage;
			agent.ResetPath();
			target = null;
		}
	}

	void OnTriggerEnter(Collider collider) { handleStrike (collider); }
	void OnTriggerStay(Collider collider) { handleStrike (collider); }
}

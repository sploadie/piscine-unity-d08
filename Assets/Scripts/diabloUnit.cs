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

	public diabloUnit target { get; private set; }

	private bool shouldMove = false;

	void OnDrawGizmos() {
		if (agent && agent.hasPath) {
			Gizmos.DrawIcon (agent.nextPosition, "wayPoint.png");
			Gizmos.color = Color.blue;
			Gizmos.DrawLine (agent.nextPosition, agent.destination);
		}
//		if (animator)
//			Gizmos.DrawIcon (animator.rootPosition, "enemy.png");
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
		// Handle death
		if (health < 0) {
			if (state != State.dead) {
				state = State.dead;
				animator.SetInteger ("state", (int)state);
				GameObject.Destroy(this.gameObject, 5);
			}
			// On death, unit sinks into ground
			transform.position += new Vector3 (0f, -0.1f, 0f) * Time.deltaTime;
		}
		if (state != State.dead) {
			// Increment attack action cooldown
			attackCooldown -= Time.deltaTime;
			// Move towards target if target exists and is not dead
			// If target not set, unit still moves if agent has destination (by runTo)
			if (target) {
				if (target.state == State.dead) {
					target = null;
					agent.ResetPath();
				} else {
					agent.destination = target.transform.position;
					if (Vector3.Distance(transform.position, target.transform.position) < 2f) {
						transform.LookAt(target.transform.position);
					}
				}
			}
			// Get agent delta
			Vector3 worldDeltaPosition = agent.nextPosition - transform.position;
			// Set if the agent should pull the unit
			shouldMove = agent.hasPath;
			if (shouldMove) {
				// Move unit at speed of agent by manipulating animation speed
				state = State.running;
				animator.speed = agent.velocity.magnitude / 5.315f; // Must know average rootmotion velocity (Z axis)
			} else {
				// Smooth stop animation (animation speed lerps back to max)
				state = State.idle;
				animator.applyRootMotion = false;
				if (animator.speed < 1) {
					animator.speed = Mathf.Clamp01(animator.speed + Time.deltaTime);
				}
			}
			// Pull unit towards agent
			if (worldDeltaPosition.magnitude > agent.radius)
				transform.position = agent.nextPosition - 0.9f*worldDeltaPosition;
			// ANIMATION
			animator.SetInteger ("state", (int)state);
		}
	}

	void OnAnimatorMove () {
		// After animation moves unit, fix unit position
		if (shouldMove) {
			Vector3 position = animator.rootPosition;
			if (position.y < agent.nextPosition.y)
				position.y = agent.nextPosition.y; // This should adjust Y axis if agent ever moves more than animation (like jumps and stairs), but whatever
			transform.position = position;
		}
	}

	public void runTo(Vector3 destination) {
		// Move to new destination
		// If attack is in progress, overwrite post-attack action
		if (attackCooldown <= 0) {
			target = null;
			state = State.running;
			agent.destination = destination;
		} else {
			delayedDestination = destination;
			delayedTarget = null;
			Invoke("delayedAction", attackCooldown);
		}
	}

	public void attack(diabloUnit victim) {
		// Move in and attack new target
		// If attack is in progress, overwrite post-attack action
		if (attackCooldown <= 0) {
			target = victim;
		} else {
			delayedTarget = victim;
			Invoke("delayedAction", attackCooldown);
		}
	}

	private Vector3 delayedDestination = Vector3.zero;
	private diabloUnit delayedTarget = null;
	private void delayedAction() {
		// Post-attack action
		// Overwritten by last command given during attack cooldown
		if (delayedTarget)
			attack (delayedTarget);
		else
			runTo (delayedDestination);
	}

	void handleStrike(Collider collider) {
		// If collision wasn't with target, ignore collision
		if (!target || target.state == State.dead)
			return;
		diabloUnit victim = collider.GetComponent<diabloUnit> ();
		if (victim == target) {
			// Set attack cooldown quickly to prevent action overlap (not sure it actually matters but jic)
			attackCooldown = attackSpeed;
			// Look at target and perform attack animation
			transform.LookAt(target.transform.position);
			animator.SetTrigger("attack");
			// Deal damage to unit
			target.health -= damage;
			// Tell agent to stop
			agent.ResetPath();
			// Set post-attack action (to be overwritten if player wishes)
			delayedTarget = target;
			// Clear target so agent doesn't try to move closer
			target = null;
			// Invoke delayed action
			Invoke("delayedAction", attackCooldown);
		}
	}

	void OnTriggerEnter(Collider collider) { handleStrike (collider); }
	void OnTriggerStay(Collider collider) { handleStrike (collider); }
}

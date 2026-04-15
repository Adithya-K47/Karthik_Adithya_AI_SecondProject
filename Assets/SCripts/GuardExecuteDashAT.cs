using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions {

	public class GuardExecuteDashAT : ActionTask {
		public BBParameter<Vector3> homePosition;
		public float dashDistance = 8.0f;
		public float dashSpeed = 20.0f;
		public Color dashingColor = Color.red;

		private NavMeshAgent navAgent;
		private Material objectMaterial;
		private Transform playerTransform;

		protected override string OnInit() {
			navAgent = agent.GetComponent<NavMeshAgent>();
			objectMaterial = agent.GetComponent<Renderer>().material;
			
			// Auto-find player
			GameObject playerObj = GameObject.FindWithTag("Player");
			if (playerObj != null) {
				playerTransform = playerObj.transform;
			}
			
			return null;
		}

		protected override void OnExecute() {
			objectMaterial.color = dashingColor;
			navAgent.isStopped = false;
			navAgent.speed = dashSpeed;
			navAgent.acceleration = 100f; 
			
			
			Vector3 dashDirection = Vector3.back;
			Vector3 dashTarget = homePosition.value + dashDirection * dashDistance;
			navAgent.SetDestination(dashTarget);
		}

		protected override void OnUpdate() {
			
			if (playerTransform != null) {
				float distToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);
				// Log occasionally to avoid spam
				if (Time.frameCount % 5 == 0) Debug.Log($"Guard Dash - Dist to player: {distToPlayer}");

				if (distToPlayer < 1.5f) {
					Debug.Log("Guard: IMPACT! Hit player successfully.");
					EndAction(true);
					return;
				}
			}

			
			if (!navAgent.pathPending && navAgent.remainingDistance <= 0.2f) {
				EndAction(true);
			}
		}

		protected override void OnStop() {
			navAgent.isStopped = true;
			navAgent.ResetPath();
		}
	}
}

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions {

	public class WardenMoveToAlertPointAT : ActionTask {
		public BBParameter<Transform> alertPoint;
		public float alertSpeed = 5f;
		public float arrivalDistance = 0.5f;

		private NavMeshAgent navAgent;

		protected override string OnInit() {
			navAgent = agent.GetComponent<NavMeshAgent>();
			return null;
		}

		protected override void OnExecute() {
			navAgent.isStopped = false;
			navAgent.speed = alertSpeed;
			if (alertPoint.value != null) {
				navAgent.SetDestination(alertPoint.value.position);
			} else {
				EndAction(false);
			}
		}

		protected override void OnUpdate() {
			if (!navAgent.pathPending && navAgent.remainingDistance <= arrivalDistance) {
				EndAction(true);
			}
		}

		protected override void OnStop() {
			navAgent.isStopped = true;
			navAgent.ResetPath();
		}
	}
}

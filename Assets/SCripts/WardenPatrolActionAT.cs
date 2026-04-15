using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions {

	public class WardenPatrolActionAT : ActionTask {
		public BBParameter<Transform[]> waypoints;
		public BBParameter<int> currentIndex;
		public int waypointsPerCycle = 3;
		public float speed = 3.5f;
		public float stoppingDistance = 0.5f;

		private NavMeshAgent navAgent;
		private int waypointsVisitedInThisCycle = 0;

		protected override string OnInit() {
			navAgent = agent.GetComponent<NavMeshAgent>();
			return null;
		}

		protected override void OnExecute() {
			waypointsVisitedInThisCycle = 0;
			navAgent.isStopped = false;
			navAgent.speed = speed;
			SetNextDestination();
		}

		protected override void OnUpdate() {
			if (!navAgent.pathPending && navAgent.remainingDistance <= stoppingDistance) {
				waypointsVisitedInThisCycle++;
				currentIndex.value = (currentIndex.value + 1) % waypoints.value.Length;

				if (waypointsVisitedInVisitCycle()) {
					EndAction(true);
				} else {
					SetNextDestination();
				}
			}
		}

		private bool waypointsVisitedInVisitCycle() {
			return waypointsVisitedInThisCycle >= waypointsPerCycle;
		}

		private void SetNextDestination() {
			if (waypoints.value != null && waypoints.value.Length > 0) {
				int index = currentIndex.value % waypoints.value.Length;
				navAgent.SetDestination(waypoints.value[index].position);
			} else {
				EndAction(false);
			}
		}

		protected override void OnStop() {
			navAgent.isStopped = true;
			navAgent.ResetPath();
		}
	}
}

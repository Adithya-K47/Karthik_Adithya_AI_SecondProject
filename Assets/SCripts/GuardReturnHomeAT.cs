using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions {

	public class GuardReturnHomeAT : ActionTask {
		public BBParameter<Vector3> homePosition;
		public float moveSpeed = 3.5f;
		public float stoppingDistance = 0.5f;
		public Color idleColor = Color.white;

		private NavMeshAgent navAgent;
		private Material objectMaterial;

		protected override string OnInit() {
			navAgent = agent.GetComponent<NavMeshAgent>();
			objectMaterial = agent.GetComponent<Renderer>().material;
			return null;
		}

		protected override void OnExecute() {
			objectMaterial.color = idleColor;
			navAgent.isStopped = false;
			navAgent.speed = moveSpeed;
			navAgent.acceleration = 8f;
			
			navAgent.SetDestination(homePosition.value);
		}

		protected override void OnUpdate() {
			if (!navAgent.pathPending && navAgent.remainingDistance <= stoppingDistance) {
				EndAction(true);
			}
		}
	}
}

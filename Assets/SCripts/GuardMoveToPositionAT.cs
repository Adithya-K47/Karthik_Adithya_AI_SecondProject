using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions {

	public class GuardMoveToPositionAT : ActionTask {
		public BBParameter<Vector3> homePosition;
		public float moveSpeed = 3.5f;
		public float stoppingDistance = 0.5f;
		public Color movingColor = Color.yellow;

		private NavMeshAgent navAgent;
		private Material objectMaterial;

		protected override string OnInit() {
			navAgent = agent.GetComponent<NavMeshAgent>();
			objectMaterial = agent.GetComponent<Renderer>().material;
			return null;
		}

		protected override void OnExecute() {
			objectMaterial.color = movingColor;
			navAgent.isStopped = false;
			navAgent.speed = moveSpeed;
			
			
			navAgent.SetDestination(homePosition.value);
		}

		protected override void OnUpdate() {
			if (!navAgent.pathPending && navAgent.remainingDistance <= stoppingDistance) {
				EndAction(true);
			}
		}
	}
}

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions {

	public class GuardChargeUpAT : ActionTask {
		public BBParameter<Vector3> homePosition;
		public float chargeDistance = 1.0f;
		public float chargeSpeed = 2.0f;
		public Color chargingColor = new Color(1f, 0.5f, 0f); // Orange

		private NavMeshAgent navAgent;
		private Material objectMaterial;

		protected override string OnInit() {
			navAgent = agent.GetComponent<NavMeshAgent>();
			objectMaterial = agent.GetComponent<Renderer>().material;
			return null;
		}

		protected override void OnExecute() {
			objectMaterial.color = chargingColor;
			navAgent.isStopped = false;
			navAgent.speed = chargeSpeed;
			
			
			Vector3 chargeTarget = homePosition.value + Vector3.forward * chargeDistance;
			navAgent.SetDestination(chargeTarget);
		}

		protected override void OnUpdate() {
			if (!navAgent.pathPending && navAgent.remainingDistance <= 0.1f) {
				EndAction(true);
			}
		}
	}
}

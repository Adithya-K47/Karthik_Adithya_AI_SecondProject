using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions {

	public class DashAT : ActionTask {
		public BBParameter<Transform> homePoint;
		public float dashDistance;
		public float dashSpeed;
		public float dashDuration;
		public float stoppingDistance;
		public float chargeDuration = 2f;
		public float chargeBackupDistance = 1f;
		public Color chargeColor = Color.yellow;
		public Color normalColor = Color.white;

		private NavMeshAgent navAgent;
		private Material objectMaterial;
		private float stateTime = 0f;
		private float originalSpeed;

		private enum DashPhase {
			Charging,
			Dashing,
			Returning
		}
		private DashPhase currentPhase;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit() {
			navAgent = agent.GetComponent<NavMeshAgent>();
			objectMaterial = agent.GetComponent<Renderer>().material;
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() {
			stateTime = 0f;
			currentPhase = DashPhase.Charging;
			
			navAgent.isStopped = false;
			originalSpeed = navAgent.speed;
			navAgent.acceleration = 8f; 
			
			objectMaterial.color = chargeColor;
			
			Vector3 chargeDestination = agent.transform.position - (agent.transform.forward * chargeBackupDistance);
			navAgent.SetDestination(chargeDestination);
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate() {
			stateTime += Time.deltaTime;
			
			if(currentPhase == DashPhase.Charging)
			{
				if(stateTime >= chargeDuration)
				{
					currentPhase = DashPhase.Dashing;
					stateTime = 0f;
					
					objectMaterial.color = normalColor;
					navAgent.speed = dashSpeed;
					navAgent.acceleration = 120f;
					
					Vector3 dashDestination = agent.transform.position + (agent.transform.forward * dashDistance);
					navAgent.SetDestination(dashDestination);
				}
			}
			else if(currentPhase == DashPhase.Dashing)
			{
				if(stateTime > dashDuration)
				{
					currentPhase = DashPhase.Returning;
					stateTime = 0f;
					
					navAgent.speed = originalSpeed;
					navAgent.acceleration = 8f;
					navAgent.SetDestination(homePoint.value.position);
				}
			}
			else if(currentPhase == DashPhase.Returning)
			{
				if(!navAgent.pathPending && navAgent.remainingDistance <= stoppingDistance)
				{
					EndAction(true);
				}
			}
		}

		//Called when the task is disabled.
		protected override void OnStop() {
			objectMaterial.color = normalColor;
			navAgent.speed = originalSpeed;
			navAgent.acceleration = 8f; 
			
			navAgent.isStopped = true;
			navAgent.ResetPath();
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}

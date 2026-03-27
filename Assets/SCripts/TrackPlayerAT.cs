using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class TrackPlayerAT : ActionTask {
		public BBParameter<GameObject> playerTarget;
		public BBParameter<Renderer> targetRenderer;
		public BBParameter<Material> orangeMaterial;
		
		public float turnSpeed = 5f;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit() {
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() {
			if (targetRenderer.value != null && orangeMaterial.value != null) {
				targetRenderer.value.material = orangeMaterial.value;
			}
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate() {
			if (playerTarget.value != null) {
				Vector3 direction = (playerTarget.value.transform.position - agent.transform.position).normalized;
				direction.y = 0; // Lock constraints to horizontal
				if (direction != Vector3.zero) {
					Quaternion lookRot = Quaternion.LookRotation(direction);
					agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRot, Time.deltaTime * turnSpeed);
				}
			}
		}

		//Called when the task is disabled.
		protected override void OnStop() {
			
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}

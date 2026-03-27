using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class DashTelegraphAT : ActionTask {
		public BBParameter<Renderer> targetRenderer;
		public BBParameter<Material> telegraphMaterial;
		public float telegraphDuration = 1.0f;
		
		private float elapsedTime = 0f;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit() {
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() {
			elapsedTime = 0f;
			if (targetRenderer.value != null && telegraphMaterial.value != null) {
				targetRenderer.value.material = telegraphMaterial.value;
			}
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate() {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > telegraphDuration) {
				EndAction(true);
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

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class SoundAlarmAT : ActionTask {
		public BBParameter<Renderer> targetRenderer;
		public BBParameter<Material> redMaterial;
		public BBParameter<GameObject> guardBotTarget;
		public string guardAlarmVariableName = "isAlarmActive";

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit() {
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() {
			if (targetRenderer.value != null && redMaterial.value != null) {
				targetRenderer.value.material = redMaterial.value;
			}
			
			// Trigger the local alarm variable on the Guard Bot directly
			if (guardBotTarget.value != null) {
				var guardBlackboard = guardBotTarget.value.GetComponent<Blackboard>();
				if (guardBlackboard != null) {
					guardBlackboard.SetVariableValue(guardAlarmVariableName, true);
				}
			}
			
			EndAction(true);
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate() {

		}

		//Called when the task is disabled.
		protected override void OnStop() {
			
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}

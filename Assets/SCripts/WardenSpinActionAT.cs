using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class WardenSpinActionAT : ActionTask {
		public float spinDuration = 2.0f;
		public BBParameter<Transform> visionCone;
		public Color spinColor = Color.red;
		public Color patrolColor = Color.white;

		private Material objectMaterial;
		private float timeSpinning = 0f;
		private Quaternion startSpinRotation;

		protected override string OnInit() {
			objectMaterial = agent.GetComponent<Renderer>().material;
			return null;
		}

		protected override void OnExecute() {
			timeSpinning = 0f;
			startSpinRotation = agent.transform.rotation;
			objectMaterial.color = spinColor;
			
			if (visionCone.value != null) {
				visionCone.value.gameObject.SetActive(true);
			}
		}

		protected override void OnUpdate() {
			timeSpinning += Time.deltaTime;
			float t = timeSpinning / spinDuration;
			
			agent.transform.rotation = startSpinRotation * Quaternion.Euler(0, Mathf.Lerp(0, 360, t), 0);
			
			if (timeSpinning >= spinDuration) {
				EndAction(true);
			}
		}

		protected override void OnStop() {
			objectMaterial.color = patrolColor;
			if (visionCone.value != null) {
				visionCone.value.gameObject.SetActive(false);
			}
		}
	}
}

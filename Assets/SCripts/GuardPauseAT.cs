using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class GuardPauseAT : ActionTask {
		public float pauseDuration = 1.0f;
		private float elapsedTime = 0f;

		protected override string OnInit() {
			return null;
		}

		protected override void OnExecute() {
			elapsedTime = 0f;
		}

		protected override void OnUpdate() {
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= pauseDuration) {
				EndAction(true);
			}
		}
	}
}

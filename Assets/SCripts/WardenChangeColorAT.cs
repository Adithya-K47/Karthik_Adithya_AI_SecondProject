using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class WardenChangeColorAT : ActionTask {
		public Color colorToSet;
		private Renderer botRenderer;

		protected override string OnInit() {
			botRenderer = agent.GetComponent<Renderer>();
			return null;
		}

		protected override void OnExecute() {
			if (botRenderer != null) {
				botRenderer.material.color = colorToSet;
			}
			EndAction(true);
		}
	}
}

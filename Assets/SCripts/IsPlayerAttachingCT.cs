using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NodeCanvas.Tasks.Conditions {

	
	public class IsPlayerAttachingCT : ConditionTask {
		
		private PlayerAttachController playerController;

		protected override string OnInit(){
			GameObject player = GameObject.FindWithTag("Player");
			if (player != null) {
				playerController = player.GetComponent<PlayerAttachController>();
			} 
			
			return null;
		}

		protected override bool OnCheck() {
			if (playerController != null) {
				return playerController.isAttaching;
			}
			return false;
		}
	}
}

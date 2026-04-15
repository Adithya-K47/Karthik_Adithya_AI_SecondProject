using UnityEngine;

namespace AI.Traps {
    
	public class KillZone : MonoBehaviour {
		
		
		public string targetTag = "GuardBot";
		public GameObject explosionEffect;

		private void Awake() {
			
			BoxCollider col = GetComponent<BoxCollider>();
			if (col != null) {
				col.isTrigger = true;
			}
		}

		private void OnTriggerEnter(Collider other) {
			
			if (other.CompareTag(targetTag)) {	
				HandleGuardDestruction(other.gameObject);
			}
		}

		private void HandleGuardDestruction(GameObject guard) {
			
			Destroy(guard);
		}
	}
}

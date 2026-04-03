using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions
{

    public class PatrolWithSpinsAT : ActionTask
    {
        public BBParameter<Transform[]> waypoints;
        public float speed;
        public float stoppingDistance;
        public float spinDuration;
        public BBParameter<Transform> visionCone;
        public Color patrolColor;
        public Color spinColor;

        private NavMeshAgent navAgent;
        private Material objectMaterial;
        private int currentWaypointIndex = 0;
        private int pointsVisited = 0;
        private bool isSpinning = false;
        private float timeSpinning = 0f;
        private Quaternion startSpinRotation;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            navAgent = agent.GetComponent<NavMeshAgent>();
            objectMaterial = agent.GetComponent<Renderer>().material;
            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            isSpinning = false;
            visionCone.value.gameObject.SetActive(false);
            objectMaterial.color = patrolColor;
            navAgent.isStopped = false;
            navAgent.speed = speed;
            navAgent.SetDestination(waypoints.value[currentWaypointIndex].position);
        }

        //Called once per frame while the action is active.
        protected override void OnUpdate()
        {
            if (isSpinning)
            {
                timeSpinning += Time.deltaTime;
                float t = timeSpinning / spinDuration;

                agent.transform.rotation = startSpinRotation * Quaternion.Euler(0, Mathf.Lerp(0, 360, t), 0);

                if (timeSpinning > spinDuration)
                {
                    isSpinning = false;
                    visionCone.value.gameObject.SetActive(false);
                    pointsVisited = 0;

                    SetNextWaypoint();
                }
            }
            else
            {
                if (!navAgent.pathPending && navAgent.remainingDistance <= stoppingDistance)
                {
                    pointsVisited++;

                    if (pointsVisited >= 3)
                    {
                        isSpinning = true;
                        visionCone.value.gameObject.SetActive(true);
                        objectMaterial.color = spinColor;
                        timeSpinning = 0f;
                        startSpinRotation = agent.transform.rotation;
                        navAgent.isStopped = true;
                    }
                    else
                    {
                        SetNextWaypoint();
                    }
                }
            }
        }

        void SetNextWaypoint()
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.value.Length;

            objectMaterial.color = patrolColor;
            navAgent.isStopped = false;
            navAgent.speed = speed;
            navAgent.SetDestination(waypoints.value[currentWaypointIndex].position);
        }

        //Called when the task is disabled.
        protected override void OnStop()
        {
            visionCone.value.gameObject.SetActive(false);
            objectMaterial.color = patrolColor;
            navAgent.isStopped = true;
            navAgent.ResetPath();
        }

        //Called when the task is paused.
        protected override void OnPause()
        {

        }
    }
}

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{

    
    public class SoundAlarmAT : ActionTask
    {
        public BBParameter<GameObject> guardBotTarget;
        public string guardAlarmVariableName = "isAlarmActive";
        public string wardenCooldownVariableName = "isCoolingDown";

        protected override void OnExecute()
        {
            //
            SetAlarmState(true);
        
        }

        protected override void OnStop()
        {
           
            SetAlarmState(false);
            SetCooldownState(true);
        }

        private void SetAlarmState(bool value)
        {
            var guardBlackboard = guardBotTarget.value.GetComponent<Blackboard>();
            guardBlackboard.SetVariableValue(guardAlarmVariableName, value);
        }

        private void SetCooldownState(bool value)
        {
            blackboard.SetVariableValue(wardenCooldownVariableName, value);
        }
    }
}

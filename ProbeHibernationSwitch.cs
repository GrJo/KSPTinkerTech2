using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TinkerTech
{
    [KSPModule("ProbeHibernationSwitch")]
    public class ProbeHibernationSwitch : PartModule
    {
        [KSPField]
        public float hibernateChargeRateFactor = 0.1f;


        [KSPField(isPersistant = true)]
        public bool hibernating = false;

        [KSPEvent(guiActive = true, guiName = "Hibernate")]
        public void ToggleHibernate()
        {
            //switching off is always available 
            if (hibernating)
            {
                Debug.Log("Reacivating");
                hibernating = false;
            }
            else
            {
                Debug.Log("Hibernation");
                hibernating = true;
            }
            
            updateControlState();
        }

        double originalChargeRate;

        public override void OnStart(PartModule.StartState state)
        {
            ModuleCommand cmdModule = part.Modules.OfType<ModuleCommand>().First();
            originalChargeRate = cmdModule.inputResources.First().rate;

            updateControlState();
            
        }

        public override void OnUpdate()
        {
            if (vessel != null && hibernating)
                part.isControlSource = false; 
        }


        private void forceControlSources()
        {
            if (hibernating)
            {
                               
            }
        }

        private void updateControlState()
        {
            if (hibernating)
            {
                part.isControlSource = false;
                Events["ToggleHibernate"].guiName = "Reactivate";

                ModuleCommand cmdModule = part.Modules.OfType<ModuleCommand>().First();
                cmdModule.inputResources.First().rate = originalChargeRate * hibernateChargeRateFactor;
            }
            else
            {
                part.isControlSource = true;
                Events["ToggleHibernate"].guiName = "Hibernate";
                Events["ToggleHibernate"].guiActive = true;
                
                ModuleCommand cmdModule = part.Modules.OfType<ModuleCommand>().First();

                //restore consumption rate
                cmdModule.inputResources.First().rate = originalChargeRate;
            }
            
        }
    }
}

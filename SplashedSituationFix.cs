using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TinkerTech
{
    [KSPAddon(KSPAddon.Startup.Flight, true)]
    class SplashedSituationFix :  MonoBehaviour
    {

        void Start()
        {
            GameEvents.onVesselSituationChange.Add(new EventData<GameEvents.HostedFromToAction<Vessel, Vessel.Situations>>.OnEvent(OnVesselSituationChanged));
            DontDestroyOnLoad(this);
        }

        void OnDestroy()
        {
            GameEvents.onVesselSituationChange.Remove(OnVesselSituationChanged);
        }

        private void OnVesselSituationChanged(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> data)
        {
            //fix landed to splashed on fluids
            if (data.to == Vessel.Situations.LANDED)
            {
                //if a part with water contact exists, force splashed situation
                foreach (Part part in data.host.Parts)
                {
                    if (part.WaterContact)
                    {
                        Debug.Log("found part with water contact in LANDED situation, changing situation to SPLASHED");
                        data.host.situation = Vessel.Situations.SPLASHED;
                        data.host.Splashed = true;
                        return;
                    }
                }

            }//endif
        }//end onsituation
    }//end behaviour
}

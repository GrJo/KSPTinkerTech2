using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TinkerTech
{
    [KSPModule("TT-PressureLimitedCockpit")]
    public class TTPressureLimitedCockpit : PartModule
    {
        [KSPField(isPersistant = false)]
        public float minSafePressure = 0.5F;

        public override void OnStart(PartModule.StartState state)
        {
            base.OnStart(state);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (vessel != null)
            {
                double pressure =  FlightGlobals.getStaticPressure(vessel.altitude, vessel.orbit.referenceBody);
                double pressureAtAltPlus1000 = FlightGlobals.getStaticPressure(vessel.altitude + 1000, vessel.orbit.referenceBody);
                if (pressure < minSafePressure)
                    KillContainedKerbals();                  
                else if (pressureAtAltPlus1000 < minSafePressure && part.protoModuleCrew.Count > 0)
                    ScreenMessages.PostScreenMessage("High Altitude warning! Cockpit is within 1000m of its altitude limit!", 10F, ScreenMessageStyle.UPPER_CENTER);

            }
        }

        private void KillContainedKerbals()
        {
            while (part.protoModuleCrew.Count > 0)
            {
                ProtoCrewMember crewMember = part.protoModuleCrew[0];

                ScreenMessages.PostScreenMessage(crewMember.name + " passes out and dies! This cockpit is not pressurized and cannot be used at this altitude.", 10F, ScreenMessageStyle.UPPER_CENTER);

                part.RemoveCrewmember(crewMember);

                crewMember.Die();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TinkerTech
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)] //nonpersistent, restarted on each editor scene entry
    class PartCountLaunchLimiter : MonoBehaviour
    {
        private bool lockLaunchButton = false;

        private Rect windowRect  = new Rect();
        private string warningMsg = "";

        GUIStyle labelStyle = new GUIStyle();

        void Start()
        {
            labelStyle = HighLogic.Skin.box;
            labelStyle.normal.textColor = Color.red;
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.wordWrap = true;

            windowRect = new Rect(Screen.width - 350, 50, 280, 100);
            GameEvents.onEditorShipModified.Add(new EventData<ShipConstruct>.OnEvent(OnEditorShipModified));
        }

        private void OnEditorShipModified(ShipConstruct shipConstruct)
        {
            if (HighLogic.CurrentGame.Mode == Game.Modes.SANDBOX || ResearchAndDevelopment.Instance == null)
                return;


            int maxParts = maxPartCount();
            if (shipConstruct.Parts.Count > maxParts)
            {               
                warningMsg = "This vessel has too many parts to be built and launched at the current tech level.\nCurrent maximum is " + maxParts + ", this vessel has " + shipConstruct.Parts.Count + " parts.";
                //ScreenMessages.PostScreenMessage(warningMsg, 15F, ScreenMessageStyle.UPPER_CENTER);
                lockLaunchButton = true;

            } else {
                warningMsg = "";
                if (shipConstruct.Parts.Count > 0)
                    lockLaunchButton = false;
                else
                    lockLaunchButton = true;
            }
        }

        private void OnGUI()
        {
            if (HighLogic.CurrentGame.Mode == Game.Modes.SANDBOX || ResearchAndDevelopment.Instance == null)
                return;

            if (lockLaunchButton)
            {
                EditorLogic.fetch.launchBtn.SetControlState(UIButton.CONTROL_STATE.DISABLED);

                if (warningMsg.Length > 0)
                {
                    GUI.Label(windowRect, warningMsg, labelStyle);
                }
            }
            else
                EditorLogic.fetch.launchBtn.SetControlState(UIButton.CONTROL_STATE.NORMAL);

            
        }

        private int maxPartCount()
        {
            int partsAllowed = 200;
            if (ResearchAndDevelopment.GetTechnologyState("veryHeavyRocketry") == RDTech.State.Available)
                partsAllowed += 25;

            return partsAllowed;
            
        }

        void OnDestroy()
        {
            GameEvents.onEditorShipModified.Remove(new EventData<ShipConstruct>.OnEvent(OnEditorShipModified));               
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TinkerTech
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    class TTPartCountLaunchLimiter : MonoBehaviour
    {
        static bool m_bIsInstantiated = false;

        private bool m_bMainInstance = false;


        private bool lockLaunchButton = false;

        
        private Rect windowRect  = new Rect();
        private string warningMsg = "";

        void Start()
        {
            if (!m_bIsInstantiated)
            {
                m_bIsInstantiated = true;
                m_bMainInstance = true;
                windowRect = new Rect(Screen.width - 350, 50, 350, 100);
                GameEvents.onEditorShipModified.Add(new EventData<ShipConstruct>.OnEvent(OnEditorShipModified));


            }
            else
            {
                Destroy(this);
            }
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
                    GUIStyle labelStyle = HighLogic.Skin.box;
                    labelStyle.normal.textColor = Color.red;
                    labelStyle.alignment = TextAnchor.MiddleCenter;
                    labelStyle.wordWrap = true;

                    GUI.Label(windowRect, warningMsg, labelStyle);

                }
            }
            else
                EditorLogic.fetch.launchBtn.SetControlState(UIButton.CONTROL_STATE.NORMAL);

            
        }

        private int maxPartCount()
        {
            return 200; //TODO Tech-dependent return here
            //if (ResearchAndDevelopment.GetTechnologyState("experimentalRocketry") == RDTech.State.Available)
        }

        void OnDestroy()
        {
            if (m_bMainInstance)
            {
                //GameEvents.onGameSceneLoadRequested.Remove(new EventData<GameScenes>.OnEvent(OnGameSceneLoadRequested));
                GameEvents.onEditorShipModified.Remove(new EventData<ShipConstruct>.OnEvent(OnEditorShipModified));
                
                m_bIsInstantiated = false;
            }
        }




    }
}

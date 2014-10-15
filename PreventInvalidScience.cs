using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TinkerTech
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]//persistent single-instance
    class PreventInvalidScience : MonoBehaviour
    {
        void Start()
        {
            //public void Add(EventData<T, U>.OnEvent evt);
            GameEvents.OnScienceRecieved.Add(OnScienceReceived);
            DontDestroyOnLoad(this);
        }

        void OnDestroy()
        {
            GameEvents.OnScienceRecieved.Remove(OnScienceReceived);
        }

        void OnScienceReceived(float value, ScienceSubject scienceSubject)
        {
            if (!scienceIsValid(scienceSubject))
            {
                {
                    Debug.Log("TinkerTech: received invalid science result:" + scienceSubject.title + "(" + value.ToString() + ") - removing " + value.ToString() + " from the pool");

                    ResearchAndDevelopment.Instance.AddScience(-value, TransactionReasons.Cheating);
                }
            }
        }

        private bool scienceIsValid(ScienceSubject subject)
        {

            //KSC landing science invalid
            if (subject.IsFromSituation(ExperimentSituations.SrfLanded)
                && (subject.title.Contains("LaunchPad") || subject.title.Contains("Runway") || subject.title.Contains("KSC")))
                return false;

            //splashed on non-water biome
            if (subject.IsFromSituation(ExperimentSituations.SrfSplashed) && subject.title.Contains("Kerbin")
                && !subject.title.Contains("Oceans"))
                return false;

            //"Landed" in Oceans
            if (subject.IsFromSituation(ExperimentSituations.SrfLanded) && subject.title.Contains("Kerbin")
                && subject.title.Contains("Oceans"))
                return false;

            return true;
        }
    }

}




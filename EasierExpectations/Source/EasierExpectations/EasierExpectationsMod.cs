using HugsLib;
using HugsLib.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace EasierExpectations
{
    public class EasierExpectationsMod : ModBase
    {
        public override string ModIdentifier => "EasierExpectations";
        
        private SettingHandle<int> minMoodBonus;
        private SettingHandle<int> maxMoodBonus;
        
        private void SetExpectationValues()
        {
            var thoughtStages = GetThoughtStages();
            
            var targetMin = minMoodBonus.Value;
            var targetMax = maxMoodBonus.Value;

            // protect against silly users
            if (targetMax < targetMin)
            {
                var temp = targetMax;
                targetMax = targetMin;
                targetMin = temp;
            }
            
            // calculate new intervals
            var stepSize = (targetMax - targetMin) / (thoughtStages.Count - 1);
            for (var i = 0; i < thoughtStages.Count; i++)
            {
                var newMoodEffect = i * stepSize + targetMin;
                thoughtStages[i].baseMoodEffect = newMoodEffect;
            }
        }

        private void LogExpectationValues()
        {
            var thoughtStages = GetThoughtStages();

            Logger.Message("EasierExpectations logging defs now:");
            Logger.Message($"total count: {thoughtStages.Count}");
            thoughtStages.ForEach(s => Logger.Message($"{s.label}: {s.baseMoodEffect}"));
        }

        private static List<ThoughtStage> GetThoughtStages()
        {
            const string defName = "LowExpectations";
            var expectationDefs = DefDatabase<ThoughtDef>.GetNamed(defName);
            var thoughtStages = expectationDefs.stages.OrderBy(stage => stage.baseMoodEffect)
                .ToList();
            return thoughtStages;
        }

        public override void DefsLoaded()
        {
            minMoodBonus = Settings.GetHandle<int>("minMoodBonus", "EasierExpectations.MinMoodBonus.Title".Translate(), "EasierExpectations.MinMoodBonus.Desc".Translate(), 0, Validators.IntRangeValidator(1, int.MaxValue));
            maxMoodBonus = Settings.GetHandle<int>("maxMoodBonus", "EasierExpectations.MaxMoodBonus.Title".Translate(), "EasierExpectations.MaxMoodBonus.Desc".Translate(), 30, Validators.IntRangeValidator(1, int.MaxValue));

            SetExpectationValues();
            LogExpectationValues();
            Logger.Message("Loaded");
        }


        public override void Initialize()
        {
        }
    }
}
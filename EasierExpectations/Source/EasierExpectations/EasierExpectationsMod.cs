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

        private void LogDefs()
        {
            var expectationDefs = DefDatabase<ExpectationDef>.AllDefs
                .OrderBy(ed => ed.maxMapWealth)
                .ToList();

            Logger.Message("EasierExpectations logging defs now:");
            expectationDefs.ForEach(d => Logger.Message(d.ToString()));
        }

        public override void DefsLoaded()
        {
            LogDefs();
            Logger.Message("Loaded");
        }

        public override void Initialize()
        {
        }
    }
}
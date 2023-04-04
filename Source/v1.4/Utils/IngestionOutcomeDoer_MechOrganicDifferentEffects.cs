﻿using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MechHumanlikes
{
    public class IngestionOutcomeDoer_MechOrganicSplitEffects : IngestionOutcomeDoer
    {
        public HediffDef organicEffect = new HediffDef();
        public ChemicalDef organicTolerance;
        public float organicSeverity = -1f;
        public bool useOrganicGeneToleranceFactors;

        public HediffDef mechanicalEffect = new HediffDef();
        public ChemicalDef mechanicalTolerance;
        public float mechanicalSeverity = -1;

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
        {
            Hediff hediff;
            if (!MHC_Utils.IsConsideredMechanical(pawn))
            {
                hediff = HediffMaker.MakeHediff(organicEffect, pawn);
                float severity = (organicSeverity > 0) ? organicSeverity : organicEffect.initialSeverity;
                AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize_NewTemp(pawn, organicTolerance, ref severity, useOrganicGeneToleranceFactors);
                hediff.Severity = severity;
                pawn.health.AddHediff(hediff);
            }
            else
            {
                hediff = HediffMaker.MakeHediff(mechanicalEffect, pawn);
                float severity = (mechanicalSeverity > 0) ? mechanicalSeverity : mechanicalEffect.initialSeverity;
                AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize_NewTemp(pawn, mechanicalTolerance, ref severity, false);
                hediff.Severity = severity;
                pawn.health.AddHediff(hediff);
            }
        }

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
        {
            if (parentDef.IsDrug && chance >= 1f)
            {
                foreach (StatDrawEntry item in organicEffect.SpecialDisplayStats(StatRequest.ForEmpty()))
                {
                    yield return item;
                }
                foreach (StatDrawEntry item in mechanicalEffect.SpecialDisplayStats(StatRequest.ForEmpty()))
                {
                    yield return item;
                }
            }
        }
    }
}



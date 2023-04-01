﻿using Verse;
using HarmonyLib;
using RimWorld;

namespace MechHumanlikes
{
    internal class ThoughtWorker_AgeReversalDemanded_Patch
    {
        // Mechanical units do not demand age reversal.
        [HarmonyPatch(typeof(ThoughtWorker_AgeReversalDemanded), "ShouldHaveThought")]
        public class CurrentStateInternal_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn p, ref ThoughtState __result)
            {
                if (!__result.Active)
                    return;

                if (Utils.IsConsideredMechanicalSapient(p))
                {
                    __result = ThoughtState.Inactive;
                }
            }
        }
    }
}
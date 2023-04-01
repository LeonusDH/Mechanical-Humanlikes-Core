﻿using Verse;
using HarmonyLib;
using RimWorld;

namespace MechHumanlikes
{
    internal class Pawn_StyleTracker_Patch
    {
        // Non humanlike intelligences don't care about style.
        [HarmonyPatch(typeof(Pawn_StyleTracker), "get_CanDesireLookChange")]
        public class get_CanDesireLookChange_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn ___pawn, ref bool __result)
            {
                __result = __result && !Utils.IsConsideredNonHumanlike(___pawn);
            }
        }
    }
}
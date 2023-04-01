﻿using Verse;
using HarmonyLib;
using RimWorld;

namespace MechHumanlikes
{
    internal class ThoughtUtility_Patch
    {
        // Other pawns don't care about executed non-humanlike intelligences.
        [HarmonyPatch(typeof(ThoughtUtility), "GiveThoughtsForPawnExecuted")]
        public class GiveThoughtsForPawnExecuted_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Pawn victim, PawnExecutionKind kind)
            {
                if (Utils.IsConsideredNonHumanlike(victim))
                    return false;
                else
                    return true;
            }
        }

        // Other pawns don't care about "organs" harvested from non-humanlikes.
        [HarmonyPatch(typeof(ThoughtUtility), "GiveThoughtsForPawnOrganHarvested")]
        public class GiveThoughtsForPawnOrganHarvested_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Pawn victim)
            {
                if (Utils.IsConsideredNonHumanlike(victim))
                    return false;
                else
                    return true;
            }
        }
    }
}
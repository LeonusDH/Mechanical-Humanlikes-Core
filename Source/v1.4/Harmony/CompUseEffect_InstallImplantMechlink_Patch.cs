﻿using System;
using Verse;
using HarmonyLib;
using RimWorld;

namespace MechHumanlikes
{
    internal class CompUseEffect_InstallImplantMechlink_Patch
    {
        // Mechanical units do not suffer gas exposure hediffs like Tox Gas.
        [HarmonyPatch(typeof(CompUseEffect_InstallImplantMechlink), "CanBeUsedBy")]
        [HarmonyPatch(new Type[] { typeof(Pawn), typeof(string) }, new ArgumentType[] { ArgumentType.Normal, ArgumentType.Out })]
        public class CanBeUsedBy_Patch
        {
            [HarmonyPostfix]
            public static void Listener(ref bool __result, Pawn p, ref string failReason)
            {
                if (!__result)
                {
                    return;
                }

                if (Utils.IsConsideredNonHumanlike(p))
                {
                    __result = false;
                    failReason = "MHC_NoSurrogateMechanitors".Translate();
                    return;
                }
            }
        }
    }
}
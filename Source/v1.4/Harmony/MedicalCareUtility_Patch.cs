﻿using Verse;
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace MechHumanlikes
{
    internal class MedicalCareUtility_Patch
    {
        // Patch for mechanical units to have Repair Stims replace medicines graphically for the medical care selector.
        [HarmonyPatch(typeof(MedicalCareUtility), "MedicalCareSelectButton")]
        public class MedicalCareSelectButton_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Rect rect, Pawn pawn)
            {
                // Mechanicals get repair stim graphics.
                if (Utils.IsConsideredMechanical(pawn))
                {
                    Func<Pawn, MedicalCareCategory> getPayload = new Func<Pawn, MedicalCareCategory>(MedicalCareSelectButton_GetMedicalCare);
                    Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>> menuGenerator = new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>>(MedicalCareSelectButton_GenerateMenu);
                    Texture2D buttonIcon;

                    switch ((int)pawn.playerSettings.medCare)
                    {
                        case 0:
                            buttonIcon = MHC_Textures.NoCare;
                            break;
                        case 1:
                            buttonIcon = MHC_Textures.NoMed;
                            break;
                        case 2:
                            buttonIcon = MHC_Textures.RepairStimSimple;
                            break;
                        case 3:
                            buttonIcon = MHC_Textures.RepairStimIntermediate;
                            break;
                        default:
                            buttonIcon = MHC_Textures.RepairStimAdvanced;
                            break;
                    }

                    Widgets.Dropdown(rect, pawn, getPayload, menuGenerator, null, buttonIcon, null, null, null, true);
                    return false;
                }
                // Organics get standard graphic generation.
                else
                {
                    return true;
                }
            }

            private static MedicalCareCategory MedicalCareSelectButton_GetMedicalCare(Pawn pawn)
            {
                return pawn.playerSettings.medCare;
            }

            private static IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>> MedicalCareSelectButton_GenerateMenu(Pawn p)
            {
                for (int i = 0; i < 5; i++)
                {
                    MedicalCareCategory mc = (MedicalCareCategory)i;
                    yield return new Widgets.DropdownMenuElement<MedicalCareCategory>
                    {
                        option = new FloatMenuOption(mc.GetLabel(), delegate
                        {
                            p.playerSettings.medCare = mc;
                        }, MenuOptionPriority.Default, null, null, 0f, null, null),
                        payload = mc
                    };
                }
            }
        }
    }




}
﻿using System;
using UnityEngine;
using Verse;

namespace MechHumanlikes
{
    /*
     *  Settings Extensions and Pawn Selectors courtesy of Simple Sidearms by PeteTimesSix. Without his work, this would have been exceedingly difficult to build!
     */
    public static class ListingExtensions
    {
        private static readonly Color SelectedButtonColor = new Color(.65f, 1f, .65f);
        private static float ButtonTextPadding = 5f;
        private static float AfterLabelMinGap = 10f;

        public static float ColumnGap = 17f;

        public static void CheckboxLabeled(this Listing_Standard instance, string label, ref bool checkOn, string tooltip = null, Action onChange = null)
        {
            var valueBefore = checkOn;
            instance.CheckboxLabeled(label, ref checkOn, tooltip);
            if (checkOn != valueBefore)
            {
                onChange?.Invoke();
            }
        }

        public static void SliderLabeled(this Listing_Standard instance, string label, ref float value, float min, float max, float displayMult = 1, int decimalPlaces = 0, string valueSuffix = "", string tooltip = null, Action onChange = null)
        {
            instance.Label($"{label}: {(value * displayMult).ToString($"F{decimalPlaces}")}{valueSuffix}", tooltip: tooltip);
            var valueBefore = value;
            value = instance.Slider(value, min, max);
            if (value != valueBefore)
            {
                onChange?.Invoke();
            }
        }

        public static void EnumSelector<T>(this Listing_Standard listing, string label, ref T value, string valueLabelPrefix, string valueTooltipPostfix = "_tooltip", string tooltip = null, Action onChange = null) where T : Enum
        {
            string[] names = Enum.GetNames(value.GetType());

            float lineHeight = Text.LineHeight;
            float labelWidth = Text.CalcSize(label).x + AfterLabelMinGap;

            float buttonsWidth = 0f;
            foreach (var name in names)
            {
                string text = (valueLabelPrefix + name).Translate();
                float width = Text.CalcSize(text).x + ButtonTextPadding * 2f;
                if (buttonsWidth < width)
                    buttonsWidth = width;
            }

            bool fitsOnLabelRow = (((buttonsWidth * names.Length) + labelWidth) < (listing.ColumnWidth));
            float buttonsRectWidth = fitsOnLabelRow ?
                listing.ColumnWidth - (labelWidth + 1f) : //little extra to handle naughty pixels
                listing.ColumnWidth;

            int rowNum = 0;
            int columnNum = 0;
            int maxColumnNum = 0;
            foreach (var name in names)
            {
                if ((columnNum + 1) * buttonsWidth > buttonsRectWidth)
                {
                    columnNum = 0;
                    rowNum++;
                }
                float x = (columnNum * buttonsWidth);
                float y = rowNum * lineHeight;
                columnNum++;
                if (rowNum == 0 && maxColumnNum < columnNum)
                    maxColumnNum = columnNum;
            }
            rowNum++; //label row
            if (!fitsOnLabelRow)
                rowNum++;

            Rect wholeRect = listing.GetRect((float)rowNum * lineHeight);

            if (!tooltip.NullOrEmpty())
            {
                if (Mouse.IsOver(wholeRect))
                {
                    Widgets.DrawHighlight(wholeRect);
                }
                TooltipHandler.TipRegion(wholeRect, tooltip);
            }

            Rect labelRect = wholeRect.TopPartPixels(lineHeight).LeftPartPixels(labelWidth);
            GUI.color = Color.white;
            Widgets.Label(labelRect, label);

            Rect buttonsRect = fitsOnLabelRow ?
                wholeRect.RightPartPixels(buttonsRectWidth) :
                wholeRect.BottomPartPixels(wholeRect.height - lineHeight);

            buttonsWidth = buttonsRectWidth / (float)maxColumnNum;

            rowNum = 0;
            columnNum = 0;
            foreach (var name in names)
            {
                if ((columnNum + 1) * buttonsWidth > (buttonsRectWidth + 2f))
                {
                    columnNum = 0;
                    rowNum++;
                }
                float x = (columnNum * buttonsWidth);
                float y = rowNum * lineHeight;
                columnNum++;
                string buttonText = (valueLabelPrefix + name).Translate();
                var enumValue = (T)Enum.Parse(value.GetType(), name);
                GUI.color = value.Equals(enumValue) ? SelectedButtonColor : Color.white;
                var buttonRect = new Rect(buttonsRect.x + x, buttonsRect.y + y, buttonsWidth, lineHeight);
                if (valueTooltipPostfix != null)
                    TooltipHandler.TipRegion(buttonRect, (valueLabelPrefix + name + valueTooltipPostfix).Translate());
                bool clicked = Widgets.ButtonText(buttonRect, buttonText);
                if (clicked && !value.Equals(enumValue))
                {
                    value = enumValue;
                    onChange?.Invoke();
                }
            }

            listing.Gap(listing.verticalSpacing);
            GUI.color = Color.white;
        }

        public static Listing_Standard BeginHiddenSection(this Listing_Standard instance, out float maxHeightAccumulator)
        {
            Rect rect = instance.GetRect(0);
            rect.height = 30000f;
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(rect);
            maxHeightAccumulator = 0f;
            return listing_Standard;
        }

        public static void NewHiddenColumn(this Listing_Standard instance, ref float maxHeightAccumulator)
        {
            if (maxHeightAccumulator < instance.CurHeight)
                maxHeightAccumulator = instance.CurHeight;
            instance.NewColumn();
        }

        public static void EndHiddenSection(this Listing_Standard instance, Listing_Standard section, float maxHeightAccumulator)
        {
            instance.GetRect(Mathf.Max(section.CurHeight, maxHeightAccumulator));
            section.End();
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SUHScripts
{
    using static MathHelper;

    public static class Radial
    {
        public static RadialSectionData GetRadialSectionData(int sections)
        {
            float sectionWidth = (360 / sections);
            float sectionStart = 360 - sectionWidth / 2;

            List<Vector2> ranges =
                Enumerable.Range(0, sections)
                .Select(index => RadialSectionRange(index, sectionStart, sectionWidth))
                .ToList();

            return new RadialSectionData(sections, sectionWidth, sectionStart, ranges);
        }

        public static Vector2 RadialSectionRange(int sectionIndex, float SectionStart, float SectionWidth)
        {
            float startPosition = NormalizeAngleDeg360(SectionStart + (sectionIndex * SectionWidth));
            float endPosition = NormalizeAngleDeg360(SectionStart + SectionWidth + (sectionIndex * SectionWidth));
            return new Vector2(startPosition, endPosition);
        }

        public static int AngleSectionIndex(float angle, RadialSectionData data)
        {
            var rangeFound = data.Ranges
            .Where(range => ConfirmInRange(angle, range))
            .First();

            return data.Ranges.IndexOf(rangeFound);
        }

        public static bool ConfirmInRange(float angle, Vector2 range)
        {
            bool shouldConvert = range.y < range.x;

            float outputAngle =
                shouldConvert ?
                angle < range.x ? angle + 360 : angle 
                : angle;

            Vector2 outputRange =
                shouldConvert ?
                range.WithY(y => y + 360) : range;

            return outputRange.x <= outputAngle && outputRange.y > outputAngle; 
        }   

        public static List<Vector3> RadialPositions(Vector3 radialAxis, Vector3 offsetVector, RadialSectionData data, Quaternion sourceIdentity)
        {
            return Enumerable.Range(0, data.Ranges.Count)
                .Select(index => RadialPosition(index, data, radialAxis, offsetVector, sourceIdentity))
                .ToList();
        }

        public static Vector3 RadialPosition(int index, RadialSectionData data, Vector3 radialAxis, Vector3 offsetVector, Quaternion sourceIdentity)
        {
            Vector3 rot = radialAxis * (data.Ranges[index].x + data.SectionWidth / 2);
            Quaternion turn = sourceIdentity * Quaternion.Euler(rot);
            return turn * offsetVector;
        }
    }

    public struct RadialSectionData
    {
        public readonly int Sections;
        public readonly float SectionWidth;
        public readonly float SectionStart;
        public readonly List<Vector2> Ranges;

        string m_ranges;
        public RadialSectionData(int sections, float sectionWidth, float sectionStart, List<Vector2> ranges)
        {
            this.Sections = sections;
            this.SectionWidth = sectionWidth;
            this.SectionStart = sectionStart;
            this.Ranges = ranges;
            m_ranges =
                ranges
                .Aggregate("", (current, next) => current.ToString() + next.ToString());
        }

        public override string ToString() =>
            $"Sections: {Sections} SectionsWidth {SectionWidth} SectionStart {SectionStart} Ranges {m_ranges}";
    }
}
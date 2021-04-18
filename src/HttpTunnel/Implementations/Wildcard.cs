using System;

namespace HttpTunnel.Implementations
{
    /// <summary>
    /// This class matches string with wildcard patterns.
    /// </summary>
    public static class Wildcard
    {
        /// <summary>
        /// The asterisk character that matches any text.
        /// </summary>
        private const string Asterisk = "*";

        /// <summary>
        /// The asterisk delimiter.
        /// </summary>
        private static readonly string[] AsteriskDelimiter = new string[] { Asterisk };

        /// <summary>
        /// Check if a string contains wildcard
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>true if the string contains wildcard; otherwise, false.</returns>
        public static bool ContainsWildcard(string input)
        {
            return input != null && input.Contains(Wildcard.Asterisk);
        }

        /// <summary>
        /// Check if the input string matches wildcard pattern
        /// </summary>
        /// <param name="input">input string to be checked</param>
        /// <param name="pattern">The wildcard pattern</param>
        /// <param name="comparisonType">comparison type</param>
        /// <returns>true if the input string matches pattern with wildcard; otherwise, false.</returns>
        public static bool IsMatch(string input, string pattern, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (input == null)
            {
                input = string.Empty;
            }

            if (pattern == null)
            {
                pattern = string.Empty;
            }

            if (pattern.Length == 0)
            {
                // Empty pattern only matches empty input.
                return input.Length == 0;
            }

            bool startsWithAsterisk = pattern.StartsWith(Asterisk);
            bool endsWithAsterisk = pattern.EndsWith(Asterisk);
            string[] parts = pattern.Split(AsteriskDelimiter, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
            {
                // Pattern only contain asterisk
                return true;
            }
            else if (parts.Length == 1 && !startsWithAsterisk && !endsWithAsterisk)
            {
                // Pattern doesn't contain asterisk
                return pattern.Equals(input, comparisonType);
            }

            // Pattern contains both text and asterisk
            int startPosition = 0;
            int endPosition = input.Length - 1;
            int partStartIndex = 0;
            int partEndIndex = parts.Length - 1;

            if (!pattern.StartsWith(Asterisk))
            {
                // "abc*" pattern only matches input string starts with "abc"
                if (input.StartsWith(parts[partStartIndex], comparisonType))
                {
                    startPosition += parts[partStartIndex].Length;
                    partStartIndex++;
                }
                else
                {
                    return false;
                }
            }

            if (!pattern.EndsWith(Asterisk))
            {
                // "*abc" pattern only matches input string ends with "abc"
                if (input.EndsWith(parts[partEndIndex], comparisonType))
                {
                    endPosition -= parts[partEndIndex].Length;
                    partEndIndex--;
                }
                else
                {
                    return false;
                }
            }

            // Now the remaining pattern becomes "*" or "*...*". To match the remaining input string, we just need to check
            // whether remaining input string contains all remaining pattern parts in sequence.
            return ContainAllParts(input, startPosition, endPosition, parts, partStartIndex, partEndIndex, comparisonType);
        }

        /// <summary>
        /// Check if the input contains all parts in sequence.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="startPosition">The start position of the input string.</param>
        /// <param name="endPosition">The end position of the input string.</param>
        /// <param name="parts">The parts to test.</param>
        /// <param name="partStartIndex">Start index of the parts.</param>
        /// <param name="partEndIndex">End index of the parts.</param>
        /// <param name="comparisonType">The comparsion type.</param>
        /// <returns>true if the input string contains all parts in the sequence; otherwise, false.</returns>
        private static bool ContainAllParts(
            string input,
            int startPosition,
            int endPosition,
            string[] parts,
            int partStartIndex,
            int partEndIndex,
            StringComparison comparisonType)
        {
            int position = startPosition; // Current position in the input string
            for (int partIndex = partStartIndex; partIndex <= partEndIndex; partIndex++)
            {
                var part = parts[partIndex];
                int index = input.IndexOf(part, position, comparisonType);
                if (index == -1 || index > endPosition - part.Length + 1)
                {
                    // Part is not found .
                    return false;
                }

                position = index + part.Length;
            }

            return true;
        }
    }
}
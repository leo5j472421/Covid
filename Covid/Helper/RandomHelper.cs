using System;
using System.Collections.Generic;
using System.Linq;
using Covid.Enums;

namespace Covid.Helper
{
    public class RandomHelper
    {
        private static Dictionary<EnumRandomStringType, string> randomCharPool = new Dictionary<EnumRandomStringType, string>()
        {
            {EnumRandomStringType.NumberCharSet, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"},
            {EnumRandomStringType.CharSet, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"},
            {EnumRandomStringType.Sso, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"}
        };

        public static string RandomString(EnumRandomStringType type, int length)

        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            string chars = randomCharPool[type];
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static int GetRandomNumber(int min, int max)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            return random.Next(min, max);
        }
    }
}
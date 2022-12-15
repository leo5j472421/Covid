using System;

namespace Covid.Enums
{
    [Flags]
    public enum EnumBaccaratResult
    {
        BankerWin = 1,
        PlayerWin = 2,
        Tie = 3,
        BankerPair = 4,
        PlayerPair = 8,
    }
}
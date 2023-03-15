using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_TacticalMagazines
{
    internal struct TextPack
    {
        public string full, almostFull, moreThanHalf, aboutHalf, lessThanHalf, almostEmpty, empty;
    }

    internal static class TextPacks
    {
        public static readonly TextPack[] textPacks = {
            new TextPack()
            {
                full = "Full",
                almostFull = "Almost Full",
                moreThanHalf = "More Than Half",
                aboutHalf = "About Half",
                lessThanHalf = "Less Than Half",
                almostEmpty = "Almost Empty",
                empty = "Empty"
            },
            new TextPack()
            {
                full = "Full",
                almostFull = "Most Of Them",
                moreThanHalf = "Quite A Few",
                aboutHalf = "About Half",
                lessThanHalf = "Some",
                almostEmpty = "Not That Many",
                empty = "Empty"
            }
        };

        public static TextPack original = textPacks[0];
    }
}
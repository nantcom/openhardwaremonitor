using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenHardwareMonitor
{
    public static class Bitmask
    {
        private static Dictionary<string, int> _MaskCache = new Dictionary<string, int>();
        private static int CreateMask( int length, int start, int end )
        {
            int maskValue;
            var key = length.ToString() + start.ToString() + end.ToString();
            if (_MaskCache.TryGetValue( key, out maskValue))
            {
                return maskValue;
            }

            bool[] mask = new bool[length];

            start = (length - 1 ) - start;
            end = (length - 1) - end;

            // start
            for (int i = start; i < end; i++)
            {
                mask[i] = true;
            }

            BitArray bMask = new BitArray(mask);
            
            int[] array = new int[1];
            bMask.CopyTo(array, 0);

            _MaskCache[key] = array[0];

            return array[0];
        }

        public static long GetValue( uint value, int start, int end )
        {
            if (end > start)
            {
                var t = end;
                end = start;
                start = t;
            }

            int mask = Bitmask.CreateMask(32, start, end);
            var result = (value & mask) >> (31- start);
            
            return result;

            /*
            A simple real example in graphics programming is that a 16-bit pixel is represented as follows:
              bit | 15| 14| 13| 12| 11| 10| 9 | 8 | 7 | 6 | 5 | 4 | 3 | 2 | 1  | 0 |
                  |       Blue        |         Green         |       Red          |
                  
            To get at the green value you would do this:
             #define GREEN_MASK  0x7E0
             #define GREEN_OFFSET  5

             // Read green
             uint16_t green = (pixel & GREEN_MASK) >> GREEN_OFFSET;


            Explanation

            In order to obtain the value of green ONLY, which starts at offset 5 
            and ends at 10 (i.e. 6-bits long), you need to use a (bit) mask, 
            which when applied against the entire 16-bit pixel, 
            will yield only the bits we are interested in.

            #define GREEN_MASK  0x7E0
            
            The appropriate mask is 0x7E0 which in binary is 0000011111100000 (which is 2016 in decimal).
            uint16_t green = (pixel & GREEN_MASK) ...;


            To apply a mask, you use the AND operator (&).
            uint16_t green = (pixel & GREEN_MASK) >> GREEN_OFFSET;



            */
        }
    }
}

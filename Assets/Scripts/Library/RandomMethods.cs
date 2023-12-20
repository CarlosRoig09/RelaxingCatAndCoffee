using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace personalLibrary
{
    public static class RandomMethods
    {
        public static int ReturnARandomObject(float[] RateAperance, float dropNothingChane, int max, int min)
        {
            float minRange = 0;
            float maxRange = 0;
            var random = Random.Range(minRange, SetMaxValueOfRandom(RateAperance) + dropNothingChane);
            for (var i = min; i < max; i++)
            {
                //Debug.Log(random);
                if (random >= minRange && random <= (maxRange += RateAperance[i] / RateAperance.Length))
                {
                    return i;
                }
                else
                    minRange = maxRange;
            }
            return -1;
        }

        public static float SetMaxValueOfRandom(float[] RateAperance)
        {
            float totalValue = 0;
            foreach (var Rate in RateAperance)
            {
                totalValue += Rate / RateAperance.Length;
            }
            return totalValue;
        }
    }
}

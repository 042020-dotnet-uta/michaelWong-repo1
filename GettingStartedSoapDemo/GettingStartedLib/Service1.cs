using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace GettingStartedLib
{
    public class FridgeService : IFridge
    {
        public static Dictionary<string, int> fridge = new Dictionary<string, int>();
        public int Add(string fruit, int count)
        {
            if (!fridge.ContainsKey(fruit))
            {
                fridge.Add(fruit, count);
                return count;
            }
            else
            {
                fridge[fruit] += count;
                return fridge[fruit];
            }
        }

        public int Subtract(string fruit, int count)
        {
            if (!fridge.ContainsKey(fruit))
            {
                return 0;
            }
            else
            {
                fridge[fruit] -= count;
                return fridge[fruit];
            }
        }

        public int Get(string fruit)
        {
            if (fridge.ContainsKey(fruit))
                return fridge[fruit];
            return 0;
        }
    }
}
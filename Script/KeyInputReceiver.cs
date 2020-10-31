using System;
using System.Collections.Generic;
using UnityEngine;

public static class KeyInputReceiver
{
    private static readonly int limit = 10;
    private static Dictionary<KeyCode, bool> keyMap = new Dictionary<KeyCode, bool>();
    private static Dictionary<KeyCode, int> counter = new Dictionary<KeyCode, int>();

    public static void Update()
    {
        if (Input.anyKey || Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    keyMap[keyCode] = true;
                }
                else keyMap[keyCode] = false;
            }
        }
        else keyMap.Clear();
    }

    public static bool GetKeyLongDown(KeyCode keyCode)
    {
        bool current;

        if (keyMap.TryGetValue(keyCode, out current) && current)
        {
            int cnt;

            if (counter.TryGetValue(keyCode, out cnt))
            {
                cnt++;
                counter[keyCode] = cnt;

                if (cnt >= limit) return true;
                else return false;
            }
            else
            {
                counter[keyCode] = 1;
                return true;
            }
        }
        else
        {
            if(counter.ContainsKey(keyCode))
            {
                counter.Remove(keyCode);
            }

            return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldData
{
    static int seed_ = 47;

    static bool loaded_ = false;

    public static bool loaded
    {
        get { return loaded_; }
        set { loaded_ = value; }
    }
    public static int seed
    {
        get { return seed_; }
        set { seed_ = value; }
    }
}

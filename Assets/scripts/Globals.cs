using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Globals
{
    public static int lvl = 0; //player(standalone) knows it the first load
    public static bool firstLoad = true;
    public static bool levelLocked = false;
    public static bool isNextLevel = false;
    public static string statistics = "";
    public static string player = "";
    public static string age = "";
    public static float timer = 0f;

    internal static void addAct(string act)
    {
        if (statistics != "") statistics += "$";
        statistics += "[" + timer.ToString() + "] " + act;
    }

}


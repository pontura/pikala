using UnityEngine;
using System.Collections;
using Fabric.Answers;

public static class Metrics {
    
    public static void OnView(string name)
    {
        Answers.LogContentView( name, "onView", name + "_onView");
    }

}

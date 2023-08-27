using UnityEngine;
using UnityEditor;

public class DeveloperTools
{
    [MenuItem("Developer/Clear PlayerPrefs", false, 1)]
    private static void NewMenuOption()
    {
        PlayerPrefs.DeleteAll();
    }
}

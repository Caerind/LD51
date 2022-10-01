using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class HideMetaFilesMenu
{
    [MenuItem("Custom/Hide Meta Files")]
    private static void HideMetaFiles()
    {
        string commandLine = @"attrib +h " + Application.dataPath.Replace(@"/", @"\") + "/*.meta /s";

        Shell.ExecuteCommand(commandLine);
    }
}
#endif // UNITY_EDITOR
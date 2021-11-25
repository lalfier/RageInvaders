using UnityEngine;
using UnityEditor;

public class DeletePlayerPrefsScript : EditorWindow
{
    /// <summary>
    /// Clear saved player prefs in editor (window menu).
    /// </summary>
    [MenuItem("Window/Delete PlayerPrefs (All)")]
    static void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}

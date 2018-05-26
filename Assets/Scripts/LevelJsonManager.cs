using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LevelJsonManager {

    public static void SaveInJson(Level level, string LevelName)
    {
        File.WriteAllText(Application.dataPath + "/Resources" + "/Levels/" + LevelName + ".txt", JsonUtility.ToJson(level));
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public static Level LoadFromJson(int level)
    {
        string temp = Resources.Load<TextAsset>("Levels/Level" + level).ToString();
        Level levelParsed = JsonUtility.FromJson<Level>(temp);
        return levelParsed;
    }
}

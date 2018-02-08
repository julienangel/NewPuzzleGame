using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelJsonManager {

    public static void SaveInJson(Level level, string LevelName)
    {
        File.WriteAllText(Application.dataPath + "/Resources" + "/" + LevelName + ".txt", JsonUtility.ToJson(level));
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public static Level LoadFromJson(int level)
    {
        string temp = Resources.Load<TextAsset>("level" + level).ToString();
        Level levelParsed = JsonUtility.FromJson<Level>(temp);
        return levelParsed;
    }
}

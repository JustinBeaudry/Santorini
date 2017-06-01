using UnityEngine;
using System.IO;

public class IO
{
  public static object ReadFile(string path)
  {
    string filePath = Path.Combine(Application.streamingAssetsPath, path);
    if (!File.Exists(filePath))
    {
      return null;
    }
    return JsonUtility.FromJson<object>(File.ReadAllText(filePath));
  }
}
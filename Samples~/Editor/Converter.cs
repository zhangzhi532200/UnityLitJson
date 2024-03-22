using LitJson;
using System.IO;
using UnityEditor;
using UnityEngine;
using Unity.Plastic.Newtonsoft.Json;
using System.Collections.Generic;

public class Converter
{
    private static readonly string JsonFileName = "JsonText.json";
    private static readonly string JsonFilePath = Path.Combine(Application.dataPath, "UnityLitJson", JsonFileName).Replace('\\', '/');
    private static string FileJSONString;
    private static JsonData FileJSONData;

    static Converter()
    {
        if (!File.Exists(JsonFilePath))
        {
            Debug.LogError($"{JsonFileName}文件不存在!");
            return;
        }
        FileJSONString = File.ReadAllText(JsonFilePath);
        FileJSONData = JsonMapper.ToObject<JsonData>(FileJSONString);
    }

    [MenuItem("Converter/序列化为JsonString")]
    public static void ToJsonString()
    {
        string json = FileJSONData.ToJson();
        //不带格式缩进
        Debug.Log($"<color={"yellow"}>{json}</color>");
        //带格式缩进
        Debug.Log(JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json), Formatting.Indented));
    }

    /// <summary>
    /// user结构体
    /// </summary>
    /*
     {
      "users": [
        {
          "id": 1,
          "name": "Alice",
          "age": 25,
          "email": "alice@example.com",
          "address": {
            "street": "123 Main St",
            ...
          },
          "interests": [
            "reading",
            ...
          ]
        },
        ...
    }
     */
    [MenuItem("Converter/序列化为JsonObject")]
    public static void Trans2ScriptableObject()
    {
        string Title = "JsonObject：";
        JsonData jsonData = JsonMapper.ToObject(FileJSONString);
        Debug.Log($"<color={"red"}>{Title}</color><color={"yellow"}>{jsonData.ToJson()}</color>");
        IList<JsonData> JSONList = jsonData["users"].ValueAsArray();
        foreach (JsonData item in JSONList)
        {
            ColorLog(item["id"].ValueAsInt(), "green");
            ColorLog(item["name"].ValueAsString(), "green");
            ColorLog(item["email"].ValueAsString(), "green");
            IList<JsonData> interestsList = item["interests"].ValueAsArray();
            foreach (string interests in interestsList)
            {
                ColorLog(interests, "green");
            }
        }

    }

    public static void ColorLog(object obj, string color)
    {
        Debug.Log($"<color={color}>{obj}</color>");
    }
}

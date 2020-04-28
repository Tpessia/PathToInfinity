using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class MaxScore
{
    private static MaxScoreClass MaxScoreObj { get; set; } = new MaxScoreClass { Score = "0" };

    public static void Save(string score)
    {
        MaxScoreObj.Score = score;

        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd"); //you can call it anything you want
        bf.Serialize(file, MaxScoreObj);
        file.Close();
    }

    public static string Load()
    {
        if(File.Exists(Application.persistentDataPath + "/savedGames.gd")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            MaxScoreObj = (MaxScoreClass)bf.Deserialize(file);
            file.Close();

            return MaxScoreObj.Score;
        }

        return "0";
    }

    [System.Serializable]
    public class MaxScoreClass
    {
        public string Score;
    }
}
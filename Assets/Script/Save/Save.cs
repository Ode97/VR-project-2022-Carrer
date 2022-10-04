using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;


public class Save : MonoBehaviour {
    public static void saveData(Data g){
        
        string destination = Application.persistentDataPath + "/" + "_map.dat";
        Debug.Log(destination);
        FileStream file;

        if(File.Exists(destination)) 
            file = File.OpenWrite(destination);
        else 
            file = File.Create(destination);


        Data data = g;
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public static Data loadData(){
        string destination = Application.persistentDataPath + "/" + "_map.dat";
        FileStream file;

        if(File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            //Debug.LogError("Save File not found");
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        Data data = (Data) bf.Deserialize(file);
        
        file.Close();

        GameManager.GM().SetLoad(data.wood, data.people, data.jobs, data.entertainment, data.workerNum);
        GameManager.GM().data = data;
        return data;        
    }
    

    public static void DeleteFile(){
        string destination = Application.persistentDataPath + "/save.dat";

        if(File.Exists(destination)) File.Delete(destination);
        else
        {
            //Debug.LogError("File not found");
            return;
        }
    }

    public static bool MainMessage(){
        string destination = Application.persistentDataPath + "/openedOnce.dat";

        if(File.Exists(destination)) {
            return true;
        }
        else {
            FileStream file;
            file = File.Create(destination);
            bool data = true;
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, data);
            file.Close();
            return false;
        }
    }
}

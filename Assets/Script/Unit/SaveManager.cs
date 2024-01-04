using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace GameSave
{
    public class SaveManager : Singleton<SaveManager>
    {
        public string SavePath
        {
            get { return Application.persistentDataPath; }
        }
        public string GetSaveFileName(int slot)
        {
            return "save_slot_" + slot + ".dat";
        }

        public string GetFullSaveFileName(int slot)
        {
            return this.SavePath + '/' + GetSaveFileName(slot);
        }

        public void SaveGamePlayData()
        {

        }
        public void SaveData(int slot, SaveData saveData)
        {
           string path = GetFullSaveFileName(slot);
            try
            {
                //object serializedData = saveData.GetSerializedData();

                //using (FileStream fileStream = File.Create(path))
                //{
                //    new BinaryFormatter().Serialize(fileStream, saveData);
                //}
                //using (FileStream fileStream = File.Open(path, FileMode.Open))
                //{
                //    new BinaryFormatter().Deserialize(fileStream);
                //}
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fileStream = File.Create(path);
                formatter.Serialize(fileStream, saveData);
                fileStream.Close();
                Debug.Log("Path : " + path);
                Debug.Log("Save");
            }
            catch(Exception message)
            {
                Debug.LogError("Error");
                Debug.LogError(message);
            }
        }

        public SaveData LoadData(int slot)
        {
            string path = GetFullSaveFileName(slot);
            if (File.Exists(path))
            {
                //using (FileStream fileStream = File.Open(path, FileMode.Open))
                //{
                //    file = (SaveData)binaryFormatter.Deserialize(fileStream);
                //}
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(path, FileMode.Open);
                if (fileStream != null)
                    Debug.Log("로드파일 존재");
                SaveData file = (SaveData)binaryFormatter.Deserialize(fileStream);
                //SaveData saveData = null;
                //saveData.LoadFromSerializedData(file);
                //Debug.Log("로드한 데이터 : " + file.GetData("hp").GetIntSelf());
                fileStream.Close();
                return file;
            }
            //try
            //{
            //    BinaryFormatter binaryFormatter = new BinaryFormatter();
            //    if (File.Exists(path))
            //    {
            //        //using (FileStream fileStream = File.Open(path, FileMode.Open))
            //        //{
            //        //    file = (SaveData)binaryFormatter.Deserialize(fileStream);
            //        //}
            //        FileStream fileStream = new FileStream(path, FileMode.Open);
            //        if(fileStream != null)
            //        Debug.Log("로드파일 존재");
            //        SaveData file = (SaveData)binaryFormatter.Deserialize(fileStream);
            //        //SaveData saveData = null;
            //        //saveData.LoadFromSerializedData(file);
            //        Debug.Log("로드한 데이터 : " + file.GetData("hp").GetIntSelf());
            //        fileStream.Close();
            //        return file;
            //    }
            //}
            //catch (Exception message)
            //{
            //    Debug.LogError("Error : " + message);
            //}
            return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
/*
[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;
    public Vector3 playerRot;

    public List<int> invenArrayNumber = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>();
}

public class SaveNLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/saveFile.txt";

    private PlayerController thePlayer;
    private Inventory theInven;

    void Start()
    {
        //"/Saves/"; ?????? ????
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";

        //?????? ???? ?????? SAVE_DATA_DIRECTORY ????
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);

    }

    public void SaveData()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        theInven = FindObjectOfType<Inventory>();

        //Player ???? ????
        saveData.playerPos = thePlayer.transform.position;
        saveData.playerRot = thePlayer.transform.eulerAngles;

        //Slot[] slots = theInven.GetSlots();
        //for(int i = 0; i < slots.Length; i++)
        //{
        //    if(slots[i].item != null)
        //    {
        //        saveData.invenArrayNumber.Add(i);
        //        saveData.invenItemName.Add(slots[i].item.itemName);
        //        saveData.invenItemNumber.Add(slots[i].item.itemCount);
        //    }
        //}

        //Player ?????? json?? ????????
        string json = JsonUtility.ToJson(saveData);

        //?????????? ?? ???? ???? ?????? ????????
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

        Debug.Log("????????");
        Debug.Log(json);
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            thePlayer = FindObjectOfType<PlayerController>();
            theInven = FindObjectOfType<Inventory>();

            //load ???? ?????? save ???????? ????
            thePlayer.transform.position = saveData.playerPos;
            thePlayer.transform.eulerAngles = saveData.playerRot;

            for (int i = 0; i < saveData.invenItemName.Count; i++)
            {
                //theInven.LoadToInven(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNumber[i]);
            }

            Debug.Log("???? ????");
        }
        else
        {
            Debug.Log("?????? ?????? ????????.");
        }

    }
}*/

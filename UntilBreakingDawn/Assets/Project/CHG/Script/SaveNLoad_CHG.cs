using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;
    public Vector3 playerRot;

    public List<int> invenArrayNumber = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>();
}

public class SaveNLoad_CHG : MonoBehaviour
{
    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/saveFile.txt";

    private PlayerController_CHG thePlayer;
    private Inventory_CHG theInven;

    void Start()
    {
        //"/Saves/"; 파일에 저장
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";

        //경로에 뭐가 없으면 SAVE_DATA_DIRECTORY 생성
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);

    }

    public void SaveData()
    {
        thePlayer = FindObjectOfType<PlayerController_CHG>();
        theInven = FindObjectOfType<Inventory_CHG>();

        //Player 위치 저장
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

        //Player 위치를 json화 시키는것
        string json = JsonUtility.ToJson(saveData);

        //텍스트들을 이 경로 파일 위치에 저장한다
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

        Debug.Log("저장완료");
        Debug.Log(json);
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            thePlayer = FindObjectOfType<PlayerController_CHG>();
            theInven = FindObjectOfType<Inventory_CHG>();

            //load 하면 위치가 save 된곳으로 이동
            thePlayer.transform.position = saveData.playerPos;
            thePlayer.transform.eulerAngles = saveData.playerRot;

            for (int i = 0; i < saveData.invenItemName.Count; i++)
            {
                //theInven.LoadToInven(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNumber[i]);
            }

            Debug.Log("로그 완료");
        }
        else
        {
            Debug.Log("세이브 파일이 없습니다.");
        }

    }
}

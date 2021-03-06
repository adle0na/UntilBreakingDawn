using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftTab
{
    public string craftsName;
    public Craft[] crafts;        // 제작 탭들이 들어갈 곳
}

[System.Serializable]
public class Craft
{
    public string craftName;
    public GameObject go_Prefab;        // 실제 설치될 프리펩
    public GameObject go_PreviewPrefab; // 미리보기 프리펩
    // 아이템
    public Item item;
}

public class CraftManual : MonoBehaviour
{
    public  bool isActivated = false;
    public  bool isPreviewActivated = false;

    [SerializeField]
    private GameObject go_BaseUI;   // 기본 베이스 UI
    [SerializeField]
    private GameObject[] craft_TabUI;    // 탭 베이스 UI

    [SerializeField]
    private CraftTab[] craftTab;    // 왼쪽 탭들 모음

    //private Craft[] craftWall;      // 슬롯

    private GameObject go_Preview;  // 미리보기 프리펩을 담을 변수
    private GameObject go_Prefab;   // 실제 생성될 프리펩을 담을 변수

    [SerializeField]
    private Transform tf_Player;    // 플레이어 위치

    // Raycast 필요 변수 선언
    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float range;

    // 탭 번호
    private int TabSelectNumber;

    // 인벤토리
    public Inventory inven;

    [SerializeField]
    private GameObject go_SlotsParent;
    private Slot[] slots;

    private Item items;

    // 재료 판별기
    private int woodCount               = 0;
    private int rookCount               = 0;
    private int metalCount              = 0;
    private int meatCount               = 0;
    private int cookMeatCount           = 0;
    private int AssaultRifleBulletCount = 0;


    private void Awake()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    private void Start()
    {
        for (int i = 0; i < craft_TabUI.Length; i++)
        {
            craft_TabUI[i].transform.Find("SlotActive").gameObject.SetActive(false);
        }

        craft_TabUI[0].transform.Find("SlotActive").gameObject.SetActive(true);
    }

    // 제작 탭 클릭
    public void TabClick(int _TabNumber)
    {
        if (craftTab[_TabNumber].craftsName == $"제작탭_{_TabNumber}")
        {
            for (int i = 0; i < craft_TabUI.Length; i++)
            {
                craft_TabUI[i].transform.Find("SlotActive").gameObject.SetActive(false);
            }

            craft_TabUI[_TabNumber].transform.Find("SlotActive").gameObject.SetActive(true);
        }
        TabSelectNumber = _TabNumber;
    }

    // 슬롯 클릭
    public void SlotClick(int _slotNumber)
    {
        items = craftTab[TabSelectNumber].crafts[_slotNumber].item;

        if (go_Prefab == null && go_Preview == null)
        {
            if (items != null && items.itemType != Item.ItemType.Equipment)
            {
                ItemCheck();
                ItemUse();
            }
            else
            {
                go_Prefab = craftTab[TabSelectNumber].crafts[_slotNumber].go_Prefab;

                ObjectCheck();
                ObjectUse(_slotNumber); 
            }
        }
    }

    private void ItemCheck()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            switch (items.name)
            {
                case "Cooked_Meat":
                    meatCount = inven.GetItemCount("RawMeat");
                    cookMeatCount = inven.GetItemCount("CookedMeat");
                    break;
                case "magazineMain":
                    metalCount = inven.GetItemCount("Metal");
                    AssaultRifleBulletCount = inven.GetItemCount("magazineMain");
                    Debug.Log(metalCount + " / " + AssaultRifleBulletCount);
                    break;
                default:
                    break;
            }
        }
    }

    private void ItemUse()
    {
        switch (items.name)
        {
            case "Cooked_Meat":
                if (meatCount >= 1)
                {
                    int meatMiuns = 1;

                    inven.SetItemCount("RawMeat", meatMiuns);
                    inven.AcquireItem(items);
                }
                break;
            case "magazineMain":
                if (metalCount >= 1)
                {
                    int metalMiuns = 1;

                    inven.SetItemCount("Metal", metalMiuns);
                    inven.AcquireItem(items);
                }
                break;
            default:
                break;
        }
    }

    private void ObjectCheck()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            switch (go_Prefab.name)
            {
                case "WoodWall":
                    woodCount = inven.GetItemCount("Log");
                    break;
                case "StoneWall":
                    rookCount = inven.GetItemCount("Rook");
                    break;
                case "ExplosiveBarrel_KHJ_Pivot":
                    metalCount = inven.GetItemCount("Metal");
                    rookCount = inven.GetItemCount("Rook");
                    break;
                default:
                    break;
            }
        }
    }

    private void ObjectUse(int _slotNumber)
    {
        switch (go_Prefab.name)
        {
            case "WoodWall":
                if (woodCount >= 4)
                {
                    go_Preview = Instantiate(craftTab[TabSelectNumber].crafts[_slotNumber].go_PreviewPrefab,
                                                    tf_Player.position + tf_Player.forward,
                                                    Quaternion.identity);

                    int woodMinus = 4;

                    inven.SetItemCount("Log", woodMinus);

                    isPreviewActivated = true;
                    go_BaseUI.SetActive(false);
                }
                break;
            case "StoneWall":
                if (rookCount >= 4)
                {
                    go_Preview = Instantiate(craftTab[TabSelectNumber].crafts[_slotNumber].go_PreviewPrefab,
                                                    tf_Player.position + tf_Player.forward,
                                                    Quaternion.identity);

                    int rookMinus = 4;

                    inven.SetItemCount("Rook", rookMinus);

                    isPreviewActivated = true;
                    go_BaseUI.SetActive(false);
                }
                break;
            case "ExplosiveBarrel_KHJ_Pivot":
                if (metalCount >= 2 && rookCount >= 2)
                {
                    go_Preview = Instantiate(craftTab[TabSelectNumber].crafts[_slotNumber].go_PreviewPrefab,
                                                    tf_Player.position + tf_Player.forward,
                                                    Quaternion.identity);

                    int metalMinus = 2;
                    int rookMinus = 2;

                    inven.SetItemCount("Metal", metalMinus);
                    inven.SetItemCount("Rook", rookMinus);

                    isPreviewActivated = true;
                    go_BaseUI.SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        {
            Window();
        }

        if (isPreviewActivated)
            PreviewPositionUpdate();

        if (Input.GetButtonDown($"Fire1"))
            Build();

        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();
    }

    // 클릭해서 설치할때 실행
    private void Build()
    {
        if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().isBuildable())
        {
            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
        }
    }

    // 설치 미리보기
    private void PreviewPositionUpdate()
    {
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;
                go_Preview.transform.position = _location;
                //go_Preview.transform.LookAt(tf_Player);
            }
        }
    }

    // 설치 취소
    private void Cancel()
    {
        if (isPreviewActivated)
            Destroy(go_Preview);

        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;

        go_BaseUI.SetActive(false);
    }

    // UI 켜기/끄기
    private void Window()
    {
        if (!isActivated)
            OpenWindow();
        else
            CloseWindow();
    }

    private void OpenWindow()
    {
        isActivated = true;
        go_BaseUI.SetActive(true);
    }

    private void CloseWindow()
    {
        go_Preview = null;
        go_Prefab = null;

        isActivated = false;
        go_BaseUI.SetActive(false);
    }
}

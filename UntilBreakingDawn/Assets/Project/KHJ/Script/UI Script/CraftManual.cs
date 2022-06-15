using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftTab
{
    public string craftsName;
    public Craft[] crafts;        // ���� �ǵ��� �� ��
}

[System.Serializable]
public class Craft
{
    public string craftName;
    public GameObject go_Prefab;        // ���� ��ġ�� ������
    public GameObject go_PreviewPrefab; // �̸����� ������
    // ������
    public Item item;
}

public class CraftManual : MonoBehaviour
{
    public  bool isActivated = false;
    public  bool isPreviewActivated = false;

    [SerializeField]
    private GameObject go_BaseUI;   // �⺻ ���̽� UI
    [SerializeField]
    private GameObject[] craft_TabUI;    // �� ���̽� UI

    [SerializeField]
    private CraftTab[] craftTab;    // ���� �ǵ� ����

    //private Craft[] craftWall;      // ����

    private GameObject go_Preview;  // �̸����� �������� ���� ����
    private GameObject go_Prefab;   // ���� ������ �������� ���� ����

    [SerializeField]
    private Transform tf_Player;    // �÷��̾� ��ġ

    // Raycast �ʿ� ���� ����
    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float range;

    // �� ��ȣ
    private int TabSelectNumber;

    // �κ��丮
    public Inventory inven;

    [SerializeField]
    private GameObject go_SlotsParent;
    private Slot[] slots;

    private Item items;

    // ��� �Ǻ���
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

    // ���� �� Ŭ��
    public void TabClick(int _TabNumber)
    {
        if (craftTab[_TabNumber].craftsName == $"������_{_TabNumber}")
        {
            for (int i = 0; i < craft_TabUI.Length; i++)
            {
                craft_TabUI[i].transform.Find("SlotActive").gameObject.SetActive(false);
            }

            craft_TabUI[_TabNumber].transform.Find("SlotActive").gameObject.SetActive(true);
        }
        TabSelectNumber = _TabNumber;
    }

    // ���� Ŭ��
    public void SlotClick(int _slotNumber)
    {
        items = craftTab[TabSelectNumber].crafts[_slotNumber].item;

        if (go_Prefab == null && go_Preview == null)
        {
            if (items != null && items.itemType != Item.ItemType.Equipment)
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if (items.name == "Cooked_Meat")
                    {
                        meatCount       = inven.GetItemCount("RawMeat");
                        cookMeatCount   = inven.GetItemCount("CookedMeat");

                        break;
                    }
                    else if (items.name == "magazineMain")
                    {
                        metalCount              = inven.GetItemCount("Metal");
                        AssaultRifleBulletCount = inven.GetItemCount("magazineMain");
                        Debug.Log(metalCount + " / " + AssaultRifleBulletCount);
                        break;
                    }
                }

                if (items.name == "Cooked_Meat")
                {
                    if (meatCount >= 1)
                    {
                        int meatMiuns = 1;
                        
                        inven.SetItemCount("RawMeat", meatMiuns);
                        inven.AcquireItem(items);
                    }
                }
                else if (items.name == "magazineMain")
                {
                    if (metalCount >= 1)
                    {
                        int metalMiuns = 1;

                        inven.SetItemCount("Metal", metalMiuns);
                        inven.AcquireItem(items);
                    }
                }
            }
            else
            {
                go_Prefab = craftTab[TabSelectNumber].crafts[_slotNumber].go_Prefab;

                for (int i = 0; i < slots.Length; i++)
                {
                    if (go_Prefab.name == "WoodWall")
                    {
                        woodCount = inven.GetItemCount("Log");
                        break;
                    }
                    else if (go_Prefab.name == "StoneWall")
                    {
                        rookCount = inven.GetItemCount("Rook");
                        break;
                    }
                    else if (go_Prefab.name == "ExplosiveBarrel_KHJ_Pivot")
                    {
                        metalCount = inven.GetItemCount("Metal");
                        rookCount  = inven.GetItemCount("Rook");
                        break;
                    }
                }

                if (go_Prefab.name == "WoodWall")
                {
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
                }
                else if (go_Prefab.name == "StoneWall")
                {
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
                }
                else if (go_Prefab.name == "ExplosiveBarrel_KHJ_Pivot")
                {
                    if (metalCount >= 2 && rookCount >=2)
                    {
                        go_Preview = Instantiate(craftTab[TabSelectNumber].crafts[_slotNumber].go_PreviewPrefab,
                                                        tf_Player.position + tf_Player.forward,
                                                        Quaternion.identity);

                        int metalMinus = 2;
                        int rookMinus  = 2;

                        inven.SetItemCount("Metal", metalMinus);
                        inven.SetItemCount("Rook", rookMinus);

                        isPreviewActivated = true;
                        go_BaseUI.SetActive(false);
                    }
                }
            }
        }
    }

    void Update()
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

    // Ŭ���ؼ� ��ġ�Ҷ� ����
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

    // ��ġ �̸�����
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

    // ��ġ ���
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

    // UI �ѱ�/����
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

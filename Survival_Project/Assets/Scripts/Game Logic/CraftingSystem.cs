using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance {get; set;}

    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI, survivalScreenUI, refineScreenUI, constructionScreenUI;

    public List<string> inventoryItemList = new List<string>();


    //Category
    Button toolsBTN, survivalBTN, refineBTN, constructionBTN;

    //Craft Buttons
    Button craftHammerBTN, craftPlankBTN, craftChestBTN, craftFoundationBTN, craftWallBTN, craftCampfireBTN;

    //Requirement Text
    Text HammerReq1, HammerReq2;
    Text PlankReq1;
    Text ChestReq1, ChestReq2;
    Text FoundationReq1;
    Text WallReq1;
    Text CampfireReq1, CampfireReq2;

    public bool isOpen;
    public GameObject cameraBrain;

    //All Blueprint
    public Blueprint HammerBLP = new Blueprint("Hammer", 1, 2, "Stone", 2, "Stick", 2);
    public Blueprint PlankBLP = new Blueprint("Plank", 2, 1, "Log", 1, "", 0);
    public Blueprint ChestBLP = new Blueprint("Chest", 1, 2, "Plank", 2, "Stick", 2);
    public Blueprint foundationBLP = new Blueprint("Foundation", 1, 1, "Plank", 4, "", 0);
    public Blueprint wallBLP = new Blueprint("Wall", 1, 1, "Plank", 2, "", 0);
    public Blueprint campfireBLP = new Blueprint("Campfire", 1, 2, "Stone", 4, "Stick", 2);



    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;

        toolsBTN = craftingScreenUI.transform.Find("Tools Button").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate{OpenToolsCategory();});

        survivalBTN = craftingScreenUI.transform.Find("Survival Button").GetComponent<Button>();
        survivalBTN.onClick.AddListener(delegate{OpenSurvivalCategory();});

        refineBTN = craftingScreenUI.transform.Find("Refine Button").GetComponent<Button>();
        refineBTN.onClick.AddListener(delegate{OpenRefineCategory();});

        constructionBTN = craftingScreenUI.transform.Find("Construction Button").GetComponent<Button>();
        constructionBTN.onClick.AddListener(delegate{OpenConstructionCategory();});

        //Hammer
        HammerReq1 = toolsScreenUI.transform.Find("Hammer").transform.Find("req1").GetComponent<Text>();
        HammerReq2 = toolsScreenUI.transform.Find("Hammer").transform.Find("req2").GetComponent<Text>();

        craftHammerBTN = toolsScreenUI.transform.Find("Hammer").transform.Find("Button").GetComponent<Button>();
        craftHammerBTN.onClick.AddListener(delegate{CraftAnyItem(HammerBLP);});


        //Plank
        PlankReq1 = refineScreenUI.transform.Find("Plank").transform.Find("req1").GetComponent<Text>();

        craftPlankBTN = refineScreenUI.transform.Find("Plank").transform.Find("Button").GetComponent<Button>();
        craftPlankBTN.onClick.AddListener(delegate{CraftAnyItem(PlankBLP);});

        //Foundation
        FoundationReq1 = constructionScreenUI.transform.Find("Foundation").transform.Find("req1").GetComponent<Text>();

        craftFoundationBTN = constructionScreenUI.transform.Find("Foundation").transform.Find("Button").GetComponent<Button>();
        craftFoundationBTN.onClick.AddListener(delegate{CraftAnyItem(foundationBLP);});

        //Wall
        WallReq1 = constructionScreenUI.transform.Find("Wall").transform.Find("req1").GetComponent<Text>();

        craftWallBTN = constructionScreenUI.transform.Find("Wall").transform.Find("Button").GetComponent<Button>();
        craftWallBTN.onClick.AddListener(delegate{CraftAnyItem(wallBLP);});


        //Chest
        ChestReq1 = survivalScreenUI.transform.Find("Chest").transform.Find("req1").GetComponent<Text>();
        ChestReq2 = survivalScreenUI.transform.Find("Chest").transform.Find("req2").GetComponent<Text>();

        craftChestBTN = survivalScreenUI.transform.Find("Chest").transform.Find("Button").GetComponent<Button>();
        craftChestBTN.onClick.AddListener(delegate{CraftAnyItem(ChestBLP);});

        //Campfire
        CampfireReq1 = survivalScreenUI.transform.Find("Campfire").transform.Find("req1").GetComponent<Text>();
        CampfireReq2 = survivalScreenUI.transform.Find("Campfire").transform.Find("req2").GetComponent<Text>();

        craftCampfireBTN = survivalScreenUI.transform.Find("Campfire").transform.Find("Button").GetComponent<Button>();
        craftCampfireBTN.onClick.AddListener(delegate{CraftAnyItem(campfireBLP);});

    }

    // Update is called once per frame
    void Update()
    {
        // RefreshNeededItems();

        if (Input.GetKeyDown(KeyCode.C) && !isOpen && ConstructionManager.Instance.inConstructionMode == false  && PlacementSystem.Instance.inPlacementMode == false)
        {
            craftingScreenUI.SetActive(true);
            isOpen = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cameraBrain.SetActive(false);

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
 
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);
            constructionScreenUI.SetActive(false);

            isOpen = false;

            if (InventorySystem.Instance.isOpen == false &&
                CampfireUIManager.Instance.isUiOpen == false &&
                StorageManager.Instance.storageUIOpen == false &&
                MenuManager.Instance.isMenuOpen == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
                cameraBrain.SetActive(true);
            }
        }
    }


    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);

        toolsScreenUI.SetActive(true);
    }

    void OpenSurvivalCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);

        survivalScreenUI.SetActive(true);
    }

    void OpenRefineCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);

        refineScreenUI.SetActive(true);
    }

    void OpenConstructionCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);

        constructionScreenUI.SetActive(true);
    }

    void CraftAnyItem(Blueprint blueprintToCraft)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);

        //Add item into inventory
        StartCoroutine(craftedDelayForSound(blueprintToCraft));
        

        //Remove resources from inventory
        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
        }
        else
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2Amount);
        }


        //Refresh list
        StartCoroutine(calculate());
    }

    public IEnumerator calculate()
    {
        yield return 0;
        InventorySystem.Instance.ReCalculateList();
        RefreshNeededItems();
    }

    public IEnumerator craftedDelayForSound(Blueprint blueprintToCraft)
    {
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i < blueprintToCraft.numberOfItemToProduce; i++)
        {
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
        }
    }

    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        int log_count = 0;
        int plank_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch(itemName)
            {
                case "Stone":
                stone_count += 1;
                    break;
                case "Stick":
                stick_count += 1;
                    break;
                case "Log":
                log_count += 1;
                    break;
                case "Plank":
                plank_count += 1;
                    break;
            }
        }

        // HAMMER
        HammerReq1.text = "2 Stone [" + stone_count + "]";
        HammerReq2.text = "2 Stick [" + stick_count + "]";

        if (stick_count >= 2 && stone_count >= 2 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftHammerBTN.gameObject.SetActive(true);
        }
        else
        {
            craftHammerBTN.gameObject.SetActive(false);
        }

        // PLANK x2
        PlankReq1.text = "1 Log [" + log_count + "]";

        if (log_count >= 1 && InventorySystem.Instance.CheckSlotAvailable(2))
        {
            craftPlankBTN.gameObject.SetActive(true);
        }
        else
        {
            craftPlankBTN.gameObject.SetActive(false);
        }

        // FOUNDATION
        FoundationReq1.text = "4 Plank [" + plank_count + "]";

        if (plank_count >= 1 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftFoundationBTN.gameObject.SetActive(true);
        }
        else
        {
            craftFoundationBTN.gameObject.SetActive(false);
        }

        // WALL
        WallReq1.text = "2 Plank [" + plank_count + "]";

        if (plank_count >= 1 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftWallBTN.gameObject.SetActive(true);
        }
        else
        {
            craftWallBTN.gameObject.SetActive(false);
        }

        // CHEST
        ChestReq1.text = "2 Plank [" + plank_count + "]";
        ChestReq2.text = "2 Stick [" + stick_count + "]";

        if (plank_count >= 2 && stick_count >= 2 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftChestBTN.gameObject.SetActive(true);
        }
        else
        {
            craftChestBTN.gameObject.SetActive(false);
        }

        // CAMPFIRE
        CampfireReq1.text = "4 Stone [" + stone_count + "]";
        CampfireReq2.text = "2 Stick [" + stick_count + "]";

        if (stone_count >= 4 && stick_count >= 2 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftCampfireBTN.gameObject.SetActive(true);
        }
        else
        {
            craftCampfireBTN.gameObject.SetActive(false);
        }

    }
}

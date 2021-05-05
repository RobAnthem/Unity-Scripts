using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

    [System.Serializable]
    public class QuestInfo
    {
        public string ID;
        public Dictionary<string, bool> objective;
        public QuestInfo(Quest q)
        {
            ID = q.ID;
            objective = new Dictionary<string, bool>();
            foreach (Quest.Objective obj in q.objectives)
                objective.Add(obj.ID, false);
        }
        public bool isComplete;
    }
    public static PlayerData Instance
    {

        get
        {
            if (instance == null)
                instance = new PlayerData();
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    [System.Serializable]
    public class CharacterInventory
    {
        public string ID;
        public Dictionary<string, int> m_inventory = new Dictionary<string, int>();
    }
    [System.Serializable]
    public class Equipment
    {
        public string minorArcana;
        public string majorArcana;
    }
    private static PlayerData instance;

    public string Name;
    public Dictionary<string,int> coins;
    public int arrows;
    public string currentQuest;
    private Dictionary<string, Dictionary<string, QuestInfo>> questInfo = new Dictionary<string, Dictionary<string, QuestInfo>>();
    public Dictionary<string, Dictionary<string, QuestInfo>> quests
    {
        get
        {
            if (questInfo == null)
                questInfo = new Dictionary<string, Dictionary<string, QuestInfo>>();
            if (!questInfo.ContainsKey(PlayerController.Instance.partyID))
                questInfo.Add(PlayerController.Instance.partyID, new Dictionary<string, QuestInfo>());
            return questInfo;
        }
    }
   // public Dictionary<string, int> inventory = new Dictionary<string, int>();
    public Dictionary<string, CharacterInventory> inventories = new Dictionary<string, CharacterInventory>();
    public Dictionary<string, Equipment> equips;
    public Dictionary<string, Dictionary<string, AreaState>> mapNodes = new Dictionary<string, Dictionary<string, AreaState>>();
    public Dictionary<string, List<string>> abilities;
    public Dictionary<string, List<string>> recipes;
    private const string startingZone = "darkForest";
    public string currentParty = "sisters";
    public bool Loaded;
    public int lastLevel;
    public PlayerData()
    {
        //questInfo = new Dictionary<string, QuestInfo>();
        //mapNodes.Add();
        abilities = new Dictionary<string, List<string>>();
        equips = new Dictionary<string, Equipment>();
        coins = new Dictionary<string, int>();
        recipes = new Dictionary<string, List<string>>();
    }
    public bool HasAbility(string pId, string aId)
    {
        if (!abilities.ContainsKey(pId))
        {
            abilities.Add(pId, new List<string>());
        }
        return abilities[pId].Contains(aId);
    }
    public void AddCoins(int amount)
    {
        if (!coins.ContainsKey(PlayerController.Instance.partyID))
        {
            coins.Add(PlayerController.Instance.partyID, 0);
        }
        coins[PlayerController.Instance.partyID] += amount;
    }
    public void RemoveCoins(int amount)
    {
        if (!coins.ContainsKey(PlayerController.Instance.partyID))
        {
            coins.Add(PlayerController.Instance.partyID, 0);
        }
        coins[PlayerController.Instance.partyID] -= amount;
    }
    public void AddAbility(string pID, string aID)
    {
        if (!abilities.ContainsKey(pID))
        {
            abilities.Add(pID, new List<string>());
        }
        abilities[pID].Add(aID);
    }
    public void AddMapNode(string id, string pID)
    {
        if (!mapNodes.ContainsKey(pID))
        {
            mapNodes.Add(pID, new Dictionary<string, AreaState>());
        }
        if (!mapNodes[pID].ContainsKey(id))
            mapNodes[pID].Add(id, AreaState.Unexplored);
        else if (mapNodes[pID].ContainsKey(id) && mapNodes[pID][id] == AreaState.Hidden)
        {
            mapNodes[pID][id] = AreaState.Unexplored;
        }
    }
    public bool HasMadeNode(string id, string pID)
    {
        if (!mapNodes.ContainsKey(pID))
        {
            mapNodes.Add(pID, new Dictionary<string, AreaState>());
        }
        return mapNodes[pID].ContainsKey(id);
    }
    public void SetNodeComplete(string id, string pID)
    {
        if (!mapNodes.ContainsKey(pID))
        {
            mapNodes.Add(pID, new Dictionary<string, AreaState>());
        }
        if (!mapNodes[pID].ContainsKey(id))
            mapNodes[pID].Add(id, AreaState.Complete);
        else if (mapNodes[pID].ContainsKey(id))
        {
            mapNodes[pID][id] = AreaState.Complete;
        }
    }
    public AreaState GetNodeState(string id, string pID)
    {
        if (!mapNodes.ContainsKey(pID))
        {
            mapNodes.Add(pID, new Dictionary<string, AreaState>());
        }
        if (!mapNodes[pID].ContainsKey(id))
            mapNodes[pID].Add(id, AreaState.Hidden);
        return mapNodes[pID][id];
    }
    public void AddQuest(Quest q)
    {
        if (!quests[PlayerController.Instance.partyID].ContainsKey(q.ID))
        {
            Debug.Log("Added Quest " + q.ID);
            quests[PlayerController.Instance.partyID].Add(q.ID, new QuestInfo(q));
            if (q.mainQuest || string.IsNullOrEmpty(currentQuest))
            {
                currentQuest = q.ID;
                QuestController.Instance.questUI.UpdateQuestUI();

            }
        }
    }
    public void AddQuestTracked(Quest q)
    {
        if (!quests[PlayerController.Instance.partyID].ContainsKey(q.ID))
        {
            Debug.Log("Added Quest " + q.ID);
            quests[PlayerController.Instance.partyID].Add(q.ID, new QuestInfo(q));
            currentQuest = q.ID;
            QuestController.Instance.questUI.UpdateQuestUI();

        }
    }
    public bool GetObjective(string quest, string objective)
    {
        if (!quests[PlayerController.Instance.partyID].ContainsKey(quest))
            return false;
        return quests[PlayerController.Instance.partyID][quest].objective[objective];
    }
    public string playerFile = "playerFile";
    public void SaveData(string file = "playerFile")
    {
        currentParty = PlayerController.Instance.partyID;
        lastLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        SaveManager.Instance.SaveData(this, file);
    }
    public static void LoadGame(string file = "playerFile")
    {
        Instance = SaveManager.Instance.LoadData<PlayerData>(file);
    }
    public void OnQuestComplete(string questID)
    {
        Debug.Log("On Quest Complete " + questID);
        Quest q = QuestController.Instance.GetQuest(questID);
        SceneControl.Instance.OnQuestComplete(questID);
        //PlayerData.Instance.questInfo.Remove(questID);
        quests[PlayerController.Instance.partyID][questID].isComplete = true;
        if (q.nextQuest && currentQuest == questID)
        {
            PlayerData.Instance.AddQuestTracked(q.nextQuest);
        }
        else
        {
            PlayerData.Instance.AddQuest(q.nextQuest);
        }
    }
    public void AddObjective(string questID, string objectiveID)
    {
        if (quests[PlayerController.Instance.partyID].ContainsKey(questID))
        {
      
            quests[PlayerController.Instance.partyID][questID].objective[objectiveID] = true;
            QuestController.Instance.questUI.UpdateQuestUI();
            bool complete = true;
            foreach (KeyValuePair<string, bool> pair in quests[PlayerController.Instance.partyID][questID].objective)
            {
                if (!pair.Value)
                {
                    complete = false;
                    break;
                }
            }
            if (complete)
            {
                OnQuestComplete(questID);
            }
        }
    }
    public void AddItem(ItemData iDat)
    {
        CheckInventories();
        if (inventories[PlayerController.Instance.partyID].m_inventory.ContainsKey(iDat.ID))
        {
            inventories[PlayerController.Instance.partyID].m_inventory[iDat.ID]++;
        }
        else
        {
            inventories[PlayerController.Instance.partyID].m_inventory.Add(iDat.ID, 1);
        }
    }
    public void RemoveItem(ItemData iDat)
    {
        if (inventories[PlayerController.Instance.partyID].m_inventory.ContainsKey(iDat.ID))
        {
            inventories[PlayerController.Instance.partyID].m_inventory[iDat.ID]--;
            if (inventories[PlayerController.Instance.partyID].m_inventory[iDat.ID] <= 0)
                inventories[PlayerController.Instance.partyID].m_inventory.Remove(iDat.ID);
        }
    }
    public bool PlayerHasItem(string id)
    {
        CheckInventories();
        return inventories[PlayerController.Instance.partyID].m_inventory.ContainsKey(id);
    }
    public int GetItemCount(string id)
    {
        CheckInventories();
        return inventories[PlayerController.Instance.partyID].m_inventory[id];
    }
    public Equipment GetEquipment(string id)
    {
        CheckInventories();
        return equips[id];
    }
    public int GetCoins()
    {
        if (!coins.ContainsKey(PlayerController.Instance.partyID))
        {
            coins.Add(PlayerController.Instance.partyID, 0);
        }
        return coins[PlayerController.Instance.partyID];
    }
    public void CheckInventories()
    {
        if (!coins.ContainsKey(PlayerController.Instance.partyID))
        {
            coins.Add(PlayerController.Instance.partyID, 0);
        }
        if (!inventories.ContainsKey(PlayerController.Instance.partyID))
        {
            PlayerData.CharacterInventory inv = new CharacterInventory();
            inv.ID = PlayerController.Instance.partyID;
            inv.m_inventory = new Dictionary<string, int>();
            inventories.Add(inv.ID, inv);
        }
        foreach (PlayerMasterControl.Character cha in SceneControl.Instance.PMC.characters)
        {
            if (!equips.ContainsKey(cha.player.characterID))
            {
                equips.Add(cha.player.characterID, new Equipment());
            }
        }
    }
    public void EquipMinor(string id)
    {
        CheckInventories();
        string oldMinor = equips[PlayerController.Instance.characterID].minorArcana;
        equips[PlayerController.Instance.characterID].minorArcana = id;
        if (!string.IsNullOrEmpty(oldMinor))
        {
            AddItem(Inventory.Instance.GetItem(oldMinor));
        }
    }
    public void EquipMajor(string id)
    {
        CheckInventories();
        string oldMajor = equips[PlayerController.Instance.characterID].majorArcana;
        equips[PlayerController.Instance.characterID].majorArcana = id;
        if (!string.IsNullOrEmpty(oldMajor))
        {
            AddItem(Inventory.Instance.GetItem(oldMajor));
        }
    }
    public void AddRecipe(string id)
    {
        CheckRecipes();
        recipes[PlayerController.Instance.partyID].Add(id);
    }
    public List<string> GetRecipes()
    {
        CheckRecipes();
        return recipes[PlayerController.Instance.partyID];
    }
    public void CheckRecipes()
    {
        if (!recipes.ContainsKey(PlayerController.Instance.partyID))
        {
            recipes.Add(PlayerController.Instance.partyID, new List<string>());
        }
    }
}

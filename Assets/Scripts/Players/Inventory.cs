﻿using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    #region Attributes

    public static Inventory instance;
    public static int numSlots = 8;

    private GameObject selectedItemPanel;
    private GameObject selectedItemSlot;
    private Text selectedItemInfo;
    private PickUpItem[] items;
    private PickUpItem selectedItem;
    private int numSlot;

    #endregion

    #region Start

    public void Start()
    {
        instance = this;

        if (items == null)
        {
            items = new PickUpItem[numSlots];
        }
    }

    #endregion

    #region Common

    public void AddItemToInventory(PickUpItem item)
    {
        int freeSlot = GetFreeSlot();

        if (freeSlot != -1)
        {
            items[freeSlot] = item;

            Image slotSprite = GameObject.Find("SlotSprite" + freeSlot).GetComponent<Image>();
            slotSprite.sprite = item.GetComponent<SpriteRenderer>().sprite;

            SendMessageToServer("InventoryUpdate/Add/" + freeSlot + "/" + item.name, true);
        }

    }

    public void RemoveItemFromInventory(PickUpItem itemToRemove)
    {

        bool found = false;
        int index;

        for (index = 0; index < items.Length; index++)
        {
            if (items[index].Equals(itemToRemove))
            {
                found = true;
                break;
            }
        }

        if (found)
        {
            items[index] = null;

            Image slotSprite = GameObject.Find("SlotSprite" + index).GetComponent<Image>();
            slotSprite.sprite = null;

            UnselectItem();
            SendMessageToServer("InventoryUpdate/Remove/" + index, true);
        }

    }

    public void SelectItem(PickUpItem item)
    {
        selectedItemInfo.text = "";
        selectedItemInfo.text = "<color=#e67f84ff><b>" + "Usando '" + item.name + "': </b></color>" + "\r\n";
        selectedItemInfo.text += "<color=#f9ca45ff>" + item.info + "</color>";

        selectedItemSlot.GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;

        ToogleSelectedItem(true);
    }

    public void UnselectItem()
    {
        selectedItemInfo.text = "";
        selectedItemSlot.GetComponent<Image>().sprite = null;

        ToogleSelectedItem(false);
    }

    public void DropItem()
    {
        SendMessageToServer("CreateGameObject/" + selectedItem.name, true);
        RemoveItemFromInventory(selectedItem);
        selectedItemPanel.SetActive(false);
    }

    #endregion

    #region Utils

    public int GetFreeSlot()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (!items[i])
            {
                return i;
            }
        }

        return -1;
    }

    protected void ToogleSelectedItem(bool active)
    {
        selectedItemSlot.SetActive(active);
        selectedItemPanel.SetActive(active);
    }

    #endregion

    #region Messaging

    private void SendMessageToServer(string message, bool secure)
    {
        if (Client.instance)
        {
            Client.instance.SendMessageToServer(message, secure);
        }
    }

    #endregion

}
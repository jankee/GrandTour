using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devdog.InventorySystem.Models;
using UnityEngine;

namespace Devdog.InventorySystem
{
    [RequireComponent(typeof(ItemCollectionBase))]
    public partial class CollectionPopulator : MonoBehaviour
    {

        public InventoryItemAmountRow[] items = new InventoryItemAmountRow[0];
        public bool fireAddItemEvents = false;

        public void Start()
        {
            var col = GetComponent<ItemCollectionBase>();
            if (col == null)
            {
                Debug.LogError("CollectionPopulator can only be used on a collection.", transform);
                return;
            }

            foreach (var item in items)
            {
                var instanceItem = Instantiate<InventoryItemBase>(item.item);
                instanceItem.currentStackSize = item.amount;
                col.AddItem(instanceItem, null, true, fireAddItemEvents);
            }
        }
    }
}
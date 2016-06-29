using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devdog.InventorySystem.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.InventorySystem
{
    public partial class CollectionToCollectionSyncer
    {
        public ItemCollectionBase fromCollection { get; private set; }
        public ItemCollectionBase toCollection { get; private set; }

        public CollectionToCollectionSyncer(ItemCollectionBase fromCollection, ItemCollectionBase toCollection)
        {
            Assert.IsNotNull(fromCollection, "From collection is null, can't sync.");
            Assert.IsNotNull(toCollection, "To collection is null, can't sync.");

//            Assert.IsFalse(fromCollection.useReferences, "fromCollection is using references, this is not allowed. Only 2 non-reference based collections can be synced.");
//            Assert.IsFalse(toCollection.useReferences, "toCollection is using references, this is not allowed. Only 2 non-reference based collections can be synced.");

            this.fromCollection = fromCollection;
            this.toCollection = toCollection;

            RegisterEvents();
        }

        public void UnRegister()
        {
            UnRegisterEvents();
        }

        private void RegisterEvents()
        {
            toCollection.Resize(fromCollection.collectionSize, true);
            CopyAll();
            CopyCurrencies();

            fromCollection.OnAddedItem += OnAddedItem;
            fromCollection.OnRemovedItem += OnRemovedItem;
            fromCollection.OnUsedItem += OnUsedItem;
            fromCollection.OnSorted += OnSorted;
            fromCollection.OnResized += OnResized;
            fromCollection.OnRemovedReference += OnRemovedReference;
            fromCollection.OnSwappedItems += OnSwappedItems;
            fromCollection.OnUnstackedItem += OnUnstackedItem;
            fromCollection.OnMergedSlots += OnMergedSlots;
            fromCollection.OnCurrencyChanged += OnCurrencyChanged;
        }
        
        private void CopyAll()
        {
            for (int i = 0; i < fromCollection.collectionSize; i++)
            {
                toCollection[i].item = fromCollection[i].item;
                toCollection[i].Repaint();
            }
        }

        private void CopyCurrencies()
        {
            toCollection.currenciesContainer.lookups = fromCollection.currenciesContainer.lookups;
        }

        private void UnRegisterEvents()
        {
            fromCollection.OnAddedItem -= OnAddedItem;
            fromCollection.OnRemovedItem -= OnRemovedItem;
            fromCollection.OnUsedItem -= OnUsedItem;
            fromCollection.OnSorted -= OnSorted;
            fromCollection.OnResized -= OnResized;
            fromCollection.OnRemovedReference -= OnRemovedReference;
            fromCollection.OnSwappedItems -= OnSwappedItems;
            fromCollection.OnUnstackedItem -= OnUnstackedItem;
            fromCollection.OnMergedSlots -= OnMergedSlots;
            fromCollection.OnCurrencyChanged -= OnCurrencyChanged;
        }


        private void OnUnstackedItem(ItemCollectionBase fromColl, uint startslot, ItemCollectionBase toColl, uint endslot, uint amount)
        {
            toCollection[startslot].item = fromCollection[startslot].item;
            toCollection[startslot].Repaint();

            if (fromColl == toColl)
            {
                toCollection[endslot].item = fromCollection[endslot].item;
                toCollection[endslot].Repaint();                
            }
        }

        private void OnCurrencyChanged(float amountBefore, InventoryCurrencyLookup lookup)
        {
            CopyCurrencies();
        }


        private void OnSwappedItems(ItemCollectionBase from, uint fromSlot, ItemCollectionBase to, uint toSlot)
        {
            if (from == fromCollection)
            {
                toCollection[fromSlot].item = fromCollection[fromSlot].item;
                toCollection[fromSlot].Repaint();
            }

            if (to == fromCollection)
            {
                toCollection[toSlot].item = fromCollection[toSlot].item;
                toCollection[toSlot].Repaint();
            }
        }

        private void OnRemovedReference(InventoryItemBase item, uint slot)
        {
            toCollection[slot].item = fromCollection[slot].item;
            toCollection[slot].Repaint();
        }

        private void OnSorted()
        {
            CopyAll();
        }

        private void OnUsedItem(InventoryItemBase item, uint itemid, uint slot, uint amount)
        {
            if (toCollection == item.itemCollection)
            {
                toCollection[slot].item = fromCollection[slot].item;
                toCollection[slot].Repaint();
            }
            else
            {
                fromCollection[slot].item = toCollection[slot].item;
                fromCollection[slot].Repaint();                
            }
        }

        private void OnRemovedItem(InventoryItemBase item, uint itemid, uint slot, uint amount)
        {
            toCollection[slot].item = fromCollection[slot].item;
            toCollection[slot].Repaint();
        }

        private void OnAddedItem(IEnumerable<InventoryItemBase> inventoryItemBases, uint amount, bool camefromcollection)
        {
            foreach (var item in inventoryItemBases)
            {
                toCollection[item.index].item = fromCollection[item.index].item;
                toCollection[item.index].Repaint();
            }
        }

        private void OnMergedSlots(ItemCollectionBase from, uint fromSlot, ItemCollectionBase to, uint toSlot)
        {
            if (from == fromCollection)
            {
                toCollection[fromSlot].item = fromCollection[fromSlot].item;
                toCollection[fromSlot].Repaint();
            }

            if (to == fromCollection)
            {
                toCollection[toSlot].item = fromCollection[toSlot].item;
                toCollection[toSlot].Repaint();
            }
        }

        private void OnResized(uint fromsize, uint tosize)
        {
            int slotsToAdd = ((int)tosize) - ((int)fromsize);
            if (slotsToAdd > 0)
            {
                toCollection.AddSlots((uint)slotsToAdd, false);
            }
            else if (slotsToAdd < 0)
            {
                toCollection.RemoveSlots((uint)Mathf.Abs(slotsToAdd), true, false);
            }
        }
    }
}

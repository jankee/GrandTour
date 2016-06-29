using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.InventorySystem.Models
{
    /// <summary>
    /// A helper class that is used to verify if a set of items can be added.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ItemCollectionBaseAddCounter
    {
        public sealed class Tuple
        {
            private uint? _itemID;

            public uint? itemID
            {
                get { return _itemID; }
                set
                {
                    _itemID = value;
                    if (_itemID == null)
                    {
                        _amount = 0;
                    }
                }
            }

            private uint _amount;
            public uint amount
            {
                get { return _amount; }
                set
                {
                    _amount = value;
                    if (_amount == 0)
                    {
                        _itemID = null;
                    }
                }
            }


            public Tuple(uint? itemID, uint amount)
            {
                this.itemID = itemID;
                this.amount = amount;
            }

            public void Reset()
            {
                itemID = null;
                amount = 0;
            }
        }

        public class CollectionLookup : InventoryCollectionLookup<Tuple[]>
        {

            public ItemCollectionBase collectionRef { get; set; }
            
            public CollectionLookup(ItemCollectionBase col)
                : this(ItemsToTuples(col.items), 50, col)
            { }

            public CollectionLookup(Tuple[] collection, int priority, ItemCollectionBase collectionRef)
                : base(collection, priority)
            {
                this.collectionRef = collectionRef;
            }
        }



        private readonly List<CollectionLookup> _collections = new List<CollectionLookup>();

        public List<CollectionLookup> collections
        {
            get
            {
                return _collections;
            }
        } 


        public ItemCollectionBaseAddCounter()
        {
            
        }

        public ItemCollectionBaseAddCounter(params InventoryCollectionLookup<ItemCollectionBase>[] collection)
        {
            LoadFrom(collection);
        }

        private static Tuple[] ItemsToTuples(InventoryUIItemWrapperBase[] items)
        {
            return ItemsToTuples(items.Select(o => o.item).ToArray());
        }

        private static Tuple[] ItemsToTuples(InventoryItemBase[] items)
        {
            Tuple[] tuples = new Tuple[items.Length];
            for (int i = 0; i < tuples.Length; i++)
            {
                var t = new Tuple(null, 0);
                if (items[i] != null)
                {
                    t.itemID = items[i].ID;
                    t.amount = items[i].currentStackSize;
                }

                tuples[i] = t;
            }

            return tuples;
        }

        public void LoadFrom(params InventoryCollectionLookup<ItemCollectionBase>[] collectionsToLoadFrom)
        {
            _collections.Clear();
            foreach (var col in collectionsToLoadFrom)
            {
                _collections.Add(new CollectionLookup(ItemsToTuples(col.collection.items), col.priority, col.collection));
            }
        }

        public void LoadFrom(params ItemCollectionBase[] collections)
        {
            LoadFrom(collections.Select(o => new InventoryCollectionLookup<ItemCollectionBase>(o, 50)).ToArray());
        }


        protected virtual CollectionLookup GetBestLootCollectionForItem(InventoryItemBase item)
        {
            CollectionLookup best = null;

            foreach (var lookup in _collections)
            {
                if (CanAddItem(lookup, item))
                {
                    if (best == null)
                        best = lookup;
                    else if (lookup.priority > best.priority)
                        best = lookup;
                }
            }

            return best;
        }

        /// <summary>
        /// Get the total weight of all items inside this collection
        /// </summary>
        /// <returns></returns>
        public virtual float GetWeight(CollectionLookup lookup)
        {
            return lookup.collection.Sum(o => o.itemID == null ? 0.0f : ItemManager.database.items[o.itemID.Value].weight * o.amount);
        }


        private bool CanAddItem(CollectionLookup lookup, InventoryItemBase item)
        {
            return CanAddItemCount(lookup, item) >= item.currentStackSize;
        }

        public uint CanAddItemCount(CollectionLookup lookup, InventoryItemBase itemToAdd)
        {
            if (lookup.collectionRef.canPutItemsInCollection == false)
                return 0;

            if (lookup.collectionRef.useReferences)
                return 0;

            if (lookup.collectionRef.VerifyFilters(itemToAdd) == false)
                return 0;

            if (lookup.collectionRef.VerifyCustomConditionals(itemToAdd) == false)
                return 0;

            int weightLimit = 99999;
            if (lookup.collectionRef.restrictByWeight && itemToAdd.weight > 0.0f) // avoid dividing by 0.0f
            {
                float weightSpace = lookup.collectionRef.restrictMaxWeight - GetWeight(lookup);
                weightLimit = Mathf.FloorToInt(weightSpace / itemToAdd.weight);
            }

            int amount = 0;
            foreach (var item in lookup.collection)
            {
                if (item.itemID != null && item.itemID == itemToAdd.ID)
                    amount += Mathf.Clamp((int)itemToAdd.maxStackSize - (int)item.amount, 0, 999999);
                else if (item.itemID == null)
                    amount += (int)itemToAdd.maxStackSize;
            }

            return (uint)Mathf.Min(amount, weightLimit);
        }


        /// <summary>
        /// Try adding a list of items.
        /// </summary>
        /// <param name="itemsToAdd"></param>
        /// <returns>All items that couldn't be added. If returnValue.Length == 0 the action was sucesful.</returns>
        public InventoryItemAmountRow[] TryAdd(IList<InventoryItemAmountRow> itemsToAdd)
        {
            var unAddedItems = new List<InventoryItemAmountRow>(itemsToAdd.Count);
            for (int i = 0; i < itemsToAdd.Count; i++)
            {
                unAddedItems.Add(itemsToAdd[i]);
            }

            for (int j = 0; j < itemsToAdd.Count; j++)
            {
                bool added = false;
                var collection = GetBestLootCollectionForItem(itemsToAdd[j].item);
                if (collection == null)
                {
                    break;
                }

                for (int i = 0; i < collection.collection.Length; i++)
                {
                    if (collection.collection[i].itemID == itemsToAdd[j].item.ID)
                    {
                        if (collection.collection[i].amount + itemsToAdd[j].amount <= itemsToAdd[j].item.maxStackSize)
                        {
                            // Doesn't exceed stack size.

                            collection.collection[i].amount += itemsToAdd[j].amount;

                            unAddedItems[j] = new InventoryItemAmountRow(null, 0);
                            added = true;
                            break;
                        }

                        // Exceeds stack size, try to add as much as possible.
                        uint canAddAmount = itemsToAdd[j].item.maxStackSize - collection.collection[i].amount;
                        unAddedItems[j].SetAmount(unAddedItems[j].amount - canAddAmount);
                        collection.collection[i].amount += canAddAmount;
                    }
                }

                if (added == false)
                {
                    for (int i = 0; i < collection.collection.Length; i++)
                    {
                        if (collection.collection[i].itemID == null)
                        {
                            collection.collection[i].itemID = itemsToAdd[j].item.ID;
                            collection.collection[i].amount = itemsToAdd[j].amount;

                            unAddedItems[j] = new InventoryItemAmountRow(null, 0);
                            break;
                        }
                    }
                }
            }

            var a = unAddedItems.ToList();
            a.RemoveAll(o => o.item == null || o.amount == 0);

            return a.ToArray();
        }

        public InventoryItemAmountRow[] TryAdd(IList<InventoryItemBase> itemsToAdd)
        {
            return TryAdd(InventoryItemUtility.ItemsToRows(itemsToAdd));
        }

        public bool TryRemoveItems(IList<InventoryItemBase> itemsToRemove)
        {
            return TryRemoveItems(InventoryItemUtility.ItemsToRows(itemsToRemove));
        }

        public bool TryRemoveItems(IList<InventoryItemAmountRow> itemsToRemove)
        {
            return TryRemoveItems(itemsToRemove.Select(o => new Tuple(o.item.ID, o.amount)).ToArray());
        }

        private bool TryRemoveItems(params Tuple[] tuples)
        {
            int itemsRemoved = 0;
            for (int i = 0; i < tuples.Length; i++)
            {
                var toRemove = tuples[i].amount;
                foreach (var collection in _collections)
                {
                    if (toRemove <= 0)
                        break;

                    foreach (var tuple in GetStacksSmallestToLargest(collection, tuples[i].itemID.Value))
                    {
                        if (tuple.itemID != null && tuple.itemID == tuples[i].itemID)
                        {
                            if (tuple.amount >= toRemove)
                            {
                                // Remove all at once
                                tuple.amount -= toRemove;
                                toRemove = 0;
                                itemsRemoved++;

                                if (tuple.amount == 0)
                                {
                                    tuple.Reset();
                                }

                                break;
                            }

                            if (tuple.amount < toRemove)
                            {
                                toRemove -= tuple.amount;
                                tuple.Reset();
                            }
                        }
                    }
                }
            }
            return tuples.Length == itemsRemoved;
        }

        public Tuple[] GetStacksSmallestToLargest(CollectionLookup lookup, uint itemID)
        {
            return lookup.collection.Where(o => o.itemID == itemID).OrderBy(o => o.amount).ToArray();
        }
    }
}

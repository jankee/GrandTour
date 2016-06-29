using UnityEngine;
using System.Collections;
using Devdog.InventorySystem.Models;
using UnityEngine.Assertions;

namespace Devdog.InventorySystem
{
    [HelpURL("http://devdog.nl/documentation/player/")]
    public abstract class InventoryPlayerBase : MonoBehaviour
    {

//          TODO: Unity doesn't handle generics, maybe in the future??
//        [System.Serializable]
//        public struct DynamicField<T> where T : ItemCollectionBase
//        {
//            private T _reference;
//            public T reference
//            {
//                get
//                {
//                    if (_reference != null)
//                    {
//                        return _reference;
//                    }
//
//                    _reference = GameObject.Find(path).GetComponent<T>();
//                    return _reference;
//                }
//            }
//            public string path { get; private set; }
//
//            public DynamicField(string path)
//            {
//                this.path = path;
//                this._reference = null;
//            }
//        }
//
//        public DynamicField<CharacterUI> characterCols;
//        public DynamicField<ItemCollectionBase>[] inventories;
//        public DynamicField<SkillbarUI> skillbars;




        // Reference based stuff
        public CharacterUI characterCollection;
        public ItemCollectionBase[] inventoryCollections = new ItemCollectionBase[0];
        public SkillbarUI skillbarCollection;


        ///////// Instantiation stuff
        public bool dynamicallyFindUIElements = false;

        public string characterUIPath = "Canvas/InventoryAndCharacter/PaddingBox/CharacterWindow";
        public string[] inventoryPaths = new string[0];
        public string skillbarPath = "Canvas/Skillbar";



        public InventoryPlayerEquipTypeBinder[] equipLocations = new InventoryPlayerEquipTypeBinder[0];



        protected virtual void Awake()
        {

        }

        protected virtual void UpdateEquipLocations()
        {
            foreach (var equipLoc in equipLocations)
            {
                if (equipLoc.findDynamic)
                {
                    Transform equipTransform = null;
                    InventoryUtility.FindChildTransform(transform, equipLoc.equipTransformPath, ref equipTransform);
                    equipLoc.equipTransform = equipTransform;

                    Assert.IsNotNull(equipLoc.equipTransform, "Equip transform path is not valid (" + equipLoc.equipTransformPath + ")");
                }
            }
        }


        public virtual void FindUIElements(bool warnWhenNotFound = true)
        {
            characterCollection = FindElement<CharacterUI>(characterUIPath, warnWhenNotFound);
            inventoryCollections = FindUIElements<ItemCollectionBase>(inventoryPaths, warnWhenNotFound);
            skillbarCollection = FindElement<SkillbarUI>(skillbarPath, warnWhenNotFound);
        }

        private T[] FindUIElements<T>(string[] paths, bool warnWhenNotFound) where T : MonoBehaviour
        {
            T[] comps = new T[paths.Length];
            for (int i = 0; i < paths.Length; i++)
            {
                comps[i] = FindElement<T>(paths[i], warnWhenNotFound);
            }

            return comps;
        }

        private T FindElement<T>(string path, bool warnWhenNotFound) where T : MonoBehaviour
        {
            var obj = GameObject.Find(path);
            if (obj == null)
            {
                if (warnWhenNotFound)
                {
                    Debug.LogWarning("Player instantiation :: Object not found (" + path + ")");
                }

                return null;
            }

            return obj.GetComponent<T>();
        }



    }
}
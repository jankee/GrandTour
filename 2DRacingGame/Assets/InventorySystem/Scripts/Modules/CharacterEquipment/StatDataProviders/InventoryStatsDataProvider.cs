using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devdog.InventorySystem.Models;

namespace Devdog.InventorySystem
{
    public class InventoryStatsDataProvider : IInventoryStatDataProvider
    {
        private CharacterUI characterUI { get; set; }

        public InventoryStatsDataProvider(CharacterUI characterUI)
        {
            this.characterUI = characterUI;
        }


        public Dictionary<string, List<IInventoryCharacterStat>> Prepare(Dictionary<string, List<IInventoryCharacterStat>> appendTo)
        {
            // Get the equip stats
            foreach (var equipStat in ItemManager.database.equipStats)
            {
                if (appendTo.ContainsKey(equipStat.category) == false)
                    appendTo.Add(equipStat.category, new List<IInventoryCharacterStat>());

                appendTo[equipStat.category].Add(new InventoryCharacterStat(InventoryPlayerManager.instance.currentPlayer, equipStat.name, "{0}", 0.0f, 9999f, equipStat.show));
            }

            return appendTo;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using Devdog.InventorySystem.Models;
using UnityEngine;

namespace Devdog.InventorySystem.UI
{
    [HelpURL("http://devdog.nl/documentation/uishowstat/")]
    public class UIShowStat : MonoBehaviour
    {
        [Header("Stat")]
        public string statCategory = "Default";
        public string statName;


        [Header("Player")]
        public bool useCurrentPlayer = true;
        public InventoryPlayer player;

        [Header("Visuals")]
        public UIShowValueModel visualizer = new UIShowValueModel();


        public void Start()
        {
            if (useCurrentPlayer)
            {
                InventoryPlayerManager.instance.OnPlayerChanged += OnPlayerChanged;
            }

            // Force a repaint.
            OnPlayerChanged(null, InventoryPlayerManager.instance.currentPlayer);
        }

        private void OnPlayerChanged(InventoryPlayer oldPlayer, InventoryPlayer newPlayer)
        {
            // Remove the old
            if (oldPlayer != null && oldPlayer.characterCollection != null)
                oldPlayer.characterCollection.OnStatChanged -= Repaint;

            player = newPlayer;

            // Add the new
            if (player != null && player.characterCollection != null)
            {
                player.characterCollection.OnStatChanged += Repaint;
                Repaint(player.characterCollection.stats.Get(statCategory, statName));
            }
        }

        protected virtual void Repaint(IInventoryCharacterStat stat)
        {
            if (stat == null || stat != player.characterCollection.stats.Get(statCategory, statName))
                return;

            visualizer.Repaint(stat.currentValue, stat.maxValue);
        }
    }
}

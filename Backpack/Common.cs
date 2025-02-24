using BepInEx;
using CG;
using CG.Client.Ship.Interactions;
using CG.Game.Player;
using CG.Input;
using CG.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoidManager;

namespace Backpack
{
    internal class Common
    {
        internal static readonly Vector3[] backpackLocations = new Vector3[] { new Vector3(0, 1.3f, -0.35f), new Vector3(0.5f, 1.3f, 0), new Vector3(-0.5f, 1.3f, 0) };
        internal static readonly int backpackSize = backpackLocations.Length;
        internal static List<CarryableObject> backpackItems = new();

        internal static void ButtonPressed(object o, EventArgs e)
        {
            if (LocalPlayer.Instance == null) return;
            if (ServiceBase<InputService>.Instance.CursorVisibilityControl.IsCursorShown) return; //If the player is typing
            if (Configs.ToggleBackpackItem.Value == KeyCode.None) return;

            if (UnityInput.Current.GetKeyDown(Configs.ToggleBackpackItem.Value))
            {
                if (LocalPlayer.Instance.Payload != null)
                    StoreItem();
                else
                    RetrieveItem();
            }
        }

        private static void StoreItem()
        {
            if (!VoidManagerPlugin.Enabled) return;

            if (backpackItems.Count >= backpackSize) return;

            //Backpack was empty, start running every frame
            if (backpackItems.Count == 0)
            {
                Events.Instance.LateUpdate += MoveItems;
            }

            //Add item to backpack
            CarryableObject payload = LocalPlayer.Instance.Payload;
            backpackItems.Add(payload);

            //Release item from the player's hands
            LocalPlayer.Instance.TryReleaseCarryable();
            payload.photonView.RequestOwnership();

            payload.OwnerChange += HandleTheft;
        }

        private static void RetrieveItem()
        {
            //No enabled check, always allow removing from backpack

            if (backpackItems.Count == 0) return;

            CarryableObject item;
            if (Configs.Stack.Value)
            {
                item = backpackItems.Last();
            }
            else
            {
                item = backpackItems.First();
            }

            RemoveItem(item);
            
            //Place item in hands
            CarryableInteract interact = (LocalPlayer.Instance.Character as CustomCharacterLocomotion).Abilities.First(ability => ability is CarryableInteract) as CarryableInteract;
            LocalPlayer.Instance.StartCoroutine(interact.DelayedPickupGrabable(item.GetComponent<GrabableObject>()));
        }

        internal static void RemoveItem(CarryableObject item)
        {
            if (!backpackItems.Contains(item)) return;

            backpackItems.Remove(item);
            item.OwnerChange -= HandleTheft;

            //No items in backpack, stop running every frame
            if (backpackItems.Count == 0)
            {
                Events.Instance.LateUpdate -= MoveItems;
            }
        }

        internal static void MoveItems(object o, EventArgs e)
        {
            //Update the location and rotation of all backpack items every frame
            for (int i = 0; i <  backpackItems.Count; i++)
            {
                CarryableObject item = backpackItems[i];
                item.Position = LocalPlayer.Instance.Position + LocalPlayer.Instance.Rotation * backpackLocations[i];
                item.Rotation = Quaternion.identity;
                item.Velocity = LocalPlayer.Instance.Velocity;
                item.UseGravity = false;
            }
        }

        //If another player takes a backpack item, remove it from the backpack
        private static void HandleTheft(Photon.Realtime.Player player)
        {
            for (int i = backpackItems.Count - 1; i >= 0; i--)
            {
                CarryableObject item = backpackItems.ElementAt(i);
                if (!item.AmOwner)
                {
                    RemoveItem(item);
                }
            }
        }
    }
}

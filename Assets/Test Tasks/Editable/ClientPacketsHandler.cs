using System;
using System.Collections.Generic;
using TestTask.NonEditable;
using UnityEngine;

namespace TestTask.Editable
{
    public static class ClientPacketsHandler
    {
        #region Packet Handlers
        public static void LoginDataReceived(Packet packet)
        {
            int responseCode = packet.ReadInt();
            int clientId = packet.ReadInt();

            ClientManager.Instance.SetClientLogInStatus(responseCode, clientId);
        }

        public static void MonsterDataReceived(Packet packet)
        {
            Debug.Log("ClientPacketsHandler: MonsterDataReceived");

            int monsterId = packet.ReadInt();
            int monsterType = packet.ReadInt();
            float monsterMaxHealth = packet.ReadFloat();
            float monsterCurrentHealth = packet.ReadFloat();

            MonsterData newMonsterData = new MonsterData(monsterId, MonsterNameExtensions.MonsterTypeFromId(monsterType), monsterMaxHealth, monsterCurrentHealth);

            ClientManager.Instance.ClientMobsManager.NotifyMonsterDataReceived(newMonsterData);
            
            Debug.Log($"ClientPacketsHandler: MonsterDataReceived: Id: {monsterId}, Type: {monsterType}, Max Health: {monsterMaxHealth}, Current Health: {monsterCurrentHealth}");
        }
        #endregion

        #region Packet Senders
        public static void SendLoginRequest()
        {
            Packet packet = new Packet(1);
            ClientManager.Instance.PacketSenderClient.SendToServer(packet);
        }

        public static void SendMonsterDataRequest()
        {
            Debug.Log("ClientPacketsHandler: SendMonsterDataRequest");

            Packet packet = new Packet(2);
            ClientManager.Instance.PacketSenderClient.SendToServer(packet);
        }
        #endregion
    }
}

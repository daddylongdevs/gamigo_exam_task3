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

        public static void NewMonsterDataReceived(Packet packet)
        {
            Debug.Log("ClientPacketsHandler: MonsterDataReceived");

            int monsterId = packet.ReadInt();
            int monsterType = packet.ReadInt();
            float monsterMaxHealth = packet.ReadFloat();
            float monsterCurrentHealth = packet.ReadFloat();

            MonsterData newMonsterData = new MonsterData(monsterId, (MonsterNames)monsterType, monsterMaxHealth, monsterCurrentHealth);

            ClientManager.Instance.ClientMobsManager.NotifyNewMonsterDataReceived(newMonsterData);

            Debug.Log($"ClientPacketsHandler: MonsterDataReceived: Id: {monsterId}, Type: {monsterType}, Max Health: {monsterMaxHealth}, Current Health: {monsterCurrentHealth}");
        }

        public static void UpdatedMonsterDataReceived(Packet packet)
        {
            Debug.Log("ClientPacketsHandler: UpdatedMonsterDataReceived");

            int monsterId = packet.ReadInt();
            int monsterType = packet.ReadInt();
            float monsterMaxHealth = packet.ReadFloat();
            float monsterCurrentHealth = packet.ReadFloat();

            MonsterData newMonsterData = new MonsterData(monsterId, (MonsterNames)monsterType, monsterMaxHealth, monsterCurrentHealth);

            ClientManager.Instance.ClientMobsManager.NotifyMonsterDataUpdated(newMonsterData);
        }

        public static void OnColorSetReceived(Packet packet)
        {
            Debug.Log("ClientPacketsHandler: ColorSetReceived");

            int colorCount = packet.ReadInt();
            Color[] colors = new Color[colorCount];

            for (int i = 0; i < colorCount; i++)
            {
                float r = packet.ReadFloat();
                float g = packet.ReadFloat();
                float b = packet.ReadFloat();
                float a = packet.ReadFloat();

                Color newColor = new Color(r, g, b, a);
                colors[i] = newColor;
            }

            ClientManager.Instance.ClientColorManager.NotifyColorSetReceived(colors);
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

        public static void SendMonsterDamageRequest(int monsterId)
        {
            Debug.Log($"ClientPacketsHandler: SendMonsterDamageRequest: Monster ID: {monsterId}");

            Packet packet = new Packet(3);
            packet.Write(monsterId);
            ClientManager.Instance.PacketSenderClient.SendToServer(packet);
        }

        public static void SendColorSetRequest()
        {
            Debug.Log("ClientPacketsHandler: SendColorSetRequest");

            Packet packet = new Packet(4);
            ClientManager.Instance.PacketSenderClient.SendToServer(packet);
        }
        #endregion
    }
}

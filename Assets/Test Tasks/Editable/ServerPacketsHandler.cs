using TestTask.NonEditable;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace TestTask.Editable
{
    public static class ServerPacketsHandler
    {
        #region Packet Handlers
        public static void LoginRequest(Packet packet)
        {
            var clientLogInResponse = ServerMock.Instance.TryConnectClient(out var clientId);
            SendLoginResponse(clientLogInResponse, clientId);
        }

        public static void OnMonsterDataRequest(Packet packet)
        {
            Debug.Log("ServerPacketsHandler: MonsterDataRequest");

            MonsterData monsterData = ServerMock.Instance.ServerMobsManager.SpawnMonster();
            SendNewMonsterSpawnedResponse(monsterData);
        }

        public static void OnMonsterDamageRequest(Packet packet)
        {
            Debug.Log("ServerPacketsHandler: MonsterDamageRequest");

            int monsterId = packet.ReadInt();
            ServerMock.Instance.ServerMobsManager.ApplyDamageToMonster(monsterId);
        }

        public static void OnColorSetRequest(Packet packet)
        {
            Debug.Log("ServerPacketsHandler: ColorSetRequest");

            SendColorSetResponse(ServerMock.Instance.ServerColors.GetServerColors());
        }

        #endregion

        #region Packet Senders
        public static void SendLoginResponse(LoginResponse response, int clientId)
        {
            using (Packet packet = new Packet(1))
            {
                packet.Write((int)response);
                packet.Write(clientId);

                ServerMock.Instance.PacketSenderServer.SendToClient(packet);
            }
        }

        public static void SendNewMonsterSpawnedResponse(MonsterData monsterData)
        {
            Debug.Log("ServerPacketsHandler: SendMonsterResponse");

            using (Packet packet = new Packet(2))
            {
                packet.Write(monsterData.MonsterId);
                packet.Write((int)monsterData.MonsterType);
                packet.Write(monsterData.MonsterMaxHealth);
                packet.Write(monsterData.MonsterCurrentHealth);

                ServerMock.Instance.PacketSenderServer.SendToClient(packet);
            }
        }

        public static void SendMonsterDamagedResponse(MonsterData monsterData)
        {
            Debug.Log("ServerPacketsHandler: SendMonsterDamagedResponse");

            using (Packet packet = new Packet(2))
            {
                packet.Write(monsterData.MonsterId);
                packet.Write((int)monsterData.MonsterType);
                packet.Write(monsterData.MonsterMaxHealth);
                packet.Write(monsterData.MonsterCurrentHealth);

                ServerMock.Instance.PacketSenderServer.SendToClient(packet);
            }
        }

        public static void SendColorSetResponse(IEnumerable<Color> colors)
        {
            Debug.Log("ServerPacketsHandler: SendColorSetResponse");

            using (Packet packet = new Packet(4))
            {
                packet.Write(colors.Count()); // Write the number of colors to the packet

                foreach (Color color in colors)
                {
                    packet.Write(color.r);
                    packet.Write(color.g);
                    packet.Write(color.b);
                    packet.Write(color.a);
                }

                ServerMock.Instance.PacketSenderServer.SendToClient(packet);
            }
        }
        #endregion
    }
}

public enum LoginResponse
{
    Success = 0,
    Failure = 1,
}
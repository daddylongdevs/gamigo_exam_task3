using Microsoft.Unity.VisualStudio.Editor;
using TestTask.NonEditable;
using UnityEngine;
using System;

namespace TestTask.Editable
{
    public class ClientMobsManager : MonoBehaviour
    {
        public static event Action<MonsterData> OnNewMonsterDataReceived;
        public static event Action<MonsterData> OnCurrentMonsterDataUpdated;
        public MonsterData CurrentMonsterData { get; private set; }

        void OnEnable()
        {
            ClientManager.Instance.ClientLogInStatusChanged += OnClientLogInStatusChanged;
        }

        void OnDisable()
        {
            ClientManager.Instance.ClientLogInStatusChanged -= OnClientLogInStatusChanged;
        }

        private void OnClientLogInStatusChanged(int status, int clientId)
        {
            if (status == 0)
            {
                GetMonsterData();
            }
        }

        public void TryApplyDamageToMonster()
        {
            // FAULT: Rapidly calling this request can potentially break the game
            // TODO: Implement a more robust way of handling damage so that its:
            // - Seemingly real time
            // - Smooth UI realignment upon server response
            ClientPacketsHandler.SendMonsterDamageRequest(CurrentMonsterData.MonsterId);
        }

        private void GetMonsterData()
        {
            ClientPacketsHandler.SendMonsterDataRequest();
        }

        public void NotifyNewMonsterDataReceived(MonsterData monsterData)
        {
            Debug.Log("ClientMobsManager: NotifyMonsterDataReceived: " + monsterData.MonsterName);

            if(CurrentMonsterData != null && CurrentMonsterData.MonsterId == monsterData.MonsterId)
            {
                NotifyMonsterDataUpdated(monsterData);
                return;
            }

            CurrentMonsterData = monsterData;
            OnNewMonsterDataReceived?.Invoke(monsterData);
        }

        public void NotifyMonsterDataUpdated(MonsterData monsterData)
        {
            if (CurrentMonsterData == null || monsterData.MonsterId != CurrentMonsterData.MonsterId)
            {
                NotifyNewMonsterDataReceived(monsterData);
                return;
            }

            CurrentMonsterData = monsterData;
            OnCurrentMonsterDataUpdated?.Invoke(CurrentMonsterData);
        }
    }
}

using Microsoft.Unity.VisualStudio.Editor;
using TestTask.NonEditable;
using UnityEngine;
using System;

namespace TestTask.Editable
{
    public class ClientMobsManager : MonoBehaviour
    {
        public static event Action<MonsterData> OnMonsterDataReceived;
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
            if(status == 0)
            {
                GetMonsterData();
            }
        }

        private void GetMonsterData()
        {
            ClientPacketsHandler.SendMonsterDataRequest();
        }

        public void NotifyMonsterDataReceived(MonsterData monsterData)
        {
            Debug.Log("ClientMobsManager: NotifyMonsterDataReceived: " + monsterData.MonsterName);
            CurrentMonsterData = monsterData;
            OnMonsterDataReceived?.Invoke(CurrentMonsterData);
        }
    }
}

using System;
using TestTask.Editable;
using TestTask.NonEditable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TestTask.Editable
{
    public class ServerMobsManager
    {
        [field: SerializeField] public MonsterData MonsterData { get; private set; }

        public ServerMobsManager()
        {
            MonsterData = SpawnMonster();
        }

        public MonsterData SpawnMonster()
        {
            var monsterId = Random.Range(1, 1000);
            var monsterType = MonsterNameExtensions.MonsterTypeFromId(monsterId);
            var monsterMaxHealth = Random.Range(50, 201);
            var monsterCurrentHealth = monsterMaxHealth;

            MonsterData = new MonsterData(monsterId, monsterType, monsterMaxHealth, monsterCurrentHealth);
            MonsterData.MonsterDeath += OnMonsterDied;

            Debug.Log($"ServerMobsManager: MonsterData: Id: {MonsterData.MonsterId}, Type: {MonsterData.MonsterType}, Max Health: {MonsterData.MonsterMaxHealth}, Current Health: {MonsterData.MonsterCurrentHealth}");
            return MonsterData;
        }

        public void ApplyDamageToMonster(int monsterId)
        {
            if (MonsterData == null)
            {
                // This is a deadend where the "game" is essentially stuck. No monster, but no spawning either.
                // Apply a potential fix here.
                Debug.LogError("ServerMobsManager: ApplyDamageToMonster: MonsterData is null");
                return;
            }

            if (MonsterData.MonsterId != monsterId)
            {
                // This is also a potential deadend since we don't have a way to attempt to spawn a new monster from client side apart from death or log in.
                // On mismatch of id, server monster can no longer be damaged. And the client will never receive updates on the new monster.
                Debug.LogError($"ServerMobsManager: ApplyDamageToMonster: MonsterId mismatch: {MonsterData.MonsterId} != {monsterId}");
                return;
            }

            float appliedDamage = Random.Range(10, 20);
            MonsterData.TakeDamage(appliedDamage);

            if (MonsterData.MonsterId == monsterId)
            {
                ServerPacketsHandler.SendMonsterDamagedResponse(MonsterData);
            }
        }

        public void OnMonsterDied()
        {
            MonsterData.MonsterDeath -= OnMonsterDied;
            MonsterData = SpawnMonster();

            ServerPacketsHandler.SendNewMonsterSpawnedResponse(MonsterData);
        }
    }
}  

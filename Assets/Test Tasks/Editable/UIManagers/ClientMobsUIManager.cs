using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TestTask.NonEditable;
using TestTask.Editable;
using UnityEngine.UI;
using TMPro;

namespace TestTask.Editable
{
    public class ClientMobsUIManager : MonoBehaviour
    {
        [field: SerializeField] private Sprite DefaultMonsterIcon;
        [field: SerializeField] private Sprite[] MonsterIcons;
        [field: SerializeField] private Image MonsterIcon;
        [field: SerializeField] private Image MonsterHealthBar;
        [field: SerializeField] private TMP_Text MonsterName;
        [field: SerializeField] private TMP_Text MonsterHealthText;

        void OnEnable()
        {
            ClientMobsManager.OnNewMonsterDataReceived += OnNewMonsterSpawned;
            ClientMobsManager.OnCurrentMonsterDataUpdated += OnCurrentMonsterDataUpdated;
        }

        void OnDisable()
        {
            ClientMobsManager.OnNewMonsterDataReceived -= OnNewMonsterSpawned;
            ClientMobsManager.OnCurrentMonsterDataUpdated -= OnCurrentMonsterDataUpdated;
        }

        public void OnNewMonsterSpawned(MonsterData monsterData)
        {
            Debug.Log("ClientMobsUIManager: OnNewMonsterSpawned: " + monsterData.MonsterName);

            UpdateMonsterName(monsterData.MonsterName);
            UpdateMonsterIcon(monsterData.MonsterName);
            UpdateMonsterHealthBar(monsterData.MonsterCurrentHealth / monsterData.MonsterMaxHealth);
            UpdateMonsterHealthText(monsterData.MonsterCurrentHealth, monsterData.MonsterMaxHealth);
        }

        public void OnCurrentMonsterDataUpdated(MonsterData monsterData)
        {
            Debug.Log("ClientMobsUIManager: OnCurrentMonsterDataUpdated: " + monsterData.MonsterName);

            UpdateMonsterHealthBar(monsterData.MonsterCurrentHealth / monsterData.MonsterMaxHealth);
            UpdateMonsterHealthText(monsterData.MonsterCurrentHealth, monsterData.MonsterMaxHealth);
        }

        private void UpdateMonsterName(string monsterName)
        {
            MonsterName.text = monsterName;
        }

        private void UpdateMonsterIcon(string monsterName)
        {
            MonsterIcon.sprite = GetMonsterIconSprite(monsterName);
        }

        private void UpdateMonsterHealthBar(float healthPercentage)
        {
            float fillAmount = Mathf.Clamp01(healthPercentage);
            MonsterHealthBar.fillAmount = fillAmount;
        }

        private void UpdateMonsterHealthText(float currentHealth, float maxHealth)
        {
            MonsterHealthText.text = $"{currentHealth} / {maxHealth}";
        }

        private Sprite GetMonsterIconSprite(string monsterName)
        {
            foreach (var icon in MonsterIcons)
            {
                if (icon.name == monsterName)
                {
                    return icon;
                }
            }
            return DefaultMonsterIcon;
        }
    }
}

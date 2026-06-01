using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TestTask.Editable;

public class ClientColorsUIManager : MonoBehaviour
{
    [SerializeField] private int count;
    [field: SerializeField] private GameObject ColorDisplayPrefab;
    [field: SerializeField] private RectTransform ColorDisplayParent;

    private List<Image> ColorDisplays = new List<Image>();

    void OnEnable()
    {
        ClientColors.OnColorSetReceived += OnColorSetReceived;
    }

    void OnDisable()
    {
        ClientColors.OnColorSetReceived -= OnColorSetReceived;
    }

    [ContextMenu("Create Color Displays")]
    public void CreateColorDisplays()
    {
        Color[] colors = new Color[count];
        for (int i = 0; i < count; i++) colors[i] = Random.ColorHSV();
        OnColorSetReceived(colors);
    }

    public void OnColorSetReceived(Color[] colors)
    {
        int newColorDisplaysNeeded = colors.Length - ColorDisplays.Count;

        if (newColorDisplaysNeeded > 0)
        {
            CreateNewColorDisplays(newColorDisplaysNeeded);
        }
        else if (newColorDisplaysNeeded < 0)
        {
            int colorDisplaysToHide = Mathf.Abs(newColorDisplaysNeeded);
            for (int i = 0; i < colorDisplaysToHide; i++)
            {
                HideColorDisplayAtIndex(colors.Length + i);
            }
        }

        for (int i = 0; i < colors.Length; i++)
        {
            Image colorDisplay = ColorDisplays[i];
            colorDisplay.gameObject.SetActive(true);
            colorDisplay.color = colors[i];
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(ColorDisplayParent as RectTransform);
    }

    public void CreateNewColorDisplays(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject colorObject = Instantiate(ColorDisplayPrefab, ColorDisplayParent);
            Image newColorDisplay = colorObject.GetComponent<Image>();
            ColorDisplays.Add(newColorDisplay);
        }
    }

    public void HideColorDisplayAtIndex(int index) => ColorDisplays[index].gameObject.SetActive(false);
}

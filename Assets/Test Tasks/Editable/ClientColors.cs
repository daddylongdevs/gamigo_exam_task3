using System;
using UnityEngine;

namespace TestTask.Editable
{
    public class ClientColors : MonoBehaviour
    {
        public static event Action<Color[]> OnColorSetReceived;
        
        public Color[] Colors { get; private set; }

        public void GetColorSet()
        {
            ClientPacketsHandler.SendColorSetRequest();
        }

        public void NotifyColorSetReceived(Color[] colors)
        {
            Colors = colors;
            OnColorSetReceived?.Invoke(Colors);
        }
    }
}

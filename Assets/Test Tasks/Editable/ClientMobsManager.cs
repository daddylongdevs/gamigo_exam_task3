using Microsoft.Unity.VisualStudio.Editor;
using TestTask.NonEditable;
using UnityEngine;

namespace TestTask.Editable
{
    public class ClientMobsManager : MonoBehaviour
    {
        public void GetMonsterData()
        {
            ClientPacketsHandler.SendMonsterDataRequest();
        }
    }
}

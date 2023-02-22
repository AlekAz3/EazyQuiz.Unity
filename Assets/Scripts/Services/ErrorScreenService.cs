using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.UI;

namespace EazyQuiz.Unity
{
    public class ErrorScreenService : MonoBehaviour
    {
        [SerializeField] private GameObject ErrorLabel;
        [SerializeField] private TMP_Text ErrorText;
        [SerializeField] private Button Button;

        public ErrorScreenService()
        {

        }

        public void Activate(string text)
        {
            ErrorLabel.SetActive(true);
            ErrorText.text = text;
        }

        public void Hide()
        {
            ErrorLabel.SetActive(false);

        }
        
    }
}

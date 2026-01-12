using TMPro;
using UnityEngine;

public class LevelDisplayUI : MonoBehaviour
{
   [SerializeField] private TMP_Text levelText;
   
   public void Show(int level)
   {
         levelText.text = $"Уровень {level}";
   }
   public void Show(string level)
   {
       levelText.text = $"Уровень {level}";
   }
}

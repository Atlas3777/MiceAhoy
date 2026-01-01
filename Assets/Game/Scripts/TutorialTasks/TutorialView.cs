using Cysharp.Threading.Tasks;
using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialView : MonoBehaviour
{
    public GameObject ActiveTaskGroup;
    public CanvasGroup ActiveTaskCanvasGroup;
    public TMP_Text TaskText;

    public TypewriterByCharacter TaskTypewriter;
}
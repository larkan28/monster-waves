using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemEquipped : MonoBehaviour
{
    [SerializeField] private Image imageIcon;
    [SerializeField] private GameObject textRoot;
    [SerializeField] private TextMeshProUGUI textLevel;

    private void Awake()
    {
        Refresh(null);
    }

    public void Refresh(Sprite icon, int level = 0)
    {
        imageIcon.sprite = icon;

        textRoot.SetActive(icon != null);
        textLevel.text = level.ToString();
    }
}

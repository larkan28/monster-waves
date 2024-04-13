using UnityEngine;

public class PlayerUI : Singleton<PlayerUI>
{
    [SerializeField] private UI_TextDamage textDamagePrefab;
    [SerializeField] private float textDamageRandomOffset;

    public void ShowDamage(Vector3 center, float damage, bool isCritical = false)
    {
        UI_TextDamage textDamage = Instantiate(textDamagePrefab, center + Util.RandomOffsetByRadius(textDamageRandomOffset), Quaternion.identity);

        if (textDamage != null)
            textDamage.ShowDamage(damage, isCritical);
    }
}

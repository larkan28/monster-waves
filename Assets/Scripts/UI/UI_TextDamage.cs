using UnityEngine;
using TMPro;

public class UI_TextDamage : MonoBehaviour
{
    [SerializeField] private TextMeshPro textDamage;
    [SerializeField] private float duration;
    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;
    [SerializeField] private float alphaDecrease;
    [SerializeField] private float sizeDecrease;
    [SerializeField] private float velocityUp;
    [SerializeField] private Color colorNormalHit;
    [SerializeField] private Color colorCriticalHit;

    private void Update()
    {
        float scaleX = transform.localScale.x - (sizeDecrease * Time.deltaTime);
        float scaleY = transform.localScale.y - (sizeDecrease * Time.deltaTime);

        transform.position += Time.deltaTime * velocityUp * Vector3.up;
        transform.localScale = new Vector3(scaleX, scaleY, 1f);

        textDamage.alpha -= Time.deltaTime * alphaDecrease;
    }

    public void ShowDamage(float damage, bool isCritical = false)
    {
        transform.localScale *= Random.Range(minSize, maxSize); 

        textDamage.enabled = true;
        textDamage.text = Mathf.RoundToInt(damage).ToString();
        
        if (isCritical)
            textDamage.color = colorCriticalHit;
        else
            textDamage.color = colorNormalHit;

        Destroy(gameObject, duration);
    }
}

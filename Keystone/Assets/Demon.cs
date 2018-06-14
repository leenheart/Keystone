using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : MonoBehaviour {

    public float HpMax;
    public float Hp;

    public RectTransform HealthBar;

    public void TakeDammage(int damage)
    {
        Hp -= damage;

        HealthBar.sizeDelta = new Vector2(Hp / 10, HealthBar.sizeDelta.y);

        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}

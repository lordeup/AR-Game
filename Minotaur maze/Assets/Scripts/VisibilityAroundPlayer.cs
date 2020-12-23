using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityAroundPlayer : MonoBehaviour
{
    // Вешаем на объект Rigidbody
    // Вешаем на объект любой подходящий коллайдер(нам сойдёт куб), задаём его размеры так, чтобы он охватывал чуть больше чем ширину коридора лабиринта
    // Ставим на коллайдере галку isTrigger
    // Вешаем на объект этот скрипт.
    // По итогу скрипт будет отображать все объекты, которые попадают в наш коллайдер-триггер
    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<Renderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<Renderer>().enabled = true;
    }
}

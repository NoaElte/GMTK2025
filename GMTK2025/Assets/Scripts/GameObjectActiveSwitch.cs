using UnityEngine;

public class GameObjectActiveSwitch : MonoBehaviour
{
    [SerializeField]
    private GameObject switchObject;

    public void Switch()
    {
        switchObject.SetActive(!switchObject.activeSelf);
    }
}

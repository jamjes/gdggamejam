using UnityEngine;

public class GameEndCanvas : MonoBehaviour
{
    [SerializeField] GameObject UI;

    private void OnEnable()
    {
        Projectile.OnDeathEnter += DisplayUI;
    }

    private void OnDisable()
    {
        Projectile.OnDeathEnter -= DisplayUI;
    }

    void Start()
    {
        UI.SetActive(false);
    }

    void DisplayUI()
    {
        UI.SetActive(true);
    }
}

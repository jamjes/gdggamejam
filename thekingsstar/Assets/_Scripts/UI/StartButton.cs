using UnityEngine;

public class StartButton : MonoBehaviour
{
    public delegate void Button();
    public static event Button OnGameBegin;

    public void StartGame()
    {
        if (OnGameBegin != null)
        {
            OnGameBegin();
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightButton : MonoBehaviour
{
    /// <summary>
    /// Sets our selected button to what we've moused over
    /// </summary>
    /// <param name="button">The button being moused over</param>
    public void OnMouseOverButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
    }
}

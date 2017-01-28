using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public enum Button : int
    {
        OK = 1,
        Yes = 1 << 1,
        No = 1 << 2
    }

    public Text m_labelContent;
    public UnityEngine.UI.Button m_buttonOk;
    public UnityEngine.UI.Button m_buttonYes;
    public UnityEngine.UI.Button m_buttonNo;

    public static void Show(string text, int buttons, System.Action<Button> callback)
    {
        var popupContainer = GameObject.Find("PopupContainer");
        if (popupContainer == null)
            return;

        var popup = popupContainer.GetComponentInChildren<Popup>(true);
        if (popup == null)
            return;

        popup.ShowInternal(text, buttons, callback);
    }

    private void ShowInternal(string text, int buttons, System.Action<Button> callback)
    {
        HideButtons();

        m_labelContent.text = text;

        if ((buttons & (int)Button.OK) == (int)Button.OK)
        {
            m_buttonOk.gameObject.SetActive(true);

            m_buttonOk.onClick.AddListener(() =>
            {
                HandleButtonClicked(Button.OK, callback);
            });
        }

        if ((buttons & (int)Button.Yes) == (int)Button.Yes)
        {
            m_buttonYes.gameObject.SetActive(true);

            m_buttonYes.onClick.AddListener(() =>
            {
                HandleButtonClicked(Button.Yes, callback);
            });
        }

        if ((buttons & (int)Button.No) == (int)Button.No)
        {
            m_buttonNo.gameObject.SetActive(true);

            m_buttonNo.onClick.AddListener(() =>
            {
                HandleButtonClicked(Button.No, callback);
            });
        }

        gameObject.SetActive(true);
    }

    private void Hide()
    {
        m_buttonOk.onClick.RemoveAllListeners();
        m_buttonYes.onClick.RemoveAllListeners();
        m_buttonNo.onClick.RemoveAllListeners();

        HideButtons();

        gameObject.SetActive(false);
    }

    private void HideButtons()
    {
        m_buttonOk.gameObject.SetActive(false);
        m_buttonYes.gameObject.SetActive(false);
        m_buttonNo.gameObject.SetActive(false);
    }

    private void HandleButtonClicked(Button button, System.Action<Button> callback)
    {
        Hide();

        if (callback != null)
            callback(button);
    }
}

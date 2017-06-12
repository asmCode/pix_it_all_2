using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public enum Button : int
    {
        OK = 1,
        Yes = 1 << 1,
        No = 1 << 2,
        Cancel = 1 << 3
    }

    public Text m_labelContent;
    public UnityEngine.UI.Button m_buttonOk;
    public UnityEngine.UI.Button m_buttonYes;
    public UnityEngine.UI.Button m_buttonNo;
    public UnityEngine.UI.Button m_buttonCancel;

    private Button m_defaultBackButton;
    private System.Action<Button> m_callback;

    public static void Show(string text, int buttons, Button defaultBackButton, System.Action<Button> callback)
    {
        // TODO: those lines can be replaced by GetPopupInstance
        var popupContainer = GameObject.Find("PopupContainer");
        if (popupContainer == null)
            return;

        var popup = popupContainer.GetComponentInChildren<Popup>(true);
        if (popup == null)
            return;

        popup.m_defaultBackButton = defaultBackButton;
        popup.m_callback = callback;

        popup.ShowInternal(text, buttons, callback);
    }

    public static bool IsVisible()
    {
        var popup = GetPopupInstance();

        return popup != null && popup.gameObject.activeSelf;
    }

    public static bool HandleBackButton()
    {
        if (!IsVisible())
            return false;

        var popup = GetPopupInstance();
        if (popup == null)
            return false;

        popup.HandleButtonClicked(popup.m_defaultBackButton, popup.m_callback);

        return true;
    }

    private static Popup GetPopupInstance()
    {
        var popupContainer = GameObject.Find("PopupContainer");
        if (popupContainer == null)
            return null;

        var popup = popupContainer.GetComponentInChildren<Popup>(true);
        if (popup == null)
            return null;

        return popup;
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

        if ((buttons & (int)Button.Cancel) == (int)Button.Cancel)
        {
            m_buttonCancel.gameObject.SetActive(true);

            m_buttonCancel.onClick.AddListener(() =>
            {
                HandleButtonClicked(Button.Cancel, callback);
            });
        }

        gameObject.SetActive(true);
    }

    private void Hide()
    {
        m_buttonOk.onClick.RemoveAllListeners();
        m_buttonYes.onClick.RemoveAllListeners();
        m_buttonNo.onClick.RemoveAllListeners();
        m_buttonCancel.onClick.RemoveAllListeners();

        HideButtons();

        gameObject.SetActive(false);
    }

    private void HideButtons()
    {
        m_buttonOk.gameObject.SetActive(false);
        m_buttonYes.gameObject.SetActive(false);
        m_buttonNo.gameObject.SetActive(false);
        m_buttonCancel.gameObject.SetActive(false);
    }

    private void HandleButtonClicked(Button button, System.Action<Button> callback)
    {
        Hide();

        if (callback != null)
            callback(button);
    }
}

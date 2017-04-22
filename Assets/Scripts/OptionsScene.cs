using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsScene : MonoBehaviour
{
    public OptionsView m_optionsView;

    private OptionsController m_optionsController;

    private void Awake()
    {
        m_optionsController = new OptionsController();
        m_optionsController.Init(m_optionsView);
    }
}

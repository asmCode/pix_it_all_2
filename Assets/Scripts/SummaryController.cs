using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummaryController
{
	private const float MaxShowStepsTime = 20.0f;
	private const float MinShowStepsTime = 5.0f;
	private const float PreferedStepTime = 0.05f;

	private enum State
	{
		CompletedBanner,
		Steps
	}

	private bool m_summaryViewVisible = false;

	private State m_state = State.CompletedBanner;
	private Board m_board;
	private SummaryView m_view;
	private Gameplay m_gameplay;

	private string m_levelName;
	private int m_stars;
	private float m_time;
	private bool m_record;
	private float m_currentRecord;
	private float m_timeFor3Stars;
	private float m_timeFor2Stars;

	private float m_stepDelay;
	private float m_stepCooldown;
    private int m_stepIndex;
    private ImageStepTileCoords[] m_steps;
    private BoardController m_boardController;


    private bool m_isActive;

	public bool IsActive
	{
		get { return m_isActive; }
	}

	public SummaryController(Board board, SummaryView view, Gameplay gameplay, BoardController boardController)
	{
		m_board = board;
		m_view = view;
		m_gameplay = gameplay;
        m_boardController = boardController;

        m_view.Clicked += ViewClicked;
    }

    public void Show(
		string levelName,
		int stars,
		float time,
		bool record,
		float currentRecord,
		float timeFor3Stars,
		float timeFor2Stars,
		ImageStepTileCoords[] steps)
	{
		m_isActive = true;

		m_levelName = levelName;
		m_stars = stars;
		m_time = time;
		m_record = record;
		m_currentRecord = currentRecord;
		m_timeFor3Stars = timeFor3Stars;
		m_timeFor2Stars = timeFor2Stars;
        m_steps = steps;

		m_stepDelay = CalculateTimePerPixel();

		m_view.ShowCompletedBanner(() =>
		{
			ShowSteps();
		});

        m_boardController.SmoothZoom(Vector2.zero, m_board.MinScale);
    }

	private float CalculateTimePerPixel()
	{
		int tilesCount = m_gameplay.ImageProgress.Width * m_gameplay.ImageProgress.Height;
		return Mathf.Clamp(tilesCount * PreferedStepTime, MinShowStepsTime, MaxShowStepsTime) / tilesCount;
	}

	private void ShowSummary()
	{
		if (m_summaryViewVisible)
			return;

		m_summaryViewVisible = true;

		m_view.Show(m_levelName, m_stars, m_time, m_record, m_currentRecord, m_timeFor3Stars, m_timeFor2Stars);
	}

	public void Update()
	{
		if (m_state == State.Steps)
			UpdateSteps();
	}

	private void ShowSteps()
	{
		if (m_state == State.Steps)
			return;

        int tilesCount = m_board.m_referenceImage.texture.width * m_board.m_referenceImage.texture.height;
       	var emptyTiles = new bool[tilesCount];
        m_board.SetTiles(emptyTiles);

		m_state = State.Steps;
	}

	private void UpdateSteps()
	{
		if (m_steps == null)
			return;

		if (m_state == State.Steps && m_steps.Length <= m_stepIndex)
		{
			ShowSummary();
			return;
		}

		m_stepCooldown += Time.deltaTime;

		while (m_stepCooldown >= m_stepDelay && m_steps.Length > m_stepIndex)
		{
			m_stepCooldown -= m_stepDelay;
			
			var step = m_steps[m_stepIndex];
			m_stepIndex++;
			var referenceColor = m_gameplay.GetReferenceColor(step.X, step.Y);

			m_board.SetPixel(step.X, step.Y, referenceColor, m_stepCooldown < m_stepDelay);
		}
	}

    private void ViewClicked()
    {
        if (m_state == State.Steps)
            ShowSummary();
    }
}

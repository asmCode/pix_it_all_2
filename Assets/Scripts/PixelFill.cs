using UnityEngine;
using UnityEngine.UI;

public class PixelFill : MonoBehaviour
{
	public Image m_image;
    public Image m_frame;
    public AnimatedImage m_anim;

	private System.Action m_finishedCallback;
    private float m_baseScale = 0.0f;
    private float m_animScale = 1.0f;
    private float m_animScaleProgress = 0.0f;

    public void Play(Color color, System.Action finishedCallback)
	{
		m_finishedCallback = finishedCallback;
		m_image.color = color;
		m_anim.Play();

        if (m_baseScale == 0.0f)
            m_baseScale = Mathf.Abs(transform.localScale.x);

        m_animScaleProgress = 0.0f;
    }

	private void Awake()
	{
	}

    private void Update()
    {
        m_animScaleProgress += Time.deltaTime * 2.0f;
        if (m_animScaleProgress >= 1.0f)
            m_animScaleProgress = 1.0f;

        float curveValue = QuadEaseIn(m_animScaleProgress);

        m_animScale = curveValue * 0.3f + 0.7f;

        float scale = m_baseScale * m_animScale;

        transform.transform.localScale = new Vector3(scale, scale, 1.0f);

        m_frame.color = new Color(1, 1, 1, 1 - curveValue);
    }

    private void OnEnable()
	{
        m_animScaleProgress = 0.0f;

        m_anim.Finished += HandleFinished;
	}

	private void OnDisable()
	{
		m_anim.Finished -= HandleFinished;
	}

	private void HandleFinished()
	{
		if (m_finishedCallback != null)
			m_finishedCallback();
	}

    private float QuadEaseIn(float time)
    {
        return time * time;
    }
}

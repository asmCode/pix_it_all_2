using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureVelocity
{
    private const int NumSamples = 2;

    class Sample
    {
        public Sample(Vector2 position, float time)
        {
            Position = position;
            Time = time;
        }
        public Vector2 Position { get; set; }
        public float Time { get; set; }
    }

    private List<Sample> m_samples = new List<Sample>(NumSamples);

    public void Reset()
    {
        m_samples.Clear();

    }

    public void Track(Vector2 position, float time)
    {
        // Debug.LogFormat("[{0}] Track {1}, {2}, time={3}", Time.frameCount, position.x, position.y, time);

        if (m_samples.Count >= NumSamples)
            m_samples.RemoveAt(0);

        m_samples.Add(new Sample(position, time));
    }

    public Vector2 GetVelocity(float time)
    {
        if (m_samples.Count <= 1)
            return Vector2.zero;

        Vector2 velocity = Vector2.zero;

        //float[] wages = { 0.1f, 0.3f, 0.6f };
        float[] wages = { 1.0f, 1.0f, 1.0f };

        for (int i = m_samples.Count - 1; i >= 1; i--)
        {
            Vector2 segmentVelocity = m_samples[m_samples.Count - 1].Position - m_samples[m_samples.Count - 2].Position;
            segmentVelocity /= m_samples[m_samples.Count - 1].Time - m_samples[m_samples.Count - 2].Time;

            velocity += segmentVelocity * wages[i];
        }

        return velocity;
    }
}

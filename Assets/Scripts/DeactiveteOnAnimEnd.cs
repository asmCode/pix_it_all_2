using UnityEngine;

public class DeactiveteOnAnimEnd : MonoBehaviour
{
	public void AnimEvent_AnimFinished()
    {
        gameObject.SetActive(false);
    }
}

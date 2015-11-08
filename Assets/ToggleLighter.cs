using UnityEngine;
using System.Collections;

public class ToggleLighter : MonoBehaviour
{
    public Transform CapContainerTransform;
    public float CapOpenAngle = 0f;
    public float CapClosedAngle = 90f;
    public float ToggleAnimationDuration = 0.15f;
    public ParticleSystem Flame;

    private bool _isOpen = true;
    private bool _isToggling = false;

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isToggling)
        {
            StartCoroutine(Toggle());
        }
	}

    private IEnumerator Toggle()
    {
        _isToggling = true;

        float t = 0f;

        float initialAngle = _isOpen ? CapOpenAngle : CapClosedAngle;
        float finalAngle = _isOpen ? CapClosedAngle : CapOpenAngle;

        while (t < 1f)
        {
            t += Time.deltaTime / ToggleAnimationDuration;

            float angle = Mathf.LerpAngle(initialAngle, finalAngle, t);

            Vector3 rotation = CapContainerTransform.localEulerAngles;
            rotation.z = angle;

            CapContainerTransform.localEulerAngles = rotation;

            yield return null;
        }

        if (_isOpen)
        {
            Flame.gameObject.SetActive(false);
        }
        else
        {
            Flame.gameObject.SetActive(true);
        }

        _isOpen = !_isOpen;
        _isToggling = false;
    }
}

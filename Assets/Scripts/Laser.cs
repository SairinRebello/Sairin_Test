using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Laser : MonoBehaviour
{
    [SerializeField] private float rotationByDegree = 10;
    [SerializeField] private float maxRotateDegree = 75;
    [SerializeField] private float startTime;

    [SerializeField] private bool mainLaser;
    [SerializeField] private Text levelIndicatorText;
    [SerializeField] private Animator levelIndicatorAnimator;

    private float rotationCount;
    private float maxRotationCount;

    private float waveUpdateTime = 15;
    private int level;

    private bool moveToNegativeDegree;
    private bool readyToRotate = true;
    private bool startRotation;

    private void Start()
    {
        MaxRotationCount();
        StartCoroutine(StartRotation(startTime));
    }

    private void FixedUpdate()
    {
        RotateLaser();
        SpeedUpdater();
    }

    private void RotateLaser()
    {
        if (readyToRotate && startRotation)
        {
            if (rotationCount < maxRotationCount)
            {
                float degree = moveToNegativeDegree ? -rotationByDegree : rotationByDegree;
                transform.Rotate(Vector3.forward, degree, Space.Self);
                rotationCount++;
            }
            else
            {
                readyToRotate = false;
                rotationCount = 0;
                StartCoroutine(Wait(4));
            }
        }
    }

    private void SpeedUpdater()
    {
        waveUpdateTime -= Time.deltaTime;
        if (waveUpdateTime < -10) waveUpdateTime = -10;
        if(waveUpdateTime < 0 && !readyToRotate)
        {
            level += 1;

            if (mainLaser)
            {
                levelIndicatorText.text = $"Level - {level}/5";
                levelIndicatorAnimator.SetBool("animate", true);
                if (level >= 5 && mainLaser)
                {
                    GameManager.Instance.GameWin();
                }
            }

            waveUpdateTime = 15;
            rotationByDegree += .05f;
            MaxRotationCount();
        }
    }

    private void MaxRotationCount()
    {
        maxRotationCount = maxRotateDegree / rotationByDegree;
    }

    private IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        moveToNegativeDegree = moveToNegativeDegree == true ? false : true;
        readyToRotate = true;
    }

    private IEnumerator StartRotation(float startTime)
    {
        yield return new WaitForSeconds(startTime);
        startRotation = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ArrowManager : MonoBehaviour
{
    public static ArrowManager Instance;
    public SteamVR_TrackedObject trackedObj;
    public GameObject arrowPrefab;
    public GameObject stringAttachPoint;
    public GameObject arrowStartPoint;
    public GameObject stringStartPoint;
    public Text scoreText1;
    public Text scoreText2;
    public Text scoreText3;
    public Text scoreText4;
    public float targetsHit;
    private float score;
    private float numberOfShots;
    private float accuracy;
    private float averagePointsPerShot;
    private float lastShotScore;

    private GameObject currentArrow;
    private bool isAttached = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Use this for initialization
    void Start()
    {
        score = 0.00f;
        accuracy = 100.00f;
        averagePointsPerShot = 0.00f;
        lastShotScore = 0.00f;
        scoreText1.text = "Score: " + score.ToString() + "\nArrows loosed: " + numberOfShots + "\nTargets hit: " + targetsHit + "\nScore per shot: " + 
                           averagePointsPerShot.ToString("f2") + "\nAccuracy: 100%" + "\nLast Shot: " + lastShotScore.ToString("f2");
        scoreText2.text = scoreText1.text;
        scoreText3.text = scoreText2.text;
        scoreText4.text = scoreText3.text;  
    }

    // Update is called once per frame
    void Update()
    {
        AttachArrow();
        PullString();
    }

    public void AddScore(float scoreValue, bool bullseye)
    {
        
        if (bullseye == false)
        {
            score += scoreValue;
            lastShotScore += scoreValue;
        } else
        {
            score += scoreValue;
            lastShotScore += scoreValue;
        }
        UpdateScore();
    }

    private void UpdateScore()
    {
        averagePointsPerShot = score / numberOfShots;
        accuracy = targetsHit / numberOfShots * 100;
        scoreText1.text = "Score: " + score.ToString("f2") + "\nArrows shot: " + numberOfShots + "\nTargets hit: " + targetsHit + "\nScore per shot: " + 
                          averagePointsPerShot.ToString("f2") + "\nAccuracy: " + accuracy.ToString("f2") + "%" + "\nLast Shot: " + lastShotScore.ToString("f2");

        scoreText2.text = scoreText1.text;
        scoreText3.text = scoreText2.text;
        scoreText4.text = scoreText3.text;
    }

    private void PullString()
    {
        if (isAttached)
        {
            float stringPositionChange = (stringStartPoint.transform.position - trackedObj.transform.position).magnitude;
            stringAttachPoint.transform.localPosition = stringStartPoint.transform.localPosition + new Vector3(5f * stringPositionChange, 0f, 0f);

            var device = SteamVR_Controller.Input((int)trackedObj.index);
            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                Fire();
                lastShotScore = 0.00f;
                numberOfShots++;
                UpdateScore();
            }
        }
    }

    private void Fire()
    {
        float stringPositionChange = (stringStartPoint.transform.position - trackedObj.transform.position).magnitude;

        currentArrow.transform.parent = null;
        currentArrow.GetComponent<Arrow>().Fired();
        Rigidbody rigidBody = currentArrow.GetComponent<Rigidbody>();
        rigidBody.velocity = currentArrow.transform.forward * 30f * stringPositionChange;
        rigidBody.useGravity = true;
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        currentArrow.GetComponent<Collider>().isTrigger = false;

        stringAttachPoint.transform.position = stringStartPoint.transform.position;

        currentArrow = null;
        isAttached = false;
    }

    public void AttachArrow()
    {
        if (currentArrow == null)
        {
            currentArrow = Instantiate(arrowPrefab);
            currentArrow.transform.parent = trackedObj.transform;
            currentArrow.transform.localPosition = new Vector3(0f, 0f, .3047f);
            currentArrow.transform.localRotation = Quaternion.identity;
        }
    }

    public void AttachBowToArrow()
    {
        currentArrow.transform.parent = stringAttachPoint.transform;
        currentArrow.transform.localPosition = arrowStartPoint.transform.localPosition;
        currentArrow.transform.rotation = arrowStartPoint.transform.rotation;

        isAttached = true;
    }
}

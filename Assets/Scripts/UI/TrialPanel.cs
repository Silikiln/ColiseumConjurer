using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrialPanel : PanelUtility {

    // Timer variables in seconds
    public float countDownDuration = 5.0f;

    //text info
    public Text trialTitle,trialDescription, textTimeCaption;

    public void UpdatePanelDisplay(Trial trialToDisplay)
    {
        //display stuff
        trialTitle.text = trialToDisplay.Name;
        trialDescription.text = trialToDisplay.Description;
    }

    public void StartTimer()
    {
        StartCoroutine(CountDown());
    }

    /// <summary>
    /// Coroutine to start the countdown
    /// </summary>
    private IEnumerator CountDown()
    {
        float countDownTimer = countDownDuration;

        textTimeCaption.text = "Starting in: " + string.Format("{0:0.0}s", countDownTimer);
        textTimeCaption.enabled = true;
        yield return null;

        while (countDownTimer > 0)
        {
            countDownTimer -= Time.deltaTime;
            textTimeCaption.text = "Starting in: " + string.Format("{0:0.0}s", countDownTimer);

            yield return null;
        }

        // Game starting after this, hide the trialPanel
        textTimeCaption.enabled = false;
        TogglePanel();
        ResetText();
        TrialHandler.Instance.LoadTrialScene();
        GameController.Instance.mainCanvas.enabled = false;
    }

}

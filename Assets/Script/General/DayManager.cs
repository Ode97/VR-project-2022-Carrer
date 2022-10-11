using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DayTime{Morning, Afternoon, Evening, Night, All}
public class DayManager : MonoBehaviour
{
    public static DayManager D{get; private set; }
    public GameObject dayLight;
    //public GameObject houseLight;
    [SerializeField]
    public float time;
    public float gameHourInSeconds;
    public TextMeshProUGUI tc;
    public DayTime dayTime;
    private bool start = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (D != null && D != this) {
            Destroy(this.gameObject);
        }else {
            D = this;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            if (time < 24)
            {
                //dayLight.transform.Rotate(new Vector3(360 / (24 * gameHourInSeconds) * Time.deltaTime, 0, 0),
                 //   Space.Self);
                time += Time.deltaTime / gameHourInSeconds;
                DisplayTime(time);
            }
            else
            {
                time = 0;
                DisplayTime(time);
            }

            if (time < 6)
                dayTime = DayTime.Night;
            else if (time < 12)
            {
                //houseLight.SetActive(false);
                dayTime = DayTime.Morning;
            }
            else if (time < 18)
                dayTime = DayTime.Afternoon;
            else
            {
                //houseLight.SetActive(true);
                dayTime = DayTime.Evening;
            }
        }
    }

    public void StartTime()
    {
        start = true;
    }

    public DayTime estimateTimeOfArrive(float t)
    {
        var estimateTimeOfFinish = time + t;
        
        DayTime day;
        
        if(estimateTimeOfFinish < 6){
            day = DayTime.Night;
            
        }else if(estimateTimeOfFinish < 12){
            
            day = DayTime.Morning;
        }else if(estimateTimeOfFinish < 18){
            
            day = DayTime.Afternoon;
        }else if (estimateTimeOfFinish < 24)
        {
            day = DayTime.Evening;
        }
        else
            day = DayTime.Night;

        return day;
    }

    void DisplayTime(float timeToDisplay) {
        float hours = Mathf.FloorToInt(timeToDisplay);
        var minutes = 0;

        if (timeToDisplay - hours > 0.5)
            minutes = 30;
        
        tc.text = $"{hours:00}:{minutes:00}";
    }
}

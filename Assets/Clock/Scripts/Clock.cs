using UnityEngine;
using System;

public class Clock : MonoBehaviour {

    // 主时钟起始时间：默认00:00:00
    public int minutes = 0;
    public int hour = 0;
    public int seconds = 0;
    public bool realTime = true;
    
    public GameObject pointerSeconds;
    public GameObject pointerMinutes;
    public GameObject pointerHours;
    
    // 时间流逝速度因子（1.0f为实时，>1.0f更快，<1.0f更慢）
    public float clockSpeed = 1.0f;
    float msecs = 0;

    // 预设常用的时区ID（适用于 Windows；如果在 Mac/Linux 下，请使用 IANA 标识符）
    private string[] timeZoneIDs = new string[]
    {
         "Eastern Standard Time",
         "Central Standard Time",
         "Pacific Standard Time",
         "China Standard Time",
         "Tokyo Standard Time"
    };

    // 各个时区对应的钟表指针（按上面时区顺序设置）：
    public GameObject[] timezonePointerSeconds;
    public GameObject[] timezonePointerMinutes;
    public GameObject[] timezonePointerHours;

    void Start() 
    {
        // 如果启用实时模式，则用系统时间初始化主时钟
        if (realTime)
        {
            DateTime now = DateTime.Now;
            hour = now.Hour;
            minutes = now.Minute;
            seconds = now.Second;
        }
    }

    void Update() 
    {
        // 模拟主时钟计时（如果不需要模拟而是实时，可以直接用 DateTime.Now）
        msecs += Time.deltaTime * clockSpeed;
        if (msecs >= 1.0f)
        {
            msecs -= 1.0f;
            seconds++;
            if(seconds >= 60)
            {
                seconds = 0;
                minutes++;
                if(minutes >= 60) // 改为>=60确保在60时进位
                {
                    minutes = 0;
                    hour++;
                    if(hour >= 24)
                        hour = 0;
                }
            }
        }

        // 计算主时钟指针旋转角度
        float rotationSeconds = (360f / 60f) * seconds;
        float rotationMinutes = (360f / 60f) * minutes;
        float rotationHours = (360f / 12f) * (hour % 12) + ((360f / (12f * 60f)) * minutes);

        // 更新主时钟指针
        pointerSeconds.transform.localEulerAngles = new Vector3(0f, 0f, rotationSeconds);
        pointerMinutes.transform.localEulerAngles = new Vector3(0f, 0f, rotationMinutes);
        pointerHours.transform.localEulerAngles   = new Vector3(0f, 0f, rotationHours);

        // 获取基准时间：如果是实时模式，则使用系统当前时间；否则，根据内部计时构造一个 DateTime（这里只关注时、分、秒）
        DateTime baseTime;
        if (realTime)
            baseTime = DateTime.Now;
        else
            baseTime = new DateTime(1, 1, 1, hour, minutes, seconds);

        // 对每个预设时区进行转换和指针更新
        if(timeZoneIDs != null &&
           timezonePointerHours != null &&
           timezonePointerMinutes != null &&
           timezonePointerSeconds != null)
        {
            for(int i = 0; i < timeZoneIDs.Length; i++)
            {
                DateTime tzTime;
                try
                {
                    // 获取时区信息，并转换基准时间到目标时区
                    TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneIDs[i]);
                    tzTime = TimeZoneInfo.ConvertTime(baseTime, tz);
                }
                catch(Exception e)
                {
                    // 如果转换失败，直接使用基准时间
                    tzTime = baseTime;
                }

                // 计算该时区的指针旋转角度
                float tzRotationSeconds = (360f / 60f) * tzTime.Second;
                float tzRotationMinutes = (360f / 60f) * tzTime.Minute;
                float tzRotationHours = (360f / 12f) * (tzTime.Hour % 12) + ((360f / (12f * 60f)) * tzTime.Minute);

                // 更新对应时区的指针
                if (i < timezonePointerSeconds.Length && timezonePointerSeconds[i] != null)
                    timezonePointerSeconds[i].transform.localEulerAngles = new Vector3(0f, 0f, tzRotationSeconds);
                if (i < timezonePointerMinutes.Length && timezonePointerMinutes[i] != null)
                    timezonePointerMinutes[i].transform.localEulerAngles = new Vector3(0f, 0f, tzRotationMinutes);
                if (i < timezonePointerHours.Length && timezonePointerHours[i] != null)
                    timezonePointerHours[i].transform.localEulerAngles = new Vector3(0f, 0f, tzRotationHours);
            }
        }
    }
}

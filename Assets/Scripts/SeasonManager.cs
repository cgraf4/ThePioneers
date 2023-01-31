using System.Collections;
using UnityEngine;

public class SeasonManager : MonoBehaviour
{
    [Header("Seasons")]
    [SerializeField] private int currentSeason;
    [SerializeField] private SeasonColorBundle[] seasonColors;
    [SerializeField] private float seasonDuration;
    [SerializeField] private float seasonChangeDuration;
    [SerializeField] private Material groundMaterial;

    IEnumerator Start()
    {
        groundMaterial.color = seasonColors[currentSeason].Color;

        float changeTimer = 0f;
        float percentage = 0f;

        while (true)
        {
            yield return new WaitForSeconds(seasonDuration);

            // wenn mehr als max dann currentSeason = 0;
            currentSeason = (currentSeason + 1) % 4; // 1 2 3 0 1 2 3 0 1 2 3 0 1 2 3 0 1 2 3 

            Color start = seasonColors[currentSeason - 1 < 0 ? 3 : currentSeason - 1].Color;
            Color end = seasonColors[currentSeason].Color;

            changeTimer = 0f;

            while (changeTimer < seasonChangeDuration)
            {
                // Percentage = currentTimer / maxDuration
                changeTimer += Time.deltaTime;
                percentage = changeTimer / seasonChangeDuration;
                groundMaterial.color = Color.Lerp(start, end, percentage);

                yield return null;
            }
        }
    }
}

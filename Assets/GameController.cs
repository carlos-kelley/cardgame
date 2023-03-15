using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// new array called adjectives


public class GameController : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform boardCenter;
    public List<string> adjectives = new List<string>
    {
        "jealous",
        "silly",
        "happy",
        "sad",
        "angry",
        "excited",
        "scared",
        "tired",
        "hungry",
        "bored"
    };

    public void ButtonGenerator()
    {
        System.Random random = new System.Random();
        Vector2 buttonSize = buttonPrefab.GetComponent<RectTransform>().sizeDelta;
        HashSet<int> generatedNumbers = new HashSet<int>();

        for (int i = 0; i < 6; i++)
        {
            int randomNumber;
            do
            {
                randomNumber = random.Next(1, 10);
            } while (generatedNumbers.Contains(randomNumber));
            generatedNumbers.Add(randomNumber);

            int randomAdjectiveIndex = random.Next(0, adjectives.Count);
            // give each number an adjective from the array
            string randomAdjective = adjectives[randomAdjectiveIndex];
            adjectives.RemoveAt(randomAdjectiveIndex);

            Debug.Log("Random number " + (i + 1) + ": " + randomNumber);

            // instantiate button
            if (boardCenter == null)
            {
                Debug.Log("Board center is null");
            }
            else
            {
                GameObject newButtonInstance = Instantiate(buttonPrefab, boardCenter);

                // // set button text
                newButtonInstance.GetComponentInChildren<Text>().text =
                    randomNumber.ToString() + " " + randomAdjective;

                // // set button position
                Vector2 buttonPosition = new Vector2(0, -(buttonSize.y * i) + 150);
                newButtonInstance.GetComponent<RectTransform>().anchoredPosition = buttonPosition;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ButtonGenerator();
    }

    // void Update()
    // {
    //     turnChanger();
    // }
}

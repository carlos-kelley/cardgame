using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CardController : MonoBehaviour
{
    public GameObject button;
    public bool played = false;
    public static int pickCounter = 0;
    public static bool P1Turn = true;
    public static bool PlayPhase = false;
    public static bool comparePhase = false;
    public int playerNumber;

    private RectTransform p1PanelRect;
    private RectTransform p2PanelRect;

    private void Start()
    {
        p1PanelRect = GameObject.Find("P1Deck").GetComponent<RectTransform>();
        p2PanelRect = GameObject.Find("P2Deck").GetComponent<RectTransform>();
    }

    public void MoveToDeck(bool isP1)
    {
        if ((isP1 && P1Turn) || (!isP1 && !P1Turn))
        {
            Transform panelRect = isP1 ? p1PanelRect.transform : p2PanelRect.transform;

            button.transform.SetParent(panelRect);

            // position the cards
            int childCount = panelRect.childCount - 1;
            if (childCount == 0)
            {
                button.transform.localPosition = Vector2.zero;
            }
            else if (childCount == 1)
            {
                button.transform.localPosition = new Vector2(0, 100);
            }
            else if (childCount == 2)
            {
                button.transform.localPosition = new Vector2(0, -100);
            }
            // button should have a variable for which player it belongs to
            button.GetComponent<CardController>().playerNumber = isP1 ? 1 : 2;

            pickCounter++;
            P1Turn = !P1Turn;
            Debug.Log("childCount: " + childCount);
            Debug.Log("Pick counter: " + pickCounter);
            Debug.Log(
                "Player number on card:" + button.GetComponent<CardController>().playerNumber
            );
            Debug.Log("P1Turn: " + P1Turn);
        }
    }

    public void MoveToDeckP1()
    {
        MoveToDeck(true);
    }

    public void MoveToDeckP2()
    {
        MoveToDeck(false);
    }

    public void moveToBoard()
    {
        if (
            played == false
            && P1Turn == true
            && button.GetComponent<CardController>().playerNumber == 1
        )
        {
            button.transform.SetParent(GameObject.Find("P1Board").transform);
            button.transform.localPosition = Vector2.zero;
            played = true;
            P1Turn = !P1Turn;
        }
        else if (
            played == false
            && P1Turn == false
            && button.GetComponent<CardController>().playerNumber == 2
        )
        {
            button.transform.SetParent(GameObject.Find("P2Board").transform);
            button.transform.localPosition = Vector2.zero;
            played = true;
            P1Turn = !P1Turn;
            PlayPhase = false;
            comparePhase = true;
            compareNumbers();
        }
    }

    public void compareNumbers()
    {
        if (played == true && comparePhase == true)
        {
            Debug.Log("Comparing numbers");
            // get the number on the card
            string text = button.GetComponentInChildren<Text>().text; // get the text from the button
            string numberString = string.Concat(Regex.Matches(text, @"\d")); // extract all the digits and concatenate them
            int cardNumber = int.Parse(numberString); // parse the numeric string as an integer

            // get the number on the card in the same position on the other board
            string otherText = GameObject
                .Find("P2Board")
                .transform.GetChild(button.transform.GetSiblingIndex())
                .GetComponentInChildren<Text>()
                .text; // get the text from the button
            string otherNumberString = string.Concat(Regex.Matches(otherText, @"\d")); // extract all the digits and concatenate them
            int otherCardNumber = int.Parse(otherNumberString); // parse the numeric string as an integer
            // compare the numbers
            if (cardNumber > otherCardNumber)
            {
                // if the number on the card is higher, destroy the other card
                Destroy(
                    GameObject
                        .Find("P2Board")
                        .transform.GetChild(button.transform.GetSiblingIndex())
                        .gameObject
                );
            }
            else if (cardNumber < otherCardNumber)
            {
                // if the number on the other card is higher, destroy this card
                Destroy(button);
            }
            else if (cardNumber == otherCardNumber)
            {
                // if the numbers are the same, destroy both cards
                Destroy(button);
                Destroy(
                    GameObject
                        .Find("P2Board")
                        .transform.GetChild(button.transform.GetSiblingIndex())
                        .gameObject
                );
            }
        }
    }

    public void cardMainFunction()
    {
        if (PlayPhase == false && comparePhase == false)
        {
            Debug.Log("Play phase is false");
            TurnChooser();
        }
        else if (PlayPhase == true && comparePhase == false)
        {
            Debug.Log("Play phase is true");
            moveToBoard();
        }
        else if (PlayPhase == false && comparePhase == true)
        {
            Debug.Log("Compare phase is true");
            compareNumbers();
        }
    }

    public void TurnChooser()
    {
        if (P1Turn == true)
        {
            MoveToDeckP1();
            PickCounterEnder();
        }
        else if (P1Turn == false)
        {
            MoveToDeckP2();
            PickCounterEnder();
        }
    }

    public void PickCounterEnder()
    {
        if (pickCounter == 6)
        {
            Debug.Log("Pick counter is 6 - Beginning game");
            pickCounter = 0;
            PlayPhase = true;
        }
    }
}

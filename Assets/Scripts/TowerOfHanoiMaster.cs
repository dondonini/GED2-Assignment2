using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerOfHanoiMaster : MonoBehaviour
{

    [SerializeField]
    private GameObject basePegTablePrefab;

    [SerializeField]
    private GameObject basePiecePrefab;

    [SerializeField]
    private TweenFunction tweenScript;

    [Range(0.1f, 0.5f)]
    [SerializeField]
    private float shrinkSize = 0.4f;

    [SerializeField]
    private enum DifficultyModes
    {
        Easy,
        Medium,
        Hard,
        ComputerMode
    }

    [SerializeField]
    private DifficultyModes currentState;

    [SerializeField]
    private Transform easyModeTarget;

    [SerializeField]
    private Transform hardAndMediumModeTarget;

    private string test;

    // Pegs, Discs
    private List<int> difficultySettings = new List<int>();

    private List<GameObject> allDiscs = new List<GameObject>();
    private List<GameObject> allPegs = new List<GameObject>();

    [SerializeField]
    private List<GameObject> hand = new List<GameObject>();

    [SerializeField]
    private List<GameObject> allDiscsOnPeg1 = new List<GameObject>();
    [SerializeField]
    private List<GameObject> allDiscsOnPeg2 = new List<GameObject>();
    [SerializeField]
    private List<GameObject> allDiscsOnPeg3 = new List<GameObject>();

    private List<GameObject> allDiscsOnPeg4 = new List<GameObject>();

    [SerializeField]
    private List<string> solution = new List<string>();

    private List<string> columnTags = new List<string>();

    private bool amIHoldingSomething = false;

    [Range(0.0f, 5.0f)]
    [SerializeField]
    private float howFastShouldISolve = 0.5f;

    private int numberOfMoves;

    private int numberOfComputerMovesNeeded;

    [SerializeField]
    private Text numberOfMovesText;

    [SerializeField]
    private Text minimumMovesText;

    private string minimumPrefix = "You will need to make at least ";
    private string minimumPostfix = " moves to finish this puzzle.";

    [SerializeField]
    private GameObject mainMenuCanvas;

    [SerializeField]
    private GameObject gameCanvas;

    [SerializeField]
    private GameObject winCanvas;

    [SerializeField]
    private Text winTurnsTaken;

    private bool gameCanvasToggle = false;

    private bool liftDiscBool = false;

    [SerializeField]
    private bool shouldIAnimateDiscs = true;

    public EasingFunction.Ease animationStyleUp;

    public EasingFunction.Ease animationStyleDown;

    public EasingFunction.Ease animationStyleMove;





    private void towerOfHanoiSolver(int numberOfDiscs, char beginning, char buffer, char ending)
    {
        if(numberOfDiscs == 1)
        {
            solution.Add("Move " + beginning.ToString() + " to " + ending);
            //Debug.Log("Move " + beginning.ToString() + " to " + ending);
        }
        else
        {
            towerOfHanoiSolver(numberOfDiscs - 1, beginning, ending, buffer);
            towerOfHanoiSolver(1, beginning, buffer, ending);
            towerOfHanoiSolver(numberOfDiscs - 1, buffer, beginning, ending);
        }
    }

    private void SetupGame()
    {
        switch (currentState)
        {
            case DifficultyModes.Easy:
                Debug.Log("Easy");
                difficultySettings.Add(3); // Number of Pegs
                difficultySettings.Add(3); // Number of Discs
                break;

            case DifficultyModes.Medium:
                Debug.Log("Medium");
                difficultySettings.Add(4); // Number of Pegs
                difficultySettings.Add(4); // Number of Discs
                break;

            case DifficultyModes.Hard:
                Debug.Log("Hard");
                difficultySettings.Add(4); // Number of Pegs
                difficultySettings.Add(6); // Number of Discs
                break;
            case DifficultyModes.ComputerMode:
                Debug.Log("Computer Mode");
                difficultySettings.Add(3); // Number of Pegs
                difficultySettings.Add(14); // Number of Discs
                break;


            default:
                Debug.LogAssertion("You have not selected a difficulty mode");
                break;
        }

        towerOfHanoiSolver(difficultySettings[1], '1', '2', '3');
        numberOfComputerMovesNeeded = solution.Count;

        gameCanvas.SetActive(true);

        minimumMovesText.text = minimumPrefix + numberOfComputerMovesNeeded + minimumPostfix;
        numberOfMovesText.text = numberOfMoves.ToString();

        columnTags.Add("#1");
        columnTags.Add("#2");
        columnTags.Add("#3");
        columnTags.Add("#4");

        GeneratePegs(difficultySettings[0]);
        GenerateDiscs(difficultySettings[1]);
    }

    public void SetDifficultyModeBtn(int whatDifficulty)
    {
        if(whatDifficulty == 0)
        {
            currentState = DifficultyModes.Easy;
            mainMenuCanvas.gameObject.SetActive(false);
            SetupGame();
        }
        else if (whatDifficulty == 1)
        {
            currentState = DifficultyModes.Medium;
            mainMenuCanvas.gameObject.SetActive(false);
            SetupGame();
        }
        else if (whatDifficulty == 2)
        {
            currentState = DifficultyModes.Hard;
            mainMenuCanvas.gameObject.SetActive(false);
            SetupGame();
        }
        else if (whatDifficulty == 3)
        {
            currentState = DifficultyModes.ComputerMode;
            mainMenuCanvas.gameObject.SetActive(false);
            SetupGame();
        }
    }

    private void RecenterCamera(Vector3 whereToMoveCamera)
    {
        Camera.main.transform.position = whereToMoveCamera;
    }

    private void GeneratePegs(int howManyPegs)
    {
        // Create a container in the unity hierarchy to put all the discs into
        GameObject allPegsContainer = new GameObject("AllPegsContainer");

        float offsetX = 0;

        for (int i = 0; i < howManyPegs; i++)
        {
            // Spawn peg
            GameObject peg = Instantiate(basePegTablePrefab, new Vector3(offsetX, 0,0), Quaternion.identity) as GameObject;

            // Add peg to list
            allPegs.Add(peg);

            // Give it a tag so it corresponds to a certain list
            allPegs[i].transform.FindChild("PEG").gameObject.tag = columnTags[i];
            // Give it a tag so it corresponds to a certain list
            allPegs[i].transform.FindChild("BASE").gameObject.tag = columnTags[i];

            // Make it look nice and neat in the inspector
            peg.transform.SetParent(allPegsContainer.transform);

            offsetX += allPegs[i].transform.FindChild("BASE").gameObject.transform.localScale.x;
        }

        if(howManyPegs == 3)
        {
            RecenterCamera(new Vector3(5, 7.5f, -12.5f));
            Camera.main.GetComponent<Orbit>().ChangeTarget(easyModeTarget);
        }
        else if(howManyPegs == 4)
        {
            RecenterCamera(new Vector3(7.5f, 7.5f, -12.5f));
            Camera.main.GetComponent<Orbit>().ChangeTarget(hardAndMediumModeTarget);
        }
        else
        {
            RecenterCamera(new Vector3(10, 7.5f, -10));
            Debug.LogAssertion("Current camera only supports 3/4 pegs");
        }

    }

    private void CheckForWin()
    {
        try
        {

            switch (currentState)
            {
                case DifficultyModes.Easy:
                    if (allDiscsOnPeg3.Count == difficultySettings[1])
                    {
                        Win();
                    }
                    break;

                case DifficultyModes.Medium:
                    if (allDiscsOnPeg4.Count == difficultySettings[1])
                    {
                        Win();
                    }
                    break;

                case DifficultyModes.Hard:
                    if (allDiscsOnPeg4.Count == difficultySettings[1])
                    {
                        Win();
                    }
                    break;

                case DifficultyModes.ComputerMode:
                    if (allDiscsOnPeg3.Count == difficultySettings[1])
                    {
                        Win();
                    }
                    break;

                default:
                    Debug.LogAssertion("You have not selected a difficulty mode");
                    break;
            }
        }
        catch
        {
            Debug.LogAssertion("You did not click a button");
        }
    }

    private void Win()
    {
        gameCanvas.SetActive(false);
        mainMenuCanvas.SetActive(false);

        winTurnsTaken.text = numberOfMoves.ToString();
        winCanvas.SetActive(true);
    }

    private void GenerateDiscs(int howManyDiscs)
    {
        // Create a container in the unity hierarchy to put all the discs into
        GameObject allDiscsContainer = new GameObject("AllDiscsContainer");

        float offsetShrink = shrinkSize;

        for (int i = 0; i < howManyDiscs; i++)
        {
            // Spawn piece at peg
            GameObject piece = Instantiate(basePiecePrefab, allPegs[0].transform.FindChild("INITIALDISCSPAWNING").gameObject.transform.position, Quaternion.identity) as GameObject;

            try
            {
                piece.gameObject.GetComponent<DiscValue>().Value = i;
            }
            catch
            {
                Debug.LogAssertion("This disc does not have a DiscValue.cs script attached!");
            }

            // Give it a tag so it corresponds to a certain list. This case it will always be zero since we spawn all discs on the first peg
            piece.tag = columnTags[0];

            // Add piece to list
            allDiscs.Add(piece);

            // Don't shrink first piece
            if (i != 0)
            {
                // Shrink piece
                piece.transform.localScale -= new Vector3(offsetShrink, 0, offsetShrink);

                // For each piece that is not the first we will need to add the offset so that each piece gets smaller as we spawn in more discs
                offsetShrink += shrinkSize;
            }

            // Make it look nice and neat in the inspector
            piece.transform.SetParent(allDiscsContainer.transform);
        }

        // Copy all discs list to peg #1 list since that is where they spawn
        allDiscsOnPeg1 = new List<GameObject>(allDiscs);

        // Correct their heights when you spawn them all into one location
        CorrectDiscHeights(allDiscs);
    }

    private void CorrectDiscHeights(List<GameObject> discList)
    {
        float currentOffset = 0;
        for(int i = 0; i < discList.Count; i++)
        {
            // every piece that is not the first...double the local scale y by 2 
            if(i != 0)
            {
                currentOffset += discList[i].transform.localScale.y * 2;
            }
            // first piece offset it by just the local scale y 
            else
            {
                currentOffset += discList[i].transform.localScale.y;
            }
            discList[i].transform.position += new Vector3(0, currentOffset, 0);
        }
    }

    private void UpdateDiscPositions(List<GameObject> discList, int whatPegNumber)
    {
        StopAllCoroutines();

        for (int i = 0; i < discList.Count; i++)
        {
            discList[i].gameObject.transform.position = allPegs[whatPegNumber].transform.FindChild("INITIALDISCSPAWNING").gameObject.transform.position;

        }
        CorrectDiscHeights(discList);
     }

    private void PickupDisc(List<GameObject> allDiscsOnPeg)
    { 
        // Get last index in an array and put them into my hand
        hand.Add(allDiscsOnPeg[allDiscsOnPeg.Count - 1]);
        allDiscsOnPeg.RemoveAt(allDiscsOnPeg.Count - 1);
        hand[0].tag = "Untagged";
        try
        {
            hand[0].gameObject.GetComponent<RandomColorOnStart>().ToggleColor();
        }
        catch
        {
            Debug.LogAssertion("Hand index 0 does not have a RandomColorOnStart script  attached");
        }

        if(shouldIAnimateDiscs)
        {
            StartCoroutine(AnimateDisc(hand[0], hand[0].transform.position + new Vector3(0, 5, 0), 1f));
        }
    }

    IEnumerator AnimateDisc(GameObject whatToMove, Vector3 whereTo, float howLong)
    {
        Vector3 endPos = whereTo;

        if (whatToMove.transform.position.y > endPos.y)
        {
            if (whatToMove.transform.position.x == endPos.x)
            {
                Tween(whatToMove.transform, endPos, animationStyleDown, howLong);
            }
            else
            {
                Vector3 pos1 = new Vector3(endPos.x, whatToMove.transform.position.y, whatToMove.transform.position.z);
                Vector3 pos2 = endPos;

                Tween(whatToMove.transform, pos1, animationStyleMove, howLong * 0.5f);

                yield return new WaitForSeconds(howLong / 2);

                Tween(whatToMove.transform, pos2, animationStyleDown, howLong * 0.5f);
            }
        }
        else
        {
            Tween(whatToMove.transform, endPos, animationStyleUp, howLong);
        }
    }

    private void Tween(Transform t, Vector3 e, EasingFunction.Ease s, float d)
    {
        tweenScript.TweenPosition(t, e, s, d);
    }

    private void DropDisc(List<GameObject> allDiscsOnPeg)
    {
        //StopAllCoroutines();


        numberOfMoves++;
        numberOfMovesText.text = numberOfMoves.ToString();

        allDiscsOnPeg.Add(hand[hand.Count - 1]);
        hand.RemoveAt(hand.Count - 1);
        try
        {
            allDiscsOnPeg[allDiscsOnPeg.Count - 1].gameObject.GetComponent<RandomColorOnStart>().ToggleColor();
        }
        catch
        {
            Debug.LogAssertion("allDiscsOnPeg[X] does not have a RandomColorOnStart script  attached");
        }
    }

    // AI - Warning: Hard to read code...
    private IEnumerator MoveDisc(int moveWhere, int toWhere)
    {
        if(moveWhere == 0)
        {
            PickupDisc(allDiscsOnPeg1);
            yield return new WaitForSeconds(howFastShouldISolve);
            if (toWhere == 1)
            {
                DropDisc(allDiscsOnPeg2);
                allDiscsOnPeg2[allDiscsOnPeg2.Count - 1].gameObject.tag = columnTags[1];
                if (shouldIAnimateDiscs)
                {
                    StartCoroutine(AnimateDisc(allDiscsOnPeg2[allDiscsOnPeg2.Count - 1], allPegs[1].transform.FindChild("INITIALDISCSPAWNING").gameObject.transform.position + new Vector3(0, (allDiscsOnPeg2[0].transform.localScale.y * 2) * allDiscsOnPeg2.Count - allDiscsOnPeg2[0].transform.localScale.y, 0), 1f));
                }
                else
                {
                    UpdateDiscPositions(allDiscsOnPeg2, 1);
                }
            }
            else if(toWhere == 2)
            {
                DropDisc(allDiscsOnPeg3);
                allDiscsOnPeg3[allDiscsOnPeg3.Count - 1].gameObject.tag = columnTags[2];
                if (shouldIAnimateDiscs)
                {
                    StartCoroutine(AnimateDisc(allDiscsOnPeg3[allDiscsOnPeg3.Count - 1], allPegs[2].transform.FindChild("INITIALDISCSPAWNING").gameObject.transform.position + new Vector3(0, (allDiscsOnPeg3[0].transform.localScale.y * 2) * allDiscsOnPeg3.Count - allDiscsOnPeg3[0].transform.localScale.y, 0), 1f));
                }
                else
                {
                    UpdateDiscPositions(allDiscsOnPeg3, 2);
                }
            }
        }

        if (moveWhere == 1)
        {
            PickupDisc(allDiscsOnPeg2);
            yield return new WaitForSeconds(howFastShouldISolve);
            if (toWhere == 0)
            {
                DropDisc(allDiscsOnPeg1);
                allDiscsOnPeg1[allDiscsOnPeg1.Count - 1].gameObject.tag = columnTags[0];
                if (shouldIAnimateDiscs)
                {
                    StartCoroutine(AnimateDisc(allDiscsOnPeg1[allDiscsOnPeg1.Count - 1], allPegs[0].transform.FindChild("INITIALDISCSPAWNING").gameObject.transform.position + new Vector3(0, (allDiscsOnPeg1[0].transform.localScale.y * 2) * allDiscsOnPeg1.Count - allDiscsOnPeg1[0].transform.localScale.y, 0), 1f));
                }
                else
                {
                    UpdateDiscPositions(allDiscsOnPeg1, 0);
                }
            }
            else if (toWhere == 2)
            {
                DropDisc(allDiscsOnPeg3);
                allDiscsOnPeg3[allDiscsOnPeg3.Count - 1].gameObject.tag = columnTags[2];
                if (shouldIAnimateDiscs)
                {
                    StartCoroutine(AnimateDisc(allDiscsOnPeg3[allDiscsOnPeg3.Count - 1], allPegs[2].transform.FindChild("INITIALDISCSPAWNING").gameObject.transform.position + new Vector3(0, (allDiscsOnPeg3[0].transform.localScale.y * 2) * allDiscsOnPeg3.Count - allDiscsOnPeg3[0].transform.localScale.y, 0), 1f));
                }
                else
                {
                    UpdateDiscPositions(allDiscsOnPeg3, 2);
                }
            }
        }

        if (moveWhere == 2)
        {
            PickupDisc(allDiscsOnPeg3);
            yield return new WaitForSeconds(howFastShouldISolve);
            if (toWhere == 0)
            {
                DropDisc(allDiscsOnPeg1);
                allDiscsOnPeg1[allDiscsOnPeg1.Count - 1].gameObject.tag = columnTags[0];
                if (shouldIAnimateDiscs)
                {
                    StartCoroutine(AnimateDisc(allDiscsOnPeg1[allDiscsOnPeg1.Count - 1], allPegs[0].transform.FindChild("INITIALDISCSPAWNING").gameObject.transform.position + new Vector3(0, (allDiscsOnPeg1[0].transform.localScale.y * 2) * allDiscsOnPeg1.Count - allDiscsOnPeg1[0].transform.localScale.y, 0), 1f));
                }
                else
                {
                    UpdateDiscPositions(allDiscsOnPeg1, 0);
                }
            }
            else if (toWhere == 1)
            {
                DropDisc(allDiscsOnPeg2);
                allDiscsOnPeg2[allDiscsOnPeg2.Count - 1].gameObject.tag = columnTags[1];
                if (shouldIAnimateDiscs)
                {
                    StartCoroutine(AnimateDisc(allDiscsOnPeg2[allDiscsOnPeg2.Count - 1], allPegs[1].transform.FindChild("INITIALDISCSPAWNING").gameObject.transform.position + new Vector3(0, (allDiscsOnPeg2[0].transform.localScale.y * 2) * allDiscsOnPeg2.Count - allDiscsOnPeg2[0].transform.localScale.y, 0), 1f));
                }
                else
                {
                    UpdateDiscPositions(allDiscsOnPeg2, 1);
                }
            }
        }

        solution.RemoveAt(0);
        if(solution.Count > 0)
        {
            StartCoroutine(MoveDisc(int.Parse(solution[0].Substring(5, 1)) - 1, int.Parse(solution[0].Substring(10, 1)) - 1));
        }
        else
        {
            yield return new WaitForSeconds(2f);
            Win();
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F2))
        {
            shouldIAnimateDiscs = !shouldIAnimateDiscs;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            gameCanvas.gameObject.SetActive(gameCanvasToggle);
            gameCanvasToggle = !gameCanvasToggle;
        }

            if (Input.GetKeyDown(KeyCode.H))
        {
            if(numberOfMoves < 1)
            {
                StartCoroutine(MoveDisc(int.Parse(solution[0].Substring(5, 1)) - 1, int.Parse(solution[0].Substring(10, 1)) - 1));
            }
            else
            {
                Debug.Log("You have already made a move, I am not longer able to help you. Blame the programmer");
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                #region Peg 1
                if (hit.transform.tag == "#1")
                {
                    if (!amIHoldingSomething && allDiscsOnPeg1.Count >= 1)
                    {
                        // Take current list and put it in your hand
                        PickupDisc(allDiscsOnPeg1);
                        amIHoldingSomething = true;
                    }
                    else if(amIHoldingSomething)
                    {
                        if(allDiscsOnPeg1.Count >= 1)
                        {
                            if (hand[0].GetComponent<DiscValue>().Value >= allDiscsOnPeg1[allDiscsOnPeg1.Count - 1].GetComponent<DiscValue>().Value)
                            {
                                // Place hand into peg #1
                                DropDisc(allDiscsOnPeg1);

                                // Take top disc on the peg and update the tag to match its position
                                allDiscsOnPeg1[allDiscsOnPeg1.Count - 1].gameObject.tag = columnTags[0];

                                // Take all discs and move them to their correct peg and run function CorrectDiscHeights(list) too
                                if (shouldIAnimateDiscs)
                                {
                                    StartCoroutine(AnimateDisc(allDiscsOnPeg1[allDiscsOnPeg1.Count - 1], allPegs[0].transform.FindChild("INITIALDISCSPAWNING").gameObject.transform.position + new Vector3(0, (allDiscsOnPeg1[0].transform.localScale.y * 2) * allDiscsOnPeg1.Count - allDiscsOnPeg1[0].transform.localScale.y, 0), 1f));
                                }
                                else
                                {
                                    UpdateDiscPositions(allDiscsOnPeg1, 0);
                                }


                                amIHoldingSomething = false;
                            }
                            else
                            {
                                Debug.Log("Illegal move");
                            }
                        }
                        else
                        {
                            // Place hand into peg #1
                            DropDisc(allDiscsOnPeg1);

                            // Take top disc on the peg and update the tag to match its position
                            allDiscsOnPeg1[allDiscsOnPeg1.Count - 1].gameObject.tag = columnTags[0];

                            // Take all discs and move them to their correct peg and run function CorrectDiscHeights(list) too
                            if (shouldIAnimateDiscs)
                            {
                                StartCoroutine(AnimateDisc(allDiscsOnPeg1[allDiscsOnPeg1.Count - 1], allPegs[0].transform.FindChild("INITIALDISCSPAWNING").gameObject.transform.position + new Vector3(0, (allDiscsOnPeg1[0].transform.localScale.y * 2) * allDiscsOnPeg1.Count - allDiscsOnPeg1[0].transform.localScale.y, 0), 1f));
                            }
                            else
                            {
                                UpdateDiscPositions(allDiscsOnPeg1, 0);
                            }

                            amIHoldingSomething = false;
                        }
                    }
                }
                #endregion

                #region Peg 2 
                else if (hit.transform.tag == "#2")
                {
                    if (!amIHoldingSomething && allDiscsOnPeg2.Count >= 1)
                    {
                        // Take current list and put it in your hand
                        PickupDisc(allDiscsOnPeg2);
                        amIHoldingSomething = true;
                    }
                    else if (amIHoldingSomething)
                    {
                        if (allDiscsOnPeg2.Count >= 1)
                        {
                            if (hand[0].GetComponent<DiscValue>().Value >= allDiscsOnPeg2[allDiscsOnPeg2.Count - 1].GetComponent<DiscValue>().Value)
                            {
                                // Place hand into peg #2
                                DropDisc(allDiscsOnPeg2);
                                allDiscsOnPeg2[allDiscsOnPeg2.Count - 1].gameObject.tag = columnTags[1];
                                if (shouldIAnimateDiscs)
                                {
                                    StartCoroutine(AnimateDisc(allDiscsOnPeg2[allDiscsOnPeg2.Count - 1], allPegs[1].transform.FindChild("INITIALDISCSPAWNING").gameObject.transform.position + new Vector3(0, (allDiscsOnPeg2[0].transform.localScale.y * 2) * allDiscsOnPeg2.Count - allDiscsOnPeg2[0].transform.localScale.y, 0), 1f));
                                }
                                else
                                {
                                    UpdateDiscPositions(allDiscsOnPeg2, 1);
                                }
                                amIHoldingSomething = false;
                            }
                            else
                            {
                                Debug.Log("Illegal move");
                            }
                        }
                        else
                        {
                            // Place hand into peg #2
                            DropDisc(allDiscsOnPeg2);
                            allDiscsOnPeg2[allDiscsOnPeg2.Count - 1].gameObject.tag = columnTags[1];
                            if (shouldIAnimateDiscs)
                            {
                                StartCoroutine(AnimateDisc(allDiscsOnPeg2[allDiscsOnPeg2.Count - 1], allPegs[1].transform.FindChild("INITIALDISCSPAWNING").gameObject.transform.position + new Vector3(0, (allDiscsOnPeg2[0].transform.localScale.y * 2) * allDiscsOnPeg2.Count - allDiscsOnPeg2[0].transform.localScale.y, 0), 1f));
                            }
                            else
                            {
                                UpdateDiscPositions(allDiscsOnPeg2, 1);
                            }
                            amIHoldingSomething = false;
                        }
                    }
                }
                #endregion

                #region Peg 3 
                else if (hit.transform.tag == "#3")
                {
                    if (!amIHoldingSomething && allDiscsOnPeg3.Count >= 1)
                    {
                        // Take current list and put it in your hand
                        PickupDisc(allDiscsOnPeg3);
                        amIHoldingSomething = true;
                    }
                    else if (amIHoldingSomething)
                    {
                        if (allDiscsOnPeg3.Count >= 1)
                        {
                            if (hand[0].GetComponent<DiscValue>().Value >= allDiscsOnPeg3[allDiscsOnPeg3.Count - 1].GetComponent<DiscValue>().Value)
                            {
                                // Place hand into peg #3
                                DropDisc(allDiscsOnPeg3);
                                allDiscsOnPeg3[allDiscsOnPeg3.Count - 1].gameObject.tag = columnTags[2];
                                if (shouldIAnimateDiscs)
                                {
                                    StartCoroutine(AnimateDisc(allDiscsOnPeg3[allDiscsOnPeg3.Count - 1], allPegs[2].transform.FindChild("INITIALDISCSPAWNING").gameObject.transform.position + new Vector3(0, (allDiscsOnPeg3[0].transform.localScale.y * 2) * allDiscsOnPeg3.Count - allDiscsOnPeg3[0].transform.localScale.y, 0), 1f));
                                }
                                else
                                {
                                    UpdateDiscPositions(allDiscsOnPeg3, 2);
                                }
                                amIHoldingSomething = false;
                            }
                            else
                            {
                                Debug.Log("Illegal move");
                            }
                        }
                        else
                        {
                            // Place hand into peg #3
                            DropDisc(allDiscsOnPeg3);
                            allDiscsOnPeg3[allDiscsOnPeg3.Count - 1].gameObject.tag = columnTags[2];
                            if (shouldIAnimateDiscs)
                            {
                                StartCoroutine(AnimateDisc(allDiscsOnPeg3[allDiscsOnPeg3.Count - 1], allPegs[2].transform.FindChild("INITIALDISCSPAWNING").gameObject.transform.position + new Vector3(0, (allDiscsOnPeg3[0].transform.localScale.y * 2) * allDiscsOnPeg3.Count - allDiscsOnPeg3[0].transform.localScale.y, 0), 1f));
                            }
                            else
                            {
                                UpdateDiscPositions(allDiscsOnPeg3, 2);
                            }
                            amIHoldingSomething = false;
                        }
                    }
                }
                #endregion

                #region Peg 4
                else if (hit.transform.tag == "#4")
                {
                    if (!amIHoldingSomething && allDiscsOnPeg4.Count >= 1)
                    {
                        // Take current list and put it in your hand
                        PickupDisc(allDiscsOnPeg4);
                        amIHoldingSomething = true;
                    }
                    else if (amIHoldingSomething)
                    {
                        if (allDiscsOnPeg4.Count >= 1)
                        {
                            if (hand[0].GetComponent<DiscValue>().Value >= allDiscsOnPeg4[allDiscsOnPeg4.Count - 1].GetComponent<DiscValue>().Value)
                            {
                                // Place hand into peg #4
                                DropDisc(allDiscsOnPeg4);
                                allDiscsOnPeg4[allDiscsOnPeg4.Count - 1].gameObject.tag = columnTags[3];
                                if (shouldIAnimateDiscs)
                                {
                                    StartCoroutine(AnimateDisc(allDiscsOnPeg4[allDiscsOnPeg4.Count - 1], allPegs[3].transform.FindChild("INITIALDISCSPAWNING").gameObject.transform.position + new Vector3(0, (allDiscsOnPeg4[0].transform.localScale.y * 2) * allDiscsOnPeg4.Count - allDiscsOnPeg4[0].transform.localScale.y, 0), 1f));
                                }
                                else
                                {
                                    UpdateDiscPositions(allDiscsOnPeg4, 3);
                                }
                                amIHoldingSomething = false;
                            }
                            else
                            {
                                Debug.Log("Illegal move");
                            }
                        }
                        else
                        {
                            // Place hand into peg #4
                            DropDisc(allDiscsOnPeg4);
                            allDiscsOnPeg4[allDiscsOnPeg4.Count - 1].gameObject.tag = columnTags[3];
                            if (shouldIAnimateDiscs)
                            {
                                StartCoroutine(AnimateDisc(allDiscsOnPeg4[allDiscsOnPeg4.Count - 1], allPegs[3].transform.FindChild("INITIALDISCSPAWNING").gameObject.transform.position + new Vector3(0, (allDiscsOnPeg4[0].transform.localScale.y * 2) * allDiscsOnPeg4.Count - allDiscsOnPeg4[0].transform.localScale.y, 0), 1f));
                            }
                            else
                            {
                                UpdateDiscPositions(allDiscsOnPeg4, 3);
                            }
                            amIHoldingSomething = false;
                        }
                    }
                }
                #endregion
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            CheckForWin();
        }
    }
}
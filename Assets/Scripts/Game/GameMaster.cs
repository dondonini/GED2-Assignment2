using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    [Header("References")]

    [ShowOnly]
    [SerializeField]
    private GameData m_GameData;

    [SerializeField]
    private GameObject m_DiscPrefab;

    int TotalMoves = 0;

    // Use this for initialization
    void Start () {
        m_GameData = GameData.GetInstance();
        if (m_GameData == null) Debug.Break();

        //SolveTowers3(m_GameData.TotalDiscs, 1, 2, 3);

        SolveTowersMore(m_GameData.TotalDiscs, 1, 2, new int[] { 1, 2, 3 });
        Debug.Log("Total moves: " + TotalMoves);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SolveTowers3(int count, int source, int dest, int inter)
    {
        if (count == 1)
        {
            Debug.Log("Move " + source + " to " + dest);
            //MoveFromTo(source, dest);//To Draw the action performed
            TotalMoves++;            //keep track of number of moves
        }
        else
        {
            //1- Move n-1 from source to intermediate stand using destination as a
            //spare
            SolveTowers3(count - 1, source, inter, dest);

            //2-Move plate #n from Src to Dest 
            SolveTowers3(1, source, dest, inter);

            //3-Move n-1 plates from intermediate to dest so they sit on plate #n 
            SolveTowers3(count - 1, inter, dest, source);
        }
    }

    private void SolveTowersMore(int count, int source, int dest, int[] pegs)
    {
        if (count == 0 || source == dest)
        {
            return;
        }

        if(count == 1 && pegs.Length > 1)
        {
            Debug.Log("Move " + source + " to " + dest);
            TotalMoves++;
        }

        if (pegs.Length == 3)
        {
            SolveTowers3(count, source, dest, pegs[2]);
        }

        if (pegs.Length >= 3 && count > 0)
        {
            int bestSolution = int.MaxValue;
            int bestScore = int.MaxValue;

            //for (count)
        }
    }


    //def FrameStewartSolution(ndisks, start= 1, end= 4, pegs= set([1, 2, 3, 4])):
    //if ndisks ==0 or start == end: #zero disks require zero moves
    //    return []
    //if  ndisks == 1 and len(pegs) > 1: #if there is only 1 disk it will only take one move
    //    return ["move(%s,%s)"%(start,end)]  
    //if len(pegs) == 3:#3 pegs is well defined optimal solution of 2^n-1
    //    return towers3(ndisks,start,end,pegs)
    //if len(pegs) >= 3 and ndisks > 0:
    //    best_solution = float("inf")
    //    best_score = float("inf")
    //    for kdisks in range(1,ndisks):
    //        helper_pegs = list(pegs.difference([start,end]))
    //        LHSMoves = FrameStewartSolution(kdisks,start,helper_pegs[0],pegs)
    //        pegs_for_my_moves = pegs.difference([helper_pegs[0]]) # cant use the peg our LHS stack is sitting on
    //        MyMoves = FrameStewartSolution(ndisks-kdisks,start,end,pegs_for_my_moves) #misleading variable name but meh 
    //        RHSMoves = FrameStewartSolution(kdisks,helper_pegs[0],end,pegs)#move the intermediat stack to 
    //        if any(move is None for move in [LHSMoves,MyMoves,RHSMoves]):continue #bad path :(
    //        move_list = LHSMoves + MyMoves + RHSMoves
    //        if(len(move_list) < best_score):
    //            best_solution = move_list
    //            best_score = len(move_list)
    //    if best_score < float("inf"):       
    //        return best_solution
    //#all other cases where there is no solution (namely one peg, or 2 pegs and more than 1 disk)
    //return None
}

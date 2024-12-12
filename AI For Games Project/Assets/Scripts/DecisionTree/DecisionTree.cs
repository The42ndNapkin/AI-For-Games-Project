using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree : MonoBehaviour
{
    private DecisionNode rootNode;

    //Constructor
    public DecisionTree(DecisionNode root)
    {
        rootNode = root;
    }
    
    public void setRoot(DecisionNode root)
    {
        rootNode = root;
    }

    public void Execute()
    {
        rootNode.Execute();
    }
}

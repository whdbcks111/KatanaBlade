using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Pathfinder : MonoBehaviour
{
   Agrid _grid;

   public Transform startObject;
   public Transform targetObject;
   private Anode _endNode;

   private void Awake()
   {
      _grid = GetComponent<Agrid>();
   }

   private void Update()
   {
      FindPath(startObject.position, targetObject.position);
   }

   void FindPath(Vector3 startPos, Vector3 targetPos)
   {
      Anode startNode = _grid.GetNodeFromWorldPoint(startPos);
      Anode targetNode = _grid.GetNodeFromWorldPoint(targetPos);

      List<Anode> openList = new List<Anode>();
      HashSet<Anode> closedList = new HashSet<Anode>();
      openList.Add(startNode);

      while (openList.Count>0)
      {
         Anode currentNode = openList[0];
         for (int i = 1; i < openList.Count; i++)
         {
            if (openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost)
            {
               currentNode = openList[i];
            }
         }

         openList.Remove(currentNode);
         closedList.Add(currentNode);

         if (currentNode == targetNode)
         {
            Retracepath(startNode, targetNode);
            return;
         }

         foreach (Anode n in _grid.GetNeighbours(currentNode))
         {
            if (!n.IswalkAbls || closedList.Contains(n))
               continue;
            int newCurrentToNeighboursCost = currentNode.GCost + GetdistanceCost(currentNode, n);
            if (newCurrentToNeighboursCost < n.GCost || !openList.Contains(n))
            {
               n.GCost = newCurrentToNeighboursCost;
               n.HCost = GetdistanceCost(n, targetNode);
               n.ParentNode = currentNode;
               
               if(!openList.Contains(n))
                  openList.Add(n);
            }
         }
      }
   }

   void Retracepath(Anode startNode, Anode endNobe)
   {
      List<Anode> path = new List<Anode>();
      Anode currentNode = _endNode;

      while (currentNode!= startNode)
      {
         path.Add(currentNode);
         currentNode = currentNode.ParentNode;
      }
      path.Reverse();
      _grid.Path = path;
   }

   int GetdistanceCost(Anode nodeA, Anode nodeB)
   {
      int distX = Mathf.Abs(nodeA.Gridx - nodeB.Gridx);
      int distY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

      if (distX > distY)
         return 14 * distY + 10 * (distX - distY);
      return 14 * distX + 10 * (distY - distX);
   }
}

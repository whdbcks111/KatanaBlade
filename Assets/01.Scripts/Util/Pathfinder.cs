using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Pathfinder : MonoBehaviour
{
   Agrid grid;

   public Transform startobject;
   public Transform targetobject;

   private void Awake()
   {
      grid = GetComponent<Agrid>();
   }

   private void Update()
   {
      FindPath(startobject.position, targetobject.position);
   }

   private void FindPath(Vector2 startPos, Vector2 targetPos)
   {
      var startNode = grid.GetNodeFromWorldPoint(startPos);
      var targetNode = grid.GetNodeFromWorldPoint(targetPos);

      var openList = new List<Anode>();
      var closedList = new HashSet<Anode>();
      openList.Add(startNode);

      while (openList.Count>0)
      {
         var currentNode = openList[0];
         for (var i = 1; i < openList.Count; i++)
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
            RetracePath(startNode, targetNode);
            return;
         }

         foreach (Anode n in grid.GetNeighbours(currentNode))
         {
            if (!n.IsWall || closedList.Contains(n))
               continue;
            var newCurrentToNeighboursCost = currentNode.GCost + GetDistanceCost(currentNode, n);
            if (newCurrentToNeighboursCost >= n.GCost && openList.Contains(n)) continue;
            n.GCost = newCurrentToNeighboursCost;
            n.HCost = GetDistanceCost(n, targetNode);
            n.ParentNode = currentNode;
               
            if(!openList.Contains(n))
               openList.Add(n);
         }
      }
   }

   private void RetracePath(Anode startNode, Anode endNode)
   {
      var path = new List<Anode>();
      var currentNode = endNode;

      while (currentNode!= startNode)
      {
         path.Add(currentNode);
         currentNode = currentNode.ParentNode;
      }
      path.Reverse();
      grid.Path = path;
   }

   int GetDistanceCost(Anode nodeA, Anode nodeB)
   {
      var distX = Mathf.Abs(nodeA.Gridx - nodeB.Gridx);
      var distY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

      if (distX > distY)
         return 14 * distY + 10 * (distX - distY);
      return 14 * distX + 10 * (distY - distX);
   }
}

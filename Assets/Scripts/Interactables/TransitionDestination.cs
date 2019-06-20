using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiniGame
{
    // This script is used for transition ending, as a flag of transition destination point
    public class TransitionDestination : MonoBehaviour
    {
        // Differnet transition start points that a destination is able to accept 
        public enum DestinationTag
        {
            A, B, C, D, E, F, G,
        }

        public DestinationTag destinationTag; // Matches the tag chosen on the TransitionPoint that is the destination for
        [Tooltip("This is the game object that has transitioned. For example, the player.")]
        public GameObject transitioningGameObject;
        public UnityEvent onReachDestination;
    }
}

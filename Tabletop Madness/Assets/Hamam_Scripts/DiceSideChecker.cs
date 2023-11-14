using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSideChecker : MonoBehaviour
{
    // this script inside empty game object has a collider to check whoch side of the dice stop
    public GameManager gameManager;
    public float timeToWait = 2;

    private Vector3 diceVelocity;

    // depends on the dots on the dice sides i will decide the number to make it move
    private void OnTriggerStay(Collider other)
    {
        diceVelocity = other.gameObject.GetComponentInParent<Dice>().GetDiceVelocity();

        if(diceVelocity == Vector3.zero)
        {
            switch (other.gameObject.name)
            {
                case "Side1":
                    StartCoroutine(gameManager.GetDiceScore(6, timeToWait));
                    break;
                case "Side2":
                    StartCoroutine(gameManager.GetDiceScore(5, timeToWait));
                    break;
                case "Side3":
                    StartCoroutine(gameManager.GetDiceScore(4, timeToWait));
                    break;
                case "Side4":
                    StartCoroutine(gameManager.GetDiceScore(3, timeToWait));
                    break;
                case "Side5":
                    StartCoroutine(gameManager.GetDiceScore(2, timeToWait));
                    break;
                case "Side6":
                    StartCoroutine(gameManager.GetDiceScore(1, timeToWait));
                    break;

            }
        }
    }
}
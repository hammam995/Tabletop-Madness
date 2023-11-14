using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public AudioClip[] clickingSFX;

    private AudioSource source;
    private Rigidbody diceRigidbody;

    private void Awake()
    {
        source = gameObject.GetComponent<AudioSource>();
        diceRigidbody = GetComponent<Rigidbody>();
    }

    public Vector3 GetDiceVelocity()
    {
        return diceRigidbody.velocity;
    }

    //to remove the dice from the view
    public void RemoveFromView()
    {
        diceRigidbody.useGravity = false;
        diceRigidbody.isKinematic = true; // not making it collide or interact with others
        transform.position = new Vector3(0, 100, 0);
    }

    public void RollDice()
    {
        diceRigidbody.useGravity = true;
        diceRigidbody.isKinematic = false;

        float dirX = Random.Range(0, 500);
        float dirY = Random.Range(0, 500);
        float dirZ = Random.Range(0, 500);

        transform.position = new Vector3(0, 6, 0);
        transform.rotation = Quaternion.identity;

        diceRigidbody.AddForce(transform.up * 500);
        diceRigidbody.AddTorque(dirX, dirY, dirZ);

    }

    public void OnCollisionEnter(Collision collision)
    {
        source.PlayOneShot(clickingSFX[Random.Range(0, clickingSFX.Length)]);
    }

}

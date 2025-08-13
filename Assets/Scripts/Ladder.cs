using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Ladder : MonoBehaviour
{
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] AudioSource sound;

    PlayerController pc;

    void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        pc = other.GetComponentInParent<PlayerController>();
        if (pc == null) return;

        pc.SetClimbState(true, climbSpeed);
        if (sound) { sound.loop = true; sound.Play(); }
    }

    void OnTriggerStay(Collider other)
    {
        if (pc == null) return;

        float axis = 0f;
        if (Input.GetKey(KeyCode.W)) axis += 1f;
        if (Input.GetKey(KeyCode.S)) axis -= 1f;

        pc.SetClimbAxis(axis);
    }

    void OnTriggerExit(Collider other)
    {
        if (pc == null) return;

        pc.SetClimbAxis(0f);
        pc.SetClimbState(false);
        if (sound) sound.Stop();

        pc = null;
    }
}

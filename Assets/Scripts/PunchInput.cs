using UnityEngine;

public class PunchInput : MonoBehaviour
{
    public PunchFist leftFist;
    public PunchFist rightFist;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            rightFist.Punch();

        if (Input.GetMouseButtonDown(1))
            leftFist.Punch();
    }
}
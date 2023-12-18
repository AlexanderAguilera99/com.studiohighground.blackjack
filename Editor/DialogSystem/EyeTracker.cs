
/**
* Copyright (C) Fernando Holguin Weber, and Studio High Ground - All Rights Reserved
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential.
* Written by Fernando Holguin <fernando@studiohighground.com>
* 
* UNDER NO CIRCUMSTANCES IS FERNANDO HOLGUIN WEBER, OR Studio High Ground, ITS PROGRAM
* DEVELOPERS OR SUPPLIERS LIABLE FOR ANY OF THE FOLLOWING, EVEN IF INFORMED OF THEIR
* POSSIBILITY: LOSS OF, OR DAMAGE TO, DATA; DIRECT, SPECIAL, INCIDENTAL, OR INDIRECT
* DAMAGES, OR FOR ANY ECONOMIC CONSEQUENTIAL DAMAGES; OR  LOST PROFITS, BUSINESS,
* REVENUE, GOODWILL, OR ANTICIPATED SAVINGS.
* 
*/


using UnityEngine;

public class EyeTracker : MonoBehaviour
{
    const float CROSS_SIZE = .2f;
    private void OnDrawGizmos()
    {
        var pos = transform.localPosition;
        Gizmos.color = new Color(.77f, 0, .77f);
        Gizmos.DrawWireSphere(pos, CROSS_SIZE * .5f);
        Gizmos.DrawLine(new Vector3(CROSS_SIZE + pos.x, pos.y, pos.z), new Vector3(-CROSS_SIZE + pos.x, pos.y, pos.z));
        Gizmos.DrawLine(new Vector3(pos.x, pos.y + CROSS_SIZE, pos.z), new Vector3(pos.x, pos.y - CROSS_SIZE, pos.z));
        Gizmos.DrawLine(new Vector3(pos.x, pos.y, pos.z + CROSS_SIZE), new Vector3(pos.x, pos.y, pos.z - CROSS_SIZE));

    }
}

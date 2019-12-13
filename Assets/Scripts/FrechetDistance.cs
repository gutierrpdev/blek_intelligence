using System.Collections.Generic;
using UnityEngine;
using System;

/* Complete proof and explanation can be found at:
 * http://www.kr.tuwien.ac.at/staff/eiter/et-archive/cdtr9464.pdf */
public class FrechetDistance
{
    private static float[][] ca;
    private static Vector2[] c1, c2;

    public static float DF(List<Vector2> l1, List<Vector2> l2)
    {
        c1 = l1.ToArray();
        c2 = l2.ToArray();

        Vector2 n1 = c1[0], n2 = c2[0];
        for(int i = 0; i < c1.Length; i++)
        {
            c1[i] -= n1;
        }

        for (int i = 0; i < c2.Length; i++)
        {
            c2[i] -= n2;
        }

        Init(c1.Length, c2.Length);
        return C(c1.Length - 1, c2.Length - 1);
    }

    private static void Init(int p, int q)
    {
        ca = new float[p][];

        for(int i = 0; i < p; i++)
        {
            ca[i] = new float[q];
            for(int j = 0; j < q; j++)
            {
                ca[i][j] = -1.0f;
            }
        }
    }

    private static float C(int i, int j)
    {
        if (ca[i][j] > -1.0f) return ca[i][j];
        else if (i == 0 && j == 0)
            ca[i][j] = Vector2.Distance(c1[0], c2[0]);
        else if (i > 0 && j == 0)
            ca[i][j] = Mathf.Max(C(i - 1, 0), Vector2.Distance(c1[i], c2[0]));
        else if (i == 0 && j > 0)
            ca[i][j] = Mathf.Max(C(0, j - 1), Vector2.Distance(c1[0], c2[j]));
        else if (i > 0 && j > 0)
            ca[i][j] =
                Mathf.Max(Mathf.Min(C(i - 1, j), C(i - 1, j - 1), C(i, j - 1)), Vector2.Distance(c1[i], c2[j]));
        else ca[i][j] = float.MaxValue;
        return ca[i][j];
    }

    public static void Test()
    {
        List<Vector2> a = new List<Vector2>();
        List<Vector2> b = new List<Vector2>();

        a.Add(new Vector2(0, 0));
        b.Add(new Vector2(0, 0));
        a.Add(new Vector2(0, 1));
        b.Add(new Vector2(0, -1));


        Debug.Log("Main>" + DF(a, b));
    }
}

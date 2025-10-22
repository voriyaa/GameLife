using UnityEngine;

public class GameController : MonoBehaviour
{
    public int w = 30, h = 30;
    public float delay = 0.2f;
    public GameObject cell;
    bool[,] a;
    GameObject[,] g;
    float t;
    bool run;

    void Start()
    {
        a = new bool[w, h];
        g = new GameObject[w, h];

        for (int i = 0; i < w; i++)
            for (int j = 0; j < h; j++)
            {
                var obj = Instantiate(cell, new Vector3(i, j), Quaternion.identity);
                g[i, j] = obj;
                a[i, j] = Random.value > 0.8f;
                SetColor(i, j);
            }

        var cam = Camera.main;
        cam.orthographic = true;
        cam.transform.position = new Vector3(w / 2f, h / 2f, -10);
        cam.orthographicSize = Mathf.Max(w, h) / 2f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) run = !run;
        if (Input.GetKeyDown(KeyCode.R)) FillRandom();
        if (Input.GetKeyDown(KeyCode.C)) ResetAll();

        if (!run)
        {
            if (Input.GetMouseButton(0))
            {
                var p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = Mathf.RoundToInt(p.x), y = Mathf.RoundToInt(p.y);
                if (x >= 0 && x < w && y >= 0 && y < h)
                {
                    a[x, y] = !a[x, y];
                    SetColor(x, y);
                }
            }
            return;
        }

        t += Time.deltaTime;
        if (t >= delay)
        {
            t = 0;
            NextGen();
        }
    }

    void NextGen()
    {
        var next = new bool[w, h];
        for (int i = 0; i < w; i++)
            for (int j = 0; j < h; j++)
            {
                int n = 0;
                for (int dx = -1; dx <= 1; dx++)
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;
                        int nx = i + dx, ny = j + dy;
                        if (nx >= 0 && nx < w && ny >= 0 && ny < h && a[nx, ny]) n++;
                    }
                if (a[i, j]) next[i, j] = (n == 2 || n == 3);
                else if (n == 3) next[i, j] = true;
            }

        a = next;

        for (int i = 0; i < w; i++)
            for (int j = 0; j < h; j++)
                SetColor(i, j);
    }

    void SetColor(int i, int j)
    {
        g[i, j].GetComponent<SpriteRenderer>().color = a[i, j] ? Color.black : Color.white;
    }

    void FillRandom()
    {
        for (int i = 0; i < w; i++)
            for (int j = 0; j < h; j++)
            {
                a[i, j] = Random.value > 0.7f;
                SetColor(i, j);
            }
    }

    void ResetAll()
    {
        for (int i = 0; i < w; i++)
            for (int j = 0; j < h; j++)
            {
                a[i, j] = false;
                SetColor(i, j);
            }
    }
}

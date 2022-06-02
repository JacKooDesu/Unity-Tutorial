using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRecorder : MonoBehaviour
{
    [SerializeField] Character2D target;
    [SerializeField] Transform ghost;

    // [SerializeField] Character2D ghost;
    [SerializeField] List<Vector2> positions = new List<Vector2>();
    List<Vector2> replayTemper = new List<Vector2>();

    [SerializeField] bool isReplaying = false;

    int frame = 0;

    private void Start()
    {
        // 後續看需求再加入
        // ghost.GetComponent<Rigidbody2D>().isKinematic = false;
        // ghost.GetComponent<Collider2D>().enabled = false;

        target.onDeadEvent += () =>
        {
            if (positions.Count != 0)
            {
                var checkPoint = target.checkpointPos;
                isReplaying = true;
                
                if (replayTemper.Count == 0)
                {
                    replayTemper.Clear();
                    replayTemper.AddRange(positions);
                    positions.Clear();

                    return;
                }

                var oldDst = Vector2.Distance(replayTemper[replayTemper.Count - 1], checkPoint);
                var newDst = Vector2.Distance(positions[positions.Count - 1], checkPoint);
                if (newDst > oldDst)
                {
                    replayTemper.Clear();
                    replayTemper.AddRange(positions);
                }
                positions.Clear();
            }
        };

        target.onNewCheckpoint += () =>
        {
            replayTemper.Clear();
            positions.Clear();
            isReplaying = false;
        };
    }

    private void FixedUpdate()
    {
        positions.Add(target.transform.position);
        if (isReplaying)
            Replay();
    }

    public void Replay()
    {
        if (replayTemper.Count == 0)
            return;

        if (frame >= replayTemper.Count)
            frame = 0;

        ghost.position = replayTemper[frame];

        frame++;
    }
}

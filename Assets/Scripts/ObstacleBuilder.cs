using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class ObstacleBuilder : MonoBehaviour
{
    public GameObject ObstaclePrefab;
    public Transform GroundTransform;
    public Transform PlayerTransform;
    public float SpawnDistance = 50f;
    public int NumberOfSections = 5;
    public int MinNumberOfObstacles = 2;
    public int MaxNumberOfObstacles = 4;
    public int NumberOfActiveRows = 5;
    public int DestroyOffset = 10;

    private int _timesSpawned = 0;
    private readonly List<GameObject> _renderedObstacles = new List<GameObject>();
    private const float TimeToDestroy = 1f;

    // Update is called once per frame
    void Update()
    {
        if (PlayerTransform.position.z > SpawnDistance * (_timesSpawned - NumberOfActiveRows + 1))
        {
            _timesSpawned++;

            SpawnObstacles();
        }
    }

    private void SpawnObstacles()
    {
        var sectionLength = GroundTransform.localScale.x / NumberOfSections;

        var rndNumber = new Random();
        var numberOfObstacles = rndNumber.Next(MinNumberOfObstacles, MaxNumberOfObstacles + 1);
        var sections = GenerateRandomArray(numberOfObstacles, 0, NumberOfSections);

        var offsetX = sectionLength / 2 + GroundTransform.position.x - GroundTransform.localScale.x / 2;

        // Destroy Old

        foreach (var obstacle in _renderedObstacles.ToList())
        {
            if (PlayerTransform.position.z > obstacle.transform.position.z + DestroyOffset)
            {
                _renderedObstacles.Remove(obstacle);
                Destroy(obstacle, TimeToDestroy);
            }
        }

        // Create New

        foreach (var section in sections)
        {
            Vector3 position = new Vector3(sectionLength * section + offsetX, GroundTransform.position.y + GroundTransform.localScale.y, _timesSpawned * SpawnDistance);
            GameObject obstacle = Instantiate(ObstaclePrefab, position, Quaternion.identity);
            _renderedObstacles.Add(obstacle);
        }
    }

    public static List<int> GenerateRandomArray(int count, int min, int max)
    {
        var random = new Random();

        //  initialize set S to empty
        //  for J := N-M + 1 to N do
        //    T := RandInt(1, J)
        //    if T is not in S then
        //      insert T in S
        //    else
        //      insert J in S
        //
        // adapted for C# which does not have an inclusive Next(..)
        // and to make it from configurable range not just 1.

        if (max <= min || count < 0 ||
                // max - min > 0 required to avoid overflow
                (count > max - min && max - min > 0))
        {
            // need to use 64-bit to support big ranges (negative min, positive max)
            throw new ArgumentOutOfRangeException("Range " + min + " to " + max +
                    " (" + ((Int64)max - (Int64)min) + " values), or count " + count + " is illegal");
        }

        // generate count random values.
        HashSet<int> candidates = new HashSet<int>();

        // start count values before max, and end at max
        for (int top = max - count; top < max; top++)
        {
            // May strike a duplicate.
            // Need to add +1 to make inclusive generator
            // +1 is safe even for MaxVal max value because top < max
            if (!candidates.Add(random.Next(min, top + 1)))
            {
                // collision, add inclusive max.
                // which could not possibly have been added before.
                candidates.Add(top);
            }
        }

        // load them in to a list, to sort
        List<int> result = candidates.ToList();

        // shuffle the results because HashSet has messed
        // with the order, and the algorithm does not produce
        // random-ordered results (e.g. max-1 will never be the first value)
        for (int i = result.Count - 1; i > 0; i--)
        {
            int k = random.Next(i + 1);
            int tmp = result[k];
            result[k] = result[i];
            result[i] = tmp;
        }
        return result;
    }
}

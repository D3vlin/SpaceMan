using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager sharedInstance;
    public List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>();
    public List<LevelBlock> currentLevelBlocks = new List<LevelBlock>();
    public Transform levelStartPosition;

    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateInicialBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateInicialBlocks()
    {
        for (int i = 0; i < 5; i++)
        {
            AddLevelBlock();
        }
    }

    public void AddLevelBlock()
    {
        int randomIdx = Random.Range(0, allTheLevelBlocks.Count);

        LevelBlock block;

        Vector3 spawPosition;

        if (currentLevelBlocks.Count == 0)
        {
            block = Instantiate(allTheLevelBlocks[0]);
            spawPosition = levelStartPosition.position;
        }
        else
        {
            block = Instantiate(allTheLevelBlocks[randomIdx]);
            spawPosition = currentLevelBlocks[currentLevelBlocks.Count - 1].endPoint.position;
        }

        block.transform.SetParent(this.transform, false);
        Vector3 correction = new Vector3(spawPosition.x - block.startPoint.position.x, spawPosition.y - block.startPoint.position.y, 0);
        block.transform.position = correction;

        currentLevelBlocks.Add(block);
    }

    public void RemoveLevelBlock()
    {
        LevelBlock oldBlock = currentLevelBlocks[0];
        currentLevelBlocks.Remove(oldBlock);
        Destroy(oldBlock.gameObject);
    }

    public void RemoveAllLevelBlocks()
    {
        while(currentLevelBlocks.Count > 0)
        {
            RemoveLevelBlock();
        }
    }
}

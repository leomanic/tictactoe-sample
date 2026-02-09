using UnityEngine;

public class BlockController : MonoBehaviour
{
    // [SerializeField] private GameObject blockPrefab;
    [SerializeField] private Block[] blocks;

    public delegate void OnBlockClicked(int index);
    public OnBlockClicked onBlockClicked;

    public void InitBlocks()
    {
        // for (int i = 0; i < 9; i++)
        // {
        //     GameObject blockObject = Instantiate(blockPrefab, transform);
        //     Block block = blockObject.GetComponent<Block>();
        //     block.InitMarker(i);
        // }

        for (int i=0; i < blocks.Length; i++) 
        {
            blocks[i].InitMarker(i, blockIndex => 
            {
                // Debug.Log("(BlockController) Block Clicked: " + blockIndex);
                onBlockClicked?.Invoke(blockIndex);
            });
        }
    }
    
    // Set marker on a specific block
    public void PlaceMarker(int blockIndex, Constants.PlayerType playerType) 
    {
        switch (playerType)
        {
            case Constants.PlayerType.Player1:
                blocks[blockIndex].SetMarker(Block.MarkerType.O);
                break;
            case Constants.PlayerType.Player2:
                blocks[blockIndex].SetMarker(Block.MarkerType.X);
                break;
        }        
    }
}
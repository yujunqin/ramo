using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchController : MonoBehaviour
{
    public Vector2 offset;
    public float direction;

    public bool root = false;
    // Start is called before the first frame update
    void Start()
    {
        // if (root)
        // {
        //     // a test context
        //     var cells = "0";
        //     var rules = new Dictionary<char, string>();
        //     rules['0'] = "1[0]0";
        //     rules['1'] = "11";
        //     cells = PrimitiveTree.Grow(cells, rules);
        //     cells = PrimitiveTree.Grow(cells, rules);
        //     cells = PrimitiveTree.Grow(cells, rules);
        //     cells = PrimitiveTree.Grow(cells, rules);
        //     cells = PrimitiveTree.Grow(cells, rules);
        //     var testPrimitiveParseData = PrimitiveTree.makePrimitiveForest(cells);
        //     // var testPrimitiveTree = PrimitiveTree.Tree(Primitive.Move(1.0f), new List<PrimitiveTree>());
        //     var testPrimitiveTree = testPrimitiveParseData.trees[0];
        //     var testContext = Context.reducePrimitiveTree(testPrimitiveTree);
        //     Context.SpawnBranchesWithContext(gameObject, testContext);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        Render();
    }
    
    void Render()
    {
        var localScale = GetComponent<Transform>().GetChild(0).GetComponent<Transform>().localScale;
        GetComponent<Transform>().GetChild(0).GetComponent<Transform>().localScale = new Vector2(100.0f * offset.x, localScale.y);
        GetComponent<Transform>().localEulerAngles = new Vector3(0.0f, 0.0f, direction);
    }
}

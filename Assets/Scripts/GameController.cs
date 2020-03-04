using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    string cells = "0";
    public Text resultText;
    // public Text costText;
    public ProgressBar progressBar;
    public float maxCost = 35.0f;

    GameObject plantRoot;
    Dictionary<char, string> rules;
    bool won = false;
    bool lost = false;

    // Start is called before the first frame update
    void Start()
    {
        plantRoot = new GameObject();
        rules = new Dictionary<char, string>();
        // initial rules
        rules['1'] = "11";
        rules['0'] = "1[0]0";
        RenderPlant();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RenderPlant()
    {
        Destroy(plantRoot);
        plantRoot = new GameObject();
        var primitiveParseData = PrimitiveTree.makePrimitiveForest(cells);
        // tree pruning, so that the unused tree won't get too large and hang the game
        cells = primitiveParseData.consumedCells;
        Debug.Log(cells);
        var contexts = primitiveParseData.trees.ConvertAll( tr => Context.reducePrimitiveTree(tr));
        contexts.ForEach(ctx => Context.SpawnBranchesWithContext(plantRoot, ctx));
        float cost = 0.0f;
        foreach (var c in contexts)
        {
            cost += Context.countCost(c);
        }
        // costText.text = "Cost used: " + cost.ToString() + "/" + maxCost.ToString();
        float t = cost / maxCost;
        // costText.color = new Color(0.5f + 0.5f * t, 1.0f - 0.5f * t, 0.5f);
        progressBar.BarValue = Mathf.Round((1.0f - t) * 100);
        // check if the cost limit has been exceeded
        if (cost > maxCost)
        {
            Lose();
        }
    }

    public void Lose()
    {
        if (!won && !lost)
        {
            lost = true;
            resultText.text = "You Lose!";
            resultText.color = new Color(1.0f, 0.0f, 0.0f);
        }
    }

    public void Win()
    {
        if (!lost && !won)
        {
            won = true;
            resultText.text = "You Win!";
            resultText.color = new Color(0.0f, 1.0f, 0.0f);
        }
    }

    public void SetOneTo(string cells)
    {
        rules['1'] = cells;
    }

    public void SetZeroTo(string cells)
    {
        rules['0'] = cells;
    }

    public void Grow()
    {
        if (!lost && !won)
        {
            cells = PrimitiveTree.Grow(cells, rules);
            RenderPlant();
        }
    }
    
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

public class Primitive {
    public string kind;
    public float value;

    public static Primitive Move(float val)
    {
        return new Primitive{kind = "move", value = val};
    }

    public static Primitive Turn(float val)
    {
        return new Primitive{kind = "turn", value = val};
    }
}

public class PrimitiveTree {
    public Primitive label;
    public List<PrimitiveTree> subForest;

    public static string printPrimitiveTree(PrimitiveTree tree)
    {
        var str = "{";
        str += " kind: " + tree.label.kind + ",";
        str += " value: " + tree.label.value + ",";
        str += " subForest: [";
        foreach (var subtree in tree.subForest)
        {
            str += " ";
            str += printPrimitiveTree(subtree);
            str += ",";
        }
        str += "]";
        str += "}";
        return str;
    }

    public static PrimitiveTree Tree(Primitive l, List<PrimitiveTree> sub)
    {
        return new PrimitiveTree{label = l, subForest = sub};
    }
    
    public static string Grow(string cells, Dictionary<char, string> rules)
    {
        string result = "";
        foreach (char cell in cells)
        {
            if (rules.ContainsKey(cell))
            {
                result += rules[cell];
            } else {
                result += cell;
            }
        }
        return result;
    }

    public struct ParseData
    {
        public List<PrimitiveTree> trees;
        public string consumedCells;
        public string remainingCells;
    }

    public static ParseData makePrimitiveForest(string cells)
    {
        float moveLength = 0.2f;
        if (cells.Length == 0)
        {
            return new ParseData {
                trees = new List<PrimitiveTree>(),
                consumedCells = "",
                remainingCells = "",
            };
        }
        else
        {
            var head = cells[0];
            var tail = cells.Substring(1);
            var tailParseData = makePrimitiveForest(tail);
            if (head == '0')
            {
                return new ParseData {
                    trees = new List<PrimitiveTree>{PrimitiveTree.Tree(Primitive.Move(moveLength), new List<PrimitiveTree>())},
                    consumedCells = "0",
                    remainingCells = tail,
                };
            }
            else if (head == '1') {
                return new ParseData {
                    trees = new List<PrimitiveTree>{PrimitiveTree.Tree(Primitive.Move(moveLength), tailParseData.trees)},
                    consumedCells = "1" + tailParseData.consumedCells,
                    remainingCells = tailParseData.remainingCells,
                };
            } else if (head == '[') {
                if (tailParseData.remainingCells.Length == 0 || tailParseData.remainingCells[0] != ']') {
                    // No matching right square bracket
                    return new ParseData {
                        trees = new List<PrimitiveTree>{PrimitiveTree.Tree(Primitive.Move(moveLength), tailParseData.trees)},
                        consumedCells = "[" + tailParseData.consumedCells,
                        remainingCells = tailParseData.remainingCells,
                    };
                } else {
                    // Has matching right bracket
                    var tailTail = tailParseData.remainingCells.Substring(1);
                    var tailTailParseData = makePrimitiveForest(tailTail);
                    return new ParseData {
                        trees = new List<PrimitiveTree>{PrimitiveTree.Tree(Primitive.Turn(45.0f), tailParseData.trees),
                                                        PrimitiveTree.Tree(Primitive.Turn(-45.0f), tailTailParseData.trees)},
                        consumedCells = "[" + tailParseData.consumedCells + "]" + tailTailParseData.consumedCells,
                        remainingCells = tailTailParseData.remainingCells,
                    };
                }
            } else if (head == ']') {
                // throw things back
                return new ParseData {
                    trees = new List<PrimitiveTree>{},
                    consumedCells = "",
                    remainingCells = cells,
                };
            }
            return tailParseData;
        }
    }
}

public class Context {
    public Vector2 offset;
    public float direction;
    public float cost;
    public List<Context> subContexts;

    public static string printContext(Context context)
    {
        var str = "{";
        str += " offset: " + context.offset.ToString() + ",";
        str += " direction: " + context.direction.ToString() + ",";
        str += " cost: " + context.cost.ToString() + ",";
        foreach (var subContext in context.subContexts)
        {
            str += printContext(subContext);
        }
        str += "}";
        return str;
    }

    public static float countCost(Context context)
    {
        float cost = context.cost;
        foreach (var subContext in context.subContexts)
        {
            cost += countCost(subContext);
        }
        return cost;
    }

    public static Context reducePrimitiveTree(PrimitiveTree tree)
    {
        Vector2 offset;
        if (tree.label.kind == "move")
        {
            offset = new Vector2(tree.label.value, 0);
        }
        else
        {
            offset = new Vector2(0, 0);
        }
        float direction;
        if (tree.label.kind == "move")
        {
            direction = 0.0f;
        }
        else
        {
            direction = tree.label.value;
        }
        float cost;
        if (tree.label.kind == "move")
        {
            cost = tree.label.value;
        }
        else
        {
            cost = 1.0f;
        }
        List<Context> subContexts = tree.subForest.ConvertAll( tr => reducePrimitiveTree(tr));
        return new Context {
            offset = offset,
            direction = direction,
            cost = cost,
            subContexts = subContexts,
        };
    }

    public static void SpawnBranchesWithContext(GameObject parentObject, Context context)
    {
        var transform = parentObject.GetComponent<Transform>();
        var offset = new Vector2(0.0f, 0.0f);
        if (parentObject.GetComponent<BranchController>())
        {
            offset = parentObject.GetComponent<BranchController>().offset;
        }
        UnityEngine.Object prefab = Resources.Load("Prefabs/Branch");
        GameObject newObject = (GameObject) GameObject.Instantiate(prefab, Vector2.zero, Quaternion.identity, transform);
        newObject.GetComponent<Transform>().localPosition = offset;
        newObject.GetComponent<BranchController>().offset = context.offset;
        newObject.GetComponent<BranchController>().direction = context.direction;
        foreach (var subContext in context.subContexts)
        {
            SpawnBranchesWithContext(newObject, subContext);
        }
    }
}
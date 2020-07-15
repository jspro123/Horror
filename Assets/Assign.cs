using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MirrorProperties {
    public string color;
    public string shape;
    public string area;
    public MirrorProperties(string a, string b, string c) {
        color = a;
        shape = b;
        area = c;
    }
}

public class ExtraFields : MonoBehaviour
{
    public string area;
    public bool goal;
}

public class ClueField : MonoBehaviour
{
    public string clueString;
}

public class Assign : MonoBehaviour {
    public List<MirrorProperties> CartesianProduct(string[] colors, string[] shapes, string[] areas) {
        var configs = new List<MirrorProperties>();
        var cartesianProduct = 
            from color in colors
            from shape in shapes
            from area in areas
            select new {color, shape, area};
        foreach(var pair in cartesianProduct) { 
            var tuple = new MirrorProperties(pair.color, pair.shape, pair.area);
            configs.Add(tuple);
        } 
        return configs;
    }

    public void Shuffle<T>(List<T> ls) {
        int n = ls.Count;
        while (n > 1) {
            n--;
            int i = Random.Range(0,n+1);
            var temp = ls[i];
            ls[i] = ls[n];
            ls[n] = temp;
        }
    }

    public void ChangeColor(GameObject mirror, string colorString) {
        GameObject mirrorFrame = mirror.transform.GetChild(0).gameObject;
        var materials = mirrorFrame.GetComponent<MeshRenderer>().materials;
        Material mat = materials[0];
        Color color;
        if (string.Equals(colorString, "red")) {
            color = Color.red;
        } else if (string.Equals(colorString, "green")) {
            color = Color.green;
        } else {
            color = Color.blue;
        }
        mat.SetColor("_Color", color);
    }

    public GameObject CreateMirror(GameObject circle, GameObject square, GameObject diamond, string shapeString) {
        GameObject newMirror;
        if (string.Equals(shapeString, "circle")) {
            newMirror = Instantiate(circleMirror);
        } else if (string.Equals(shapeString, "square")) {
            newMirror = Instantiate(square);
        } else {
            newMirror = Instantiate(diamond);
        }
        return newMirror;
    }

    public void AssignClues(MirrorProperties goalSpec, List<GameObject> cluePool1, List<GameObject> cluePool2, List<GameObject> cluePool3, GameObject clue) {
        Random random = new Random();
        GameObject randomCluePoint1 = cluePool1[Random.Range(0,cluePool1.Count)];
        GameObject randomCluePoint2 = cluePool2[Random.Range(0,cluePool2.Count)];
        GameObject randomCluePoint3 = cluePool3[Random.Range(0,cluePool3.Count)];
        List<GameObject> cluepoints = new List<GameObject>{randomCluePoint1, randomCluePoint2, randomCluePoint3};
        List<string> mp = new List<string>{goalSpec.color, goalSpec.shape, goalSpec.area};
        Shuffle(cluepoints);
        for(int i = 0; i < cluepoints.Count; i++) {
            GameObject newClue = Instantiate(clue);
            newClue.transform.position = cluepoints[0].transform.position;
            ClueField compField  = newClue.AddComponent(typeof(ClueField)) as ClueField;
            compField.clueString = mp[0];
        }
    }

    public void AssignMirrors(int goalMirror, List<GameObject> positions, GameObject circle, GameObject square, GameObject diamond, List<MirrorProperties> props) {
        for(int i = 0; i < positions.Count; i++) { 
            string color = props[i].color;
            string shape = props[i].shape;
            string area = props[i].area;
            var position = positions[i].transform.position;
            GameObject newMirror = CreateMirror(circleMirror, square, diamond, shape);
            ChangeColor(newMirror, color);
            newMirror.transform.position = position;
            ExtraFields compExtra = newMirror.AddComponent(typeof(ExtraFields)) as ExtraFields;
            compExtra.area = area;
            compExtra.goal = false;
            if (i ==  goalMirror) {
                compExtra.goal = true;
            }
        } 
    }

    public GameObject circleMirror;
    public GameObject squareMirror;
    public GameObject diamondMirror;
    public List<GameObject> mirrorPositions;
    public GameObject clue;
    public List<GameObject> cluePool1;
    public List<GameObject> cluePool2;
    public List<GameObject> cluePool3;
    // public GameObject specialRoom;

    string[] colors = {"red","blue","green"};
    string[] shapes = {"square","diamond","circle"};
    string[] areas = {"1","2","3"};

    void Start() { 
        var CP = CartesianProduct(colors, shapes, areas);
        int mpCount = mirrorPositions.Count;
        if (mpCount > CP.Count) {
            Debug.Log("Number of mirrors is larger than the number of available combinations.");
            return;
        }
        Shuffle(CP);
        int goalMirror = Random.Range(0,mpCount);
        var goalSpec = CP[goalMirror];
        AssignMirrors(goalMirror, mirrorPositions, circleMirror, squareMirror, diamondMirror, CP);
        AssignClues(goalSpec, cluePool1, cluePool2, cluePool3, clue);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class MirrorPositionInfo
{
    public GameObject mirrorPos;
    public LayerMask reflectLayers;
}


public class MirrorProperties {
    public string color;
    public string shape;
    public string number;
    public MirrorProperties(string a, string b, string c) {
        color = a;
        shape = b;
        number = c;
    }
}

public class Assign : MonoBehaviour {
    public List<MirrorProperties> CartesianProduct(string[] colors, string[] shapes, string[] numbers) {
        var configs = new List<MirrorProperties>();
        var cartesianProduct = 
            from color in colors
            from shape in shapes
            from number in numbers
            select new {color, shape, number};
        foreach(var pair in cartesianProduct) { 
            var tuple = new MirrorProperties(pair.color, pair.shape, pair.number);
            configs.Add(tuple);
        } 
        return configs;
    }

    public void Shuffle<T>(List<T> ls) {
        int n = ls.Count;
        while (n > 1) {
            n--;
            int i = UnityEngine.Random.Range(0,n+1);
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

    public void AssignClues(MirrorProperties goalSpec, List<GameObject> cluePool1, List<GameObject> cluePool2, List<GameObject> cluePool3, GameObject clue, GameObject player) {
        GameObject randomCluePoint1 = cluePool1[UnityEngine.Random.Range(0,cluePool1.Count)];
        GameObject randomCluePoint2 = cluePool2[UnityEngine.Random.Range(0,cluePool2.Count)];
        GameObject randomCluePoint3 = cluePool3[UnityEngine.Random.Range(0,cluePool3.Count)];
        List<GameObject> cluepoints = new List<GameObject>{randomCluePoint1, randomCluePoint2, randomCluePoint3};
        List<string> mp = new List<string>{goalSpec.color, goalSpec.shape, goalSpec.number};
        Shuffle(cluepoints);
        for(int i = 0; i < cluepoints.Count; i++) {
            GameObject newClue = Instantiate(clue);
            newClue.transform.SetParent(clueParent.transform);
            newClue.transform.position = cluepoints[i].transform.position;
            Grab grabObject = newClue.GetComponent<Grab>();
            grabObject.player = player;
            TMPro.TextMeshPro text = newClue.transform.Find("TEXT").GetComponent<TMPro.TextMeshPro>();
            text.text = mp[i];
        }
    }

    private void PrepareNewMirror(GameObject newMirror, MirrorPositionInfo positionInfo)
    {
        Transform t = positionInfo.mirrorPos.transform;
        newMirror.transform.SetParent(mirrorParent.transform);

        newMirror.transform.position = t.position;
        newMirror.transform.rotation = t.rotation;
        newMirror.transform.localScale = t.localScale;
        newMirror.tag = positionInfo.mirrorPos.tag;

        for(int i = 0; i < newMirror.transform.childCount; i++)
        {
            newMirror.transform.GetChild(i).gameObject.tag = positionInfo.mirrorPos.tag;
        }

        MirrorScript mirrorScript = newMirror.GetComponent<MirrorScript>();
        mirrorScript.ReflectLayers = positionInfo.reflectLayers;
    }

    public void AssignMirrors(int goalMirror, List<MirrorPositionInfo> mirrors, GameObject circle, GameObject square, GameObject diamond, List<MirrorProperties> props, GameObject player) {
        for(int i = 0; i < mirrors.Count; i++) { 
            string color = props[i].color;
            string shape = props[i].shape;
            string number = props[i].number;

            GameObject newMirror = CreateMirror(circleMirror, square, diamond, shape);
            PrepareNewMirror(newMirror, mirrors[i]);
            ChangeColor(newMirror, color);

            GameObject textComp = newMirror.transform.GetChild(4).gameObject;
            TMPro.TextMeshPro textMesh = textComp.GetComponent<TMPro.TextMeshPro>();
            textMesh.text = number;

            GameObject collideArea = newMirror.transform.GetChild(5).gameObject;
            Interact interactObject = collideArea.GetComponent<Interact>();
            interactObject.player = player;
            interactObject.goal = false;
            if (i == goalMirror) {
                interactObject.goal = true;
            }
        } 
    }

    [Header("Important Prefabs")]
    public GameObject circleMirror;
    public GameObject squareMirror;
    public GameObject diamondMirror;
    public GameObject clue;
    
    [Header("Parents of Instantiated Objects")]
    public GameObject mirrorParent;
    public GameObject clueParent; 

    [Header("Position of mirrors/clues")]
    public GameObject player;
    public List<MirrorPositionInfo> mirrorPositions;
    public List<GameObject> cluePool1;
    public List<GameObject> cluePool2;
    public List<GameObject> cluePool3;


    string[] colors = {"red","blue","green"};
    string[] shapes = {"square","diamond","circle"};
    string[] numbers = {"1","2","3"};

    void Start() { 
        var CP = CartesianProduct(colors, shapes, numbers);
        int mpCount = mirrorPositions.Count;
        if (mpCount > CP.Count) {
            Debug.Log("Number of mirrors is larger than the number of available combinations.");
            return;
        }
        Shuffle(CP);
        int goalMirror = UnityEngine.Random.Range(0,mpCount);
        var goalSpec = CP[goalMirror];
        AssignMirrors(goalMirror, mirrorPositions, circleMirror, squareMirror, diamondMirror, CP, player);
        AssignClues(goalSpec, cluePool1, cluePool2, cluePool3, clue, player);
    }
}

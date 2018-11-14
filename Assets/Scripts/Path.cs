using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MiniJSON;

public class Path
{
    public List<float> X { get; }
    public List<float> Y { get; }
    public List<float> Z { get; }
    public int Length { get; }

    public Path(string jsonString)
    {
        // https://gist.github.com/darktable/1411710

        Dictionary<string, object> dict = Json.Deserialize(jsonString) as Dictionary<string, object>;

        X = convertCoordinatesToFloat(dict["x"]);
        Y = convertCoordinatesToFloat(dict["y"]);
        Z = convertCoordinatesToFloat(dict["z"]);

        Length = X.Count;
    }

    private List<float> convertCoordinatesToFloat(object src)
    {
        return ((List<object>)src).Select(i => float.Parse(i.ToString())).ToList();
    }
}

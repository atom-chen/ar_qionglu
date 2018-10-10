using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using ElviraFrame;

public class JsonHandle : MonoBehaviour
{
    private PointClass status = new PointClass();
 

    private void Add()
    {
        Timepaths item = new Timepaths("1", "1.png");
        List<Timepaths> items = new List<Timepaths>();
        items.Add(item);


        Paths paths = new Paths("20180619", items);
        List<Paths> ppp = new List<Paths>();
        ppp.Add(paths);

        Data data = new Data("3", "555", "666", "3", ppp);
        if (status != null)
        {
            status.data.Add(data);
        }

    }

    private void Save()
    {
        JsonManager.SaveJsonToFile(status);
    }
}

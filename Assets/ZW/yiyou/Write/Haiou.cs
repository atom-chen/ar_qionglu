using ElviraFrame.AB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Haiou : WriteItem
{

    public void Init()
    {
    List<Material> mats = YiyouStaticDataManager.Instance.GetHaiouMaterial();

        int count = Random.Range(20, 50);
        for (int i = 0; i < count; i++)
        {
  GameObject  go=          Instantiate<GameObject>(YiyouStaticDataManager.Instance.LoadGameObject("Seagull"),
                new Vector3(Random.Range(-10f,10f), 2f, Random.Range(-10f, 10f)),
                Quaternion.identity,
                this.transform.parent);
            int index = Random.Range(0, mats.Count - 1);
            go.GetComponentInChildren<SkinnedMeshRenderer>().material = mats[index];

        }
    }


}

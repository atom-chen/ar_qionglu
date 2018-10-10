using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaTanTrigger : ViewTrigger {
    protected override IEnumerator DoSomeThing()
    {
        yield return base.DoSomeThing();
        yield return  new WaitForSeconds(2);
        Photo.sprite = Sprites[1];
        yield return new WaitForSeconds(2);
        Photo.sprite = Sprites[2];

        yield return new WaitForSeconds(2);
        Photo.sprite = Sprites[3];

        yield return new WaitForSeconds(2);
        Photo.sprite = Sprites[4];
    }

    protected override void Stop()
    {
        base.Stop();

        StopCoroutine(DoSomeThing());
    }
}

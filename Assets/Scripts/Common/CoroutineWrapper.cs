using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CoroutineWrapper : MonoBehaviour {
    #region 单例
    static CoroutineWrapper inst;
    private readonly static object mutex = new object();
    public static CoroutineWrapper Inst {
        get {
            lock (mutex) {
                if (inst == null) {
                    var obj = new GameObject("CoroutineWrapper");
                    inst = obj.AddComponent<CoroutineWrapper>();
                }
                return inst;
            }
        }
    }
    void Awake() {
        inst = this;
        DontDestroyOnLoad(this);
    }
    #endregion
    public IEnumerator ExeDelay(int frames,Action ev) {
        for (int i = 0; i < frames;i++ ) {
            yield return new WaitForEndOfFrame();
        }
        ev();
    }
    public IEnumerator ExeDelayS(float sec, Action ev) {
        yield return new WaitForSeconds(sec);
        ev();;
    }
    public static void EXES(float sec, Action ev) {
        inst .StartCoroutine(inst.ExeDelayS(sec,ev));
    }
    public static void EXEF(int frames, Action ev) {
        inst.StartCoroutine(inst.ExeDelay(frames, ev));
    }
    public static void EXEE(Func<IEnumerator> ien) {
        inst.StartCoroutine(ien());
    }
    public Action OnPerFrame;
    void Update() {
        if (OnPerFrame != null)
            OnPerFrame();
    }
}

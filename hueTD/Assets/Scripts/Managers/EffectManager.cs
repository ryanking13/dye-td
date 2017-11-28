using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 이펙트 관리 (처리, 등록, 주기적 실행, 삭제) 클래스
/// </summary>
public class EffectManager : MonoBehaviour {

    private GameManager gm;

    private List<ContinuousEffect> continuousEffects;

    public void init() {
        gm = GameManager.gm;
        continuousEffects = new List<ContinuousEffect>();
    }

    // run continuous effects
    public void RunContinuousEffects() {

        for(int i = continuousEffects.Count-1; i >= 0; i--) {

            bool running = continuousEffects[i].run();

            // if effect ended
            if (!running)
                continuousEffects.RemoveAt(i);
        }
    }

    // process effect
    public void process<T>(Effect effect, T target) {

        // load effect process function
        MethodInfo method = this.GetType().GetMethod(effect.name);
        
        // set parameters
        object[] parameters = new object[effect.paramList.Count + 1];
        parameters[0] = target;

        for (int i = 0; i < effect.paramList.Count; i++) {
            parameters[i + 1] = effect.paramList[i];
        }

        // call function
        method.Invoke(this, parameters);
    }

    #region tower effects

    public void TowerPoison(Enemy e, float damage, float period, float interval) {
        continuousEffects.Add(new TowerPoisonEffect(e, (int)damage, period, interval, Time.time));
    }

    #endregion

}

public class Effect {
    public string name;
    public List<float> paramList;

    public Effect(string name, List<float> paramList) {
        this.name = name;
        this.paramList = paramList;
    }
}

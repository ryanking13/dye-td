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
    private List<AuraEffects> auraEffects;

    public void init() {
        gm = GameManager.gm;
        continuousEffects = new List<ContinuousEffect>();
        auraEffects = new List<AuraEffects>();
    }

    // run aura effects
    public void RunAuraEffects() {
        for(int i = auraEffects.Count-1; i>= 0; i--) {
            auraEffects[i].run();
        }
    }

    // run continuous effects
    public void RunContinuousEffects() {

        for(int i = continuousEffects.Count-1; i >= 0; i--) {

            bool running = continuousEffects[i].run();

            // if effect ended, automatically remove effect
            if (!running)
                continuousEffects.RemoveAt(i);
        }
    }

    public void ClearAuraEffects(GameObject o) {
        for (int i = auraEffects.Count - 1; i >= 0; i--) {
            if (auraEffects[i].clear(o)) {
                auraEffects.RemoveAt(i);
                break;
            }
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

    // cleanup effect
    public void cleanUp(GameObject o) {
        ClearAuraEffects(o);
    }

    #region tower effects

    public void TowerPoison(Enemy e, float damage, float period, float interval) {
        continuousEffects.Add(new TowerPoisonEffect(e, (int)damage, period, interval, gm.GetTime()));
    }

    public void AccelTowers(Tower t, float amount, float range) {
        auraEffects.Add(new TowerAccelerateEffect(t.gameObject, amount, range));
        RunAuraEffects();
    }

    #endregion

}

public class Effect {
    public string name;
    public string type;
    public List<float> paramList;

    public Effect(string name, string type, List<float> paramList) {
        this.name = name;
        this.type = type;
        this.paramList = paramList;
    }
}

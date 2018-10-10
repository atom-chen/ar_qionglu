using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
public class PostEffectsBase : MonoBehaviour {

    protected bool CheckSupport() {
        if (SystemInfo.supportsImageEffects == false) {
            Debug.LogWarning("This platform does not support image effects.");
            return false;
        }

        return true;
    }

    protected void NotSupported() {
        enabled = false;
    }

    protected void CheckResources() {
        bool isSupported = CheckSupport();

        if (isSupported == false) {
            NotSupported();
        }
    }

    protected void Start() {
        CheckResources();
    }

    // Called when need to create the material used by this effect
    protected Material CheckShaderAndCreateMaterial(Shader shader, Material material) {
        if (shader == null) {
            return null;
        }

        if (shader.isSupported && material && material.shader == shader)
            return material;

        if (!shader.isSupported) {
            return null;
        }
        else {
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            if (material)
                return material;
            else 
                return null;
        }
    }
}
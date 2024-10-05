using System;
using UnityEngine;
using MonoMod;
using Mono.Cecil;

class patch_Materials_Manager : Materials_Manager
{
    [MonoModIgnore]
    private BotLab_Controller main_contr;

    public extern void orig_selectFromScene(GameObject obj);
    // Patch selectFromScene to check for collider count if it's a custom shape
    public void selectFromScene(GameObject obj)
    {
        orig_selectFromScene(obj);
        if (this.comp_selected == null)
            return;
        Component_Info comp_info = this.comp_selected.GetComponent<Component_Info>();
        if (comp_info == null)
        {
            Debug.LogWarning("Component info was null when tried to highlight based on colliders");
            return;
        }
        Comp_Material_Edit_Interface mat_ed_int = this.comp_selected.GetComponent<Comp_Material_Edit_Interface>();
        if (mat_ed_int == null)
        {
            Debug.LogWarning("How in hell is Comp_Material_Edit_Interface null????");
            return;
        }
        if (comp_info.comp_type == CompType.Custom && mat_ed_int.GetType() == typeof(Shape_Info_Simple))
        {
            this.main_contr.highliglt_script.removeHighlightsOnGameObjectAndChildren(this.comp_selected);
            if (this.comp_selected.transform.childCount == 2)
            {
                this.main_contr.highliglt_script.highlightGameObjectAndChildren(new Color(0f, 1f, 0f), this.comp_selected);
            } 
            else
            {
                this.main_contr.highliglt_script.highlightGameObjectAndChildren(new Color(1f, 0f, 0f), this.comp_selected);
            }
        }
    }
}
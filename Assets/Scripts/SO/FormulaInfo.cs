using UnityEngine;

namespace MicroJam10.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/FormulaInfo", fileName = "FormulaInfo")]
    public class FormulaInfo : ScriptableObject
    {
        public PropInfo[] Props;
    }
}
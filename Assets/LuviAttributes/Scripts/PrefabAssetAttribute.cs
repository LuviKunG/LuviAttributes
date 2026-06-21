using System;
using UnityEngine;

namespace LuviKunG.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class PrefabAssetAttribute : PropertyAttribute
    {

    }
}
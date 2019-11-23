using System;
using UnityEngine;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Selection
{
    public class SelectionFactory : MonoBehaviour
    {
        public static ISelectable Selection(SelectionName selection)
        {
            switch(selection)
            {
                case SelectionName.ChanceByScore:
                    return new ChanceByScoreSelector();
                case SelectionName.BestSeven:
                    return new BestSeven();
                default:
                    throw new MissingComponentException();
            }
        }
    }
}
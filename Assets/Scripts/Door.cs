using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EOpenDoorConditionCombinationRule
{
    And,
    Or
};

public class Door : MonoBehaviour
{
    [SerializeField]
    private GameObject _doorBody;

    [SerializeField]
    private List<BaseCondition> _conditions = new List<BaseCondition>();

    [SerializeField]
    private EOpenDoorConditionCombinationRule _combinationRule;

    bool _isFullyCompleted = false;

    void Start()
    {
        _doorBody.SetActive(true);

        GameEvents.Instance.onConditionCompleted += OnConditionCompletedAction;
    }

    void OnDestroy()
    {
        GameEvents.Instance.onConditionCompleted -= OnConditionCompletedAction;
    }

    void OnConditionCompletedAction(BaseCondition baseCondition)
    {
        if (_conditions.Contains(baseCondition))
        {
            if (_combinationRule == EOpenDoorConditionCombinationRule.And)
            {
                _isFullyCompleted = _conditions.TrueForAll(c => c.IsConditionCompleted);
            }
            else if (_combinationRule == EOpenDoorConditionCombinationRule.Or)
            {
                foreach(BaseCondition condition in _conditions)
                {
                    _isFullyCompleted = _isFullyCompleted || condition.IsConditionCompleted;
                }
            }
        }

        _doorBody.SetActive(!_isFullyCompleted);
    }

    public void AddCondition(BaseCondition condition)
    {
        _conditions.Add(condition);
    }

    public void SetCombinationRule(EOpenDoorConditionCombinationRule combinationRule)
    {
        _combinationRule = combinationRule;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSpawningState : ShadowState
{
    public ShadowSpawningState(ShadowAbility context, ShadowFactory factory) : base(context, factory)
    {
    }

    protected override void CheckSwitchStates()
    {
        if (!_ctx.Inputs.ability && _ctx.FxInstance != null)
        {
            _ctx.CreateShadow(_ctx.FxInstance.transform);
            Debug.Log("After spawning");
            SwitchState(_factory.Active());
        }
        else if(!_ctx.Inputs.ability && _ctx.FxInstance == null)
        {
            SwitchState(_factory.Inactive());
        }
        else if (_ctx.Inputs.cancel)
        {
            
            SwitchState(_factory.Inactive());
        }
    }

    public override void EnterState()
    {
        Debug.Log("spawning");
    }

    protected override void ExitState()
    {
        _ctx.DestroyFX();
        Debug.Log("Exited Spawnig");
    }

    protected override void InitializeSubState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        CheckAvailiblePosition();

        CheckSwitchStates();
    }

    private void CheckAvailiblePosition()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawRay(Camera.main.transform.position, ray.direction, new Color(0,0,1));


        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _ctx.AbilityRange, _ctx.AbilityLayerMask))
        {
            Debug.Log(hit.normal);

            if (hit.normal != new Vector3(0, 1 , 0) && hit.collider is BoxCollider) 
            {
                float length = hit.transform.localScale.x * ((BoxCollider)hit.collider).size.x;
                float width = hit.transform.localScale.z * ((BoxCollider)hit.collider).size.z;
                float height = hit.transform.localScale.y * ((BoxCollider)hit.collider).size.y;
                Vector3 dimensions = new Vector3(length, height, width);

                float topPoint = hit.transform.position.y + dimensions.y / 2;
                Vector3 topPointPos = new Vector3(hit.point.x, topPoint, hit.point.z);


                if (_ctx.FxInstance != null)
                {
                    _ctx.FxInstance.transform.position = topPointPos + ray.direction * 0.2f;
                }
                else
                {
                    _ctx.InstantiateFX(topPointPos, Quaternion.identity);
                }
            }
            else
            {
                if (_ctx.FxInstance != null)
                {
                    _ctx.FxInstance.transform.position = hit.point;
                }
                else
                {
                    _ctx.InstantiateFX(hit.point, Quaternion.identity);
                }
            }
        }
        Debug.Log("Update runned");
    }
}

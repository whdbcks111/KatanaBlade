using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAdapter : MonoBehaviour
{
    public Item NowItem;
    public KeyCode UseItemKey;
    private Item _lastItem;
    private bool _canActivityUse;

    // Start is called before the first frame update
    void Start()
    {
        _lastItem = NowItem;
        _canActivityUse = true;
        if (NowItem != null)
            NowItem.OnAcceptItem();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(UseItemKey) && _canActivityUse == true) {
            if(NowItem != null)
            {
                NowItem.OnActiveUse();
                StartCoroutine(ActivityUseCor(NowItem.Cooldown));
            }
        }
        if(NowItem != null)
        {
            NowItem.PassiveUpdate();
        }
        if(_lastItem != NowItem)
        {
            _lastItem = NowItem;
            _canActivityUse = true;
            NowItem.OnAcceptItem();
        }
    }

    private IEnumerator ActivityUseCor(float cooldown)
    {
        _canActivityUse = false;
        yield return new WaitForSeconds(cooldown);
        _canActivityUse = true;
    }
}

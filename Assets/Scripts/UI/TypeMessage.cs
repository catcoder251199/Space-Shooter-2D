using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textUI;
    [SerializeField, TextArea] private string[] _msgList;
    [SerializeField] private bool _isLoop;
    [SerializeField] private float _delayPerMessage;
    [SerializeField] private int _charactersPerMinute;
    [SerializeField] private bool _typeOnStart;

    private float _delayPerChar;
    private Coroutine _typeRoutine;

    public void Start()
    {
        if (_typeOnStart)
            StartType();
    }

    public void SetMessageList(string[] list) => _msgList = list;

    public void StartType()
    {
        _delayPerChar = 60f / _charactersPerMinute; // seconds
        if (_typeRoutine != null)
            StopCoroutine(_typeRoutine);
        _typeRoutine = StartCoroutine(TypeRoutine());
    }

    public void StartType(bool isLoop)
    {
        _isLoop = isLoop;
        StartType();
    }

    public void StopType()
    {
        if (_typeRoutine != null)
            StopCoroutine(_typeRoutine);
        _typeRoutine = null;
    }

    private IEnumerator TypeRoutine()
    {
        if (_msgList != null && _msgList.Length > 0)
        {
            int i = 0;
            while (i  < _msgList.Length)
            {
                string msg = _msgList[i++];
                if (msg != null)
                    yield return StartCoroutine(TypeMessageRoutine(msg));
                if (_isLoop)
                    i = i % _msgList.Length;
                yield return new WaitForSeconds(_delayPerMessage);
            }
        }
    }

    private IEnumerator TypeMessageRoutine(string msg)
    {
        string text = "";
        _textUI.text = text;
        for (int i = 0; i < msg.Length; i++)
        {
            text += msg[i];
            _textUI.text = text;
            yield return new WaitForSeconds(_delayPerChar);
        }
    }
}

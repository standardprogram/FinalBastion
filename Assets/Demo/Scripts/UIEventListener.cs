﻿//------------------------------------------------------------------------------
// Copyright 2016 Baofeng Mojing Inc. All rights reserved.
//------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class UIEventListener : MonoBehaviour
{

    public delegate void VoidDelegate(GameObject go);
    public delegate void BoolDelegate(GameObject go, bool state);
    public delegate void FloatDelegate(GameObject go, float delta);
    public delegate void VectorDelegate(GameObject go, Vector2 delta);
    public delegate void StringDelegate(GameObject go, string text);
    public delegate void ObjectDelegate(GameObject go, GameObject draggedObject);
    public delegate void KeyCodeDelegate(GameObject go, KeyCode key);

    public object parameter;

    public VoidDelegate onSubmit;
    public VoidDelegate onClick;
    public VoidDelegate onDoubleClick;
    public BoolDelegate onHover;
    public BoolDelegate onPress;
    public BoolDelegate onSelect;
    public FloatDelegate onScroll;
    public VectorDelegate onDrag;
    public ObjectDelegate onDrop;
    public StringDelegate onInput;
    public KeyCodeDelegate onKey;

    void OnSubmit() { if (onSubmit != null) onSubmit(gameObject); }
    void OnClick() { if (onClick != null) onClick(gameObject); }
    void OnDoubleClick() { if (onDoubleClick != null) onDoubleClick(gameObject); }
    void OnHover(bool isOver) { if (onHover != null) onHover(gameObject, isOver); }
    void OnPress(bool isPressed) { if (onPress != null) onPress(gameObject, isPressed); }
    void OnSelect(bool selected) { if (onSelect != null) onSelect(gameObject, selected); }
    void OnScroll(float delta) { if (onScroll != null) onScroll(gameObject, delta); }
    void OnDrag(Vector2 delta) { if (onDrag != null) onDrag(gameObject, delta); }
    void OnDrop(GameObject go) { if (onDrop != null) onDrop(gameObject, go); }
    void OnInput(string text) { if (onInput != null) onInput(gameObject, text); }
    void OnKey(KeyCode key) { if (onKey != null) onKey(gameObject, key); }

    /// <summary>
    /// Get or add an event listener to the specified game object.
    /// </summary>

    static public UIEventListener Get(GameObject go)
    {
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if (listener == null) listener = go.AddComponent<UIEventListener>();
        return listener;
    }
}

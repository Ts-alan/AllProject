using System;

/// <summary>
/// Summary description for FilterState
/// </summary>
public class FilterState
{
    private bool _or = false;
    private bool _not = false;
    private bool _isSelected = false;
    private object _content = null;

    public bool Or
    {
        get { return _or; }
        set { _or = value; }
    }

    public bool Not
    {
        get { return _not; }
        set { _not = value; }
    }

    public bool IsSelected
    {
        get { return _isSelected; }
        set { _isSelected = value; }
    }

    public object Content
    {
        get { return _content; }
        set { _content = value; }
    }

    public FilterState()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public FilterState(bool or, bool not, bool isSelected, object content)
    {
        this._or = or;
        this._not = not;
        this._isSelected = isSelected;
        this._content = content;
    }
}

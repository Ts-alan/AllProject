using System;
using System.Collections.Generic;
using System.Text;

public interface IFilter
{
    string GenerateSQL();
    void Clear();
    bool Validate();
    object SaveState();
    void LoadState(object savedState);
    string GetID();
}

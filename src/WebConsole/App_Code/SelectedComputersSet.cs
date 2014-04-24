using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.Vba32CC.DataBase;

public class SelectedComputersSet
{
    #region Properties

    private ComputersEntityCollection _AllComputers;
    public ComputersEntityCollection AllComputers
    {
        get { return _AllComputers; }
        set { _AllComputers = value; }
    }

    private ComputersEntityCollection _VSISComputers;
    public ComputersEntityCollection VSISComputers
    {
        get { return _VSISComputers; }
        set { _VSISComputers = value; }
    }

    private ComputersEntityCollection _OtherComputers;
    public ComputersEntityCollection OtherComputers
    {
        get { return _OtherComputers; }
        set { _OtherComputers = value; }
    }

    #endregion

    #region Constructors

    public SelectedComputersSet()
    {
        _AllComputers = new ComputersEntityCollection();
        _VSISComputers = new ComputersEntityCollection();
        _OtherComputers = new ComputersEntityCollection();
    }

    #endregion

}

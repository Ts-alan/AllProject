using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Text;
using System.Runtime.InteropServices;

public struct AuthInfo
{
    public VbaKeyState KeyState;
    public UInt32 ComputerLimit;
    public DateTime ExpirationDate;
    public UInt32 LicenseNumber;
    public string CustomerName;

    public AuthInfo(byte[] input)
    {
        this.KeyState = (VbaKeyState)BitConverter.ToUInt32(input, 0);
        this.ComputerLimit = BitConverter.ToUInt32(input, 4);
        byte[] inputExpirationDate = new byte[16];
        Array.Copy(input, 8, inputExpirationDate, 0, 16);

        this.ExpirationDate = (new SystemTime(inputExpirationDate)).GetDataTime();
        this.LicenseNumber = BitConverter.ToUInt32(input, 24);
        this.CustomerName =
             Encoding.Default.GetString(input, 28, 128).TrimEnd('\0');
    }
}

[StructLayout(LayoutKind.Sequential)]
struct SystemTime
{
    [MarshalAs(UnmanagedType.U2)]
    public ushort Year;
    [MarshalAs(UnmanagedType.U2)]
    public ushort Month;
    [MarshalAs(UnmanagedType.U2)]
    public ushort DayOfWeek;
    [MarshalAs(UnmanagedType.U2)]
    public ushort Day;
    [MarshalAs(UnmanagedType.U2)]
    public ushort Hour;
    [MarshalAs(UnmanagedType.U2)]
    public ushort Minute;
    [MarshalAs(UnmanagedType.U2)]
    public ushort Second;
    [MarshalAs(UnmanagedType.U2)]
    public ushort Milliseconds;

    public SystemTime(byte[] input)
    {
        this.Year = BitConverter.ToUInt16(input, 0);
        this.Month = BitConverter.ToUInt16(input, 2);
        this.DayOfWeek = BitConverter.ToUInt16(input, 4);
        this.Day = BitConverter.ToUInt16(input, 6);
        this.Hour = BitConverter.ToUInt16(input, 8);
        this.Minute = BitConverter.ToUInt16(input, 10);
        this.Second = BitConverter.ToUInt16(input, 12);
        this.Milliseconds = BitConverter.ToUInt16(input, 14);

    }

    public DateTime GetDataTime()
    {
        try
        {
            return new DateTime(this.Year, this.Month,
                this.Day, this.Hour, this.Minute, this.Second,
                this.Milliseconds);
        }
        catch
        {
            return DateTime.Now;
        }

    }
}

public enum VbaKeyState
{
    Success,             // Ключ в порядке, можно работать
    NotFound,            // Не найден ключевой файл
    BadKey,              // Ошибка при проверке подписи ключа
    NoSection,           // Нет секции для нужной программы
    Expired,             // Время действия ключа истекло
    InvalidTime,         // Время установлено неправильно
    RolledBackTime       // Время откручено назад хитрым пользователем
}

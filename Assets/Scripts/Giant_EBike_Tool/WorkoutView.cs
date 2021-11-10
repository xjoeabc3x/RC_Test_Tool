using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkoutView : MonoBehaviour
{
    [SerializeField, Header("[2B/2E切換]")]
    Toggle CmdSwitch;
    [SerializeField, Header("[剩餘時間]")]
    InputField RemainTime;
    [SerializeField, Header("[剩餘距離]")]
    InputField RemainDis;
    [SerializeField, Header("[剩餘卡路里]")]
    InputField RemainCal;
    [SerializeField, Header("[心率]")]
    InputField HeartRate;
    [SerializeField, Header("[GPS狀態]")]
    Toggle GPSStatus;
    [SerializeField, Header("[海拔]")]
    InputField Altitude;
    [SerializeField, Header("[海拔標題[2B]]")]
    Text Altitude_tittle2B;
    [SerializeField, Header("[海拔標題[2E]]")]
    Text Altitude_tittle2E;
    [SerializeField, Header("[時制]")]
    Dropdown TimeFormat;
    [SerializeField, Header("[小時]")]
    InputField Hour;
    [SerializeField, Header("[分鐘]")]
    InputField Minute;
    [SerializeField, Header("[手機電量]")]
    InputField PhoneBattery;
    [SerializeField, Header("[已消耗卡路里]")]
    InputField BurnedCal;

    private void OnDisable()
    {
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "1A_normal", null, null);
    }

    public void SendCmd()
    {
        bool NoInputError = (CmdSwitch.isOn) ? Check2E() : Check2B();
        if (!NoInputError)
            return;
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device is not connected.");
            return;
        }
        if (CmdSwitch.isOn)
            Send_2E();
        else
            Send_2B();
    }

    private void Send_2B()
    {
        string sendbyte = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", GetRemainTime(), GetRemainDis(), GetRemainCal(), GetHeartRate(), 
            GetGPSStatus(), GetAltitude(), GetMin(), GetByte13());
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "1A_workout", null, null);
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "2B", null, sendbyte.Split(','));
    }

    private void Send_2E()
    {
        string sendbyte = string.Format("00,{0},{1},{2},{3},{4},{5},{6},01,{7},{8},{9}", GetRemainTime(), GetRemainDis(), GetRemainCal(), GetHeartRate(),
            GetAltitude(), GetMin(), GetByte13(), GetGPSStatus(), GetPhoneBattery(), GetBurnedCal());
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "1A_workout", null, null);
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "2E", null, sendbyte.Split(','));
    }

    public void SetAltitudeTittle(bool status)
    {
        Altitude_tittle2B.enabled = !status;
        Altitude_tittle2E.enabled = status;
    }

    #region [Compute]

    private string GetRemainTime()
    {
        string grt = int.Parse(RemainTime.text).ToString("X6");
        return string.Format("{4}{5},{2}{3},{0}{1}", grt[0].ToString(), grt[1].ToString(), grt[2].ToString(), grt[3].ToString(), grt[4].ToString(), grt[5].ToString());
    }

    private string GetRemainDis()
    {
        string grd = int.Parse(RemainDis.text).ToString("X6");
        return string.Format("{4}{5},{2}{3},{0}{1}", grd[0].ToString(), grd[1].ToString(), grd[2].ToString(), grd[3].ToString(), grd[4].ToString(), grd[5].ToString());
    }

    private string GetRemainCal()
    {
        string grc = int.Parse(RemainCal.text).ToString("X4");
        return string.Format("{2}{3},{0}{1}", grc[0].ToString(), grc[1].ToString(), grc[2].ToString(), grc[3].ToString());
    }

    private string GetHeartRate()
    {
        return int.Parse(HeartRate.text).ToString("X2");
    }

    private string GetGPSStatus()
    {
        return (GPSStatus.isOn) ? "01" : "00";
    }

    private string GetAltitude()
    {
        if (!CmdSwitch.isOn) //2B
        {
            string alt = int.Parse(Altitude.text).ToString("X4");
            return string.Format("{2}{3},{0}{1}", alt[0].ToString(), alt[1].ToString(), alt[2].ToString(), alt[3].ToString());
        }
        else //2E
        {
            int input = int.Parse(Altitude.text);
            if (input < 0)
            {
                input += 65536;
            }
            string alt = input.ToString("X4");
            return string.Format("{2}{3},{0}{1}", alt[0].ToString(), alt[1].ToString(), alt[2].ToString(), alt[3].ToString());
        }
    }

    private string GetMin()
    {
        return int.Parse(Minute.text).ToString("X2");
    }

    private string GetByte13()
    {
        int ap = 0;
        switch (TimeFormat.value)
        {
            case 0:
                ap = 0;
                break;
            case 1:
                ap = 32;
                break;
            case 2:
                ap = 64;
                break;
        }
        int clk_h = int.Parse(Hour.text);
        return (ap + clk_h).ToString("X2");
    }

    private string GetPhoneBattery()
    {
        return int.Parse(PhoneBattery.text).ToString("X2");
    }

    private string GetBurnedCal()
    {
        string accal = int.Parse(BurnedCal.text).ToString("X4");
        return string.Format("{2}{3},{0}{1}", accal[0].ToString(), accal[1].ToString(), accal[2].ToString(), accal[3].ToString());
    }

    #endregion

    #region [Check Items]
    private bool Check2B()
    {
        return CheckRemainTime() & CheckRemainDis() & CheckRemainCal() & CheckHeartRate() & CheckAltitude() & CheckHour() & CheckMin();
    }

    private bool Check2E()
    {
        return CheckRemainTime() & CheckRemainDis() & CheckRemainCal() & CheckHeartRate() & CheckAltitude() & CheckHour() & CheckMin()
            & CheckPhoneBattery() & CheckBurnedCal();
    }

    private bool CheckRemainTime()
    {
        if (string.IsNullOrEmpty(RemainTime.text))
        {
            Toast.Instance.ShowToast("Remaining time is empty.");
            return false;
        }
        if (int.Parse(RemainTime.text) < 0 || int.Parse(RemainTime.text) > 16777215)
        {
            Toast.Instance.ShowToast("Remaining time input error.");
            return false;
        }
        return true;
    }

    private bool CheckRemainDis()
    {
        if (string.IsNullOrEmpty(RemainDis.text))
        {
            Toast.Instance.ShowToast("Remaining distance is empty.");
            return false;
        }
        if (int.Parse(RemainDis.text) < 0 || int.Parse(RemainDis.text) > 16777215)
        {
            Toast.Instance.ShowToast("Remaining distance input error.");
            return false;
        }
        return true;
    }

    private bool CheckRemainCal()
    {
        if (string.IsNullOrEmpty(RemainCal.text))
        {
            Toast.Instance.ShowToast("Remaining calories is empty.");
            return false;
        }
        if (int.Parse(RemainCal.text) < 0 || int.Parse(RemainCal.text) > 65535)
        {
            Toast.Instance.ShowToast("Remaining calories input error.");
            return false;
        }
        return true;
    }

    private bool CheckHeartRate()
    {
        if (string.IsNullOrEmpty(HeartRate.text))
        {
            Toast.Instance.ShowToast("Heart Rate is empty.");
            return false;
        }
        if (int.Parse(HeartRate.text) < 0 || int.Parse(HeartRate.text) > 255)
        {
            Toast.Instance.ShowToast("Heart Rate input error.");
            return false;
        }
        return true;
    }

    private bool CheckAltitude()//Altitude
    {
        if (string.IsNullOrEmpty(Altitude.text))
        {
            Toast.Instance.ShowToast("Altitude is empty.");
            return false;
        }
        if (!int.TryParse(Altitude.text, out int inumber))
        {
            Toast.Instance.ShowToast("Altitude input error.(Integer number only.)");
            return false;
        }
        
        if (!CmdSwitch.isOn)
        {
            if (int.Parse(Altitude.text) < 0 || int.Parse(Altitude.text) > 65535)
            {
                Toast.Instance.ShowToast("Altitude input error.");
                return false;
            }
        }
        else
        {
            if (int.Parse(Altitude.text) < -32768 || int.Parse(Altitude.text) > 32767)
            {
                Toast.Instance.ShowToast("Altitude input error.");
                return false;
            }
        }

        return true;
    }
    //分時制
    private bool CheckHour()
    {
        if (string.IsNullOrEmpty(Hour.text))
        {
            Toast.Instance.ShowToast("Hour is empty.");
            return false;
        }
        if (TimeFormat.value == 0) //24
        {
            if (int.Parse(Hour.text) < 0 || int.Parse(Hour.text) > 24)
            {
                Toast.Instance.ShowToast("Hour input error.");
                return false;
            }
        }
        else //12
        {
            if (int.Parse(Hour.text) < 1 || int.Parse(Hour.text) > 12)
            {
                Toast.Instance.ShowToast("Hour input error.");
                return false;
            }
        }
        return true;
    }

    private bool CheckMin()
    {
        if (string.IsNullOrEmpty(Minute.text))
        {
            Toast.Instance.ShowToast("Minute is empty.");
            return false;
        }
        if (int.Parse(Minute.text) < 0 || int.Parse(Minute.text) > 59)
        {
            Toast.Instance.ShowToast("Minute input error.");
            return false;
        }
        return true;
    }

    private bool CheckPhoneBattery()
    {
        if (string.IsNullOrEmpty(PhoneBattery.text))
        {
            Toast.Instance.ShowToast("Phone Battery is empty.");
            return false;
        }
        if (int.Parse(PhoneBattery.text) < 0 || int.Parse(PhoneBattery.text) > 59)
        {
            Toast.Instance.ShowToast("Phone Battery input error.");
            return false;
        }
        return true;
    }

    private bool CheckBurnedCal()
    {
        if (string.IsNullOrEmpty(BurnedCal.text))
        {
            Toast.Instance.ShowToast("Burned Calories is empty.");
            return false;
        }
        if (int.Parse(BurnedCal.text) < 0 || int.Parse(BurnedCal.text) > 65535)
        {
            Toast.Instance.ShowToast("Burned Calories input error.");
            return false;
        }
        return true;
    }
    #endregion
}

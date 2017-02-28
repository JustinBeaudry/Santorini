using UnityEngine;

public class GameDevices : MonoBehaviour
{
	public enum Devices
	{
		Computer,
		Mobile
	}

	public enum ControlDevices
	{
		KeyBoardMouse,
		GamePad,
		Touch
	}

	public Devices Device;
	public ControlDevices ControlDevice;

	void Awake ()
	{
		// Set Device by RuntimePlatform
		switch ( Application.platform )
		{
			case RuntimePlatform.OSXEditor:
				Device = Devices.Computer;
				break;
			case RuntimePlatform.OSXPlayer:
				Device = Devices.Computer;
				break;
			case RuntimePlatform.WindowsEditor:
				Device = Devices.Computer;
				break;
			case RuntimePlatform.WindowsPlayer:
				Device = Devices.Computer;
				break;
			case RuntimePlatform.LinuxEditor:
				Device = Devices.Computer;
				break;
			case RuntimePlatform.LinuxPlayer:
				Device = Devices.Computer;
				break;
			case RuntimePlatform.IPhonePlayer:
				Device = Devices.Mobile;
				break;
			case RuntimePlatform.Android:
				Device = Devices.Mobile;
				break;
		}
		// Set ControlDevice from Device and PlayerPrefsManager
		switch ( Device )
		{
			case Devices.Computer:
			// @TODO load from PlayerPrefsManager
				ControlDevice = ControlDevices.KeyBoardMouse;
				break;
			case Devices.Mobile:
			// @TODO load from PlayerPrefsManager
				ControlDevice = ControlDevices.Touch;
				break;
		}
	}
}
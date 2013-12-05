using UnityEngine;

using System.Collections;
using WebSocket4Net;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Linq;
using System;

public class NetJoyClient : MonoBehaviour {
	private WebSocket ws = null;
	private object lockStart = new object();
	private bool guiActive = true;	
	public string host = "192.168.0.212";
	private IPHostEntry hosts = null;
	private bool runConnect = false;
	private WebClient wc = null;
	private string url;
	private Timer wtWebTimer = null;
	private object wtLock = new object();
	private bool downloading = false;
	private string[] flapsRaised;
	private string[] flapsCombat;
	private string[] flapsLanding;	
	private string[] flapsTakeoff;
	private bool isvalid;
	private GameObject netjoy;
	private string lastTime = string.Empty;
	
	void OnApplicationQuit()
	{
		PlayerPrefs.Save();
		ThreadPool.QueueUserWorkItem( a => Stop() );
		Thread.Sleep( 1000 );
		
	}
	void OnGUI() {
		if( guiActive )
		{
			host = GUI.TextField(new Rect( 50, 10, 200,20), host, 25);			
			
			if( GUI.Button ( new Rect(260, 10,50,20), "GO"))
			{				
				PlayerPrefs.SetString("IP", host);
				PlayerPrefs.Save();

				if( !IsValidIp( host ) )
				{
					try
					{
						hosts = Dns.GetHostEntry(host);
					}
					catch{}
				}
				else
				{
					url = string.Format("ws://{0}:54545/",host);
				}
					
				if( hosts == null )
				{
					runConnect = true;				
					ConnectSocket();
					runConnect = false;
					
				}
				else
				{
					foreach( var ip in hosts.AddressList )
					{
						url = string.Format("ws://{0}:54545/",ip);
						host = ip.ToString();
						runConnect = true;				
						ConnectSocket();
						runConnect = false;
					}
				}
				
			}
		}

	}
	public static bool Connected
	{
		get;set;
	}
	public bool IsValidIp(string addr)
    {
        IPAddress ip;
        bool valid = !string.IsNullOrEmpty(addr) && IPAddress.TryParse(addr, out ip);
        return valid;
    }
	public static bool Gear
	{
		get;set;
	}
	public static int X
	{
		get;set;
	}
	public static int Y
	{
		get;set;		
	}
	public static int Z
	{
		get;set;
	}
	public static int RY
	{
		get;set;
	}
	public static int RX
	{
		get;set;
	}
	public static int RZ
	{
		get;set;
	}
	public static int Slider0
	{
		get;set;
	}
	public static int Slider1
	{
		get;set;
	}
	
	
	// Use this for initialization
	void Start () {
		host = PlayerPrefs.GetString( "IP", host);
		flapsCombat = new string[] {"закрылки: бой", "flaps: combat"};
		flapsRaised = new string[] {"закрылки: убраны", "flaps: raised", "боезапас восполнен", "самолёт отремонтирован", "aircraft repaired", "aircraft rearmed"};
		flapsLanding = new string[] {"закрылки: посадка", "flaps: landing"};
		flapsTakeoff = new string[] {"закрылки: взлёт", "flaps: takeoff"};
		netjoy = GameObject.Find("JoyMonitor");
		DataValid = true;
	}
	void Stop() {
		try{
			
			ws = null;
			wc.Dispose();
			wtWebTimer.Change(Timeout.Infinite, Timeout.Infinite );
			wtWebTimer.Dispose();
		}
		catch{}
	}
	void ConnectSocket()
	{
		if( !runConnect )
			return;
		
		lock (lockStart )
		{
			if( wc == null )
			{
				try
				{
					wc = new WebClient();
					wc.Encoding = new System.Text.UTF8Encoding(false);
					wtWebTimer = new Timer( downloadWTNumbers, null, 250, 250 );
				}
				catch
				{
				}
			}
			if( ws == null )
			{
				try
				{
					ws = new WebSocket(url);
					
					ws.Error += new System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(HandleWsError);
					ws.Opened += new System.EventHandler(HandleWsOpened);
					ws.Closed += new System.EventHandler(HandleWsClosed);
					ws.MessageReceived += new System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>(HandleWsMessageReceived);
					Debug.Log("Connecting to " + url);
					ws.Open();
				}
				catch( System.Exception e )
				{
					Debug.Log( e.Message);
				}
			}
			
			
		}
	}
	public static bool DataValid
	{
		get;set;
	}
	public static bool FlapsValid
	{
		get;set;
	}
	public static bool GearValid
	{
		get;set;
	}
	public void downloadWTNumbers( object state )
	{
		if( downloading )
			return;
		
		lock( wtLock )
		{
			downloading = true;
			try
			{
				string indicators = wc.DownloadString( string.Format("http://{0}:8111/indicators", host) );
			
				if( !string.IsNullOrEmpty( indicators ) )
				{
				
					JObject indicatorsJson = JObject.Parse( indicators );
					
					if( indicatorsJson != null )
					{
						DataValid = indicatorsJson["valid"].ToObject<bool>();
						if( !DataValid )
						{
							Flaps = FlapsPos.Raised;
							Gear = true;
							downloading = false;
							return;
						}									
						object gearJObject = indicatorsJson["gears_lamp"];
						if( gearJObject == null )
						{
							GearValid = false;
						}
						else
						{
							var gear = indicatorsJson["gears_lamp"].ToObject<float>();						
							if( gear == 1.0f || gear == 0.0f )
							{
								var newValue =(gear == 1.0f?false:true);
								if( newValue != Gear )
								{
									Gear = newValue;
									Debug.Log ( Gear );
								}
							}				
							GearValid = true;
						}
					}	
				}
				
				string messages = wc.DownloadString( string.Format("http://{0}:8111/hudmsg?lastEvt={1}&lastDmg={2}", host, LastEvent, LastDamage));
				if( !string.IsNullOrEmpty( messages ) )
				{
					JObject messagesJson = JObject.Parse(messages);
					if( messagesJson != null )
					{						
						JArray msgIdPairs = JArray.Parse( messagesJson["events"].ToString() );
						foreach( var pair in msgIdPairs )
						{
							int id = pair["id"].ToObject<int>();
							LastEvent = id;
							string msg = pair["msg"].ToString().ToLower();
							if( flapsCombat.Contains( msg ) )
							{
								Flaps = FlapsPos.Combat;
							}
							else if( flapsLanding.Contains( msg ) )
							{
								Flaps = FlapsPos.Landing;
							}
							else if( flapsRaised.Contains( msg ) )
							{
								Flaps = FlapsPos.Raised;
							}
							else if( flapsTakeoff.Contains( msg ) )
							{
								Flaps = FlapsPos.Takeoff;
							}
							
							Debug.Log( msg + " " + Flaps.ToString() );
								
						}
						
						JArray damagePairs = JArray.Parse( messagesJson["damage"].ToString() );
						foreach( var pair in damagePairs )
						{
							int id = pair["id"].ToObject<int>();
							LastDamage = id;
						}
					}
					
					
				}
			}
			catch{}
			downloading = false;
		}
	}
	public enum FlapsPos
	{
		Raised,
		Combat,
		Takeoff,
		Landing
	}
	public static FlapsPos Flaps
	{
		get;set;
	}
	public static int LastEvent
	{
		get;set;
	}
	public static int LastDamage
	{
		get;set;
	}
	void HandleWsOpened( object sender, System.EventArgs e )
	{
		guiActive = false;
		Debug.Log( "Websocket connected!" );
		Connected = true;

	}
	void HandleWsClosed( object sender, System.EventArgs e )
	{
		Debug.Log( "Closed" );
		Stop ();
		runConnect = true;
		ConnectSocket();
		
	}

	void HandleWsError (object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
	{
		Debug.Log( e.Exception.Message );
		Stop ();
		runConnect = true;
		ConnectSocket();
	}

	void HandleWsMessageReceived (object sender, MessageReceivedEventArgs e)
	{
		JObject json = JObject.Parse( e.Message );
		if( json != null )
		{
			var v = json["v"].ToObject<int>();
			var n = json["n"].ToString();
			
			if( !string.IsNullOrEmpty(n) )
			{
				switch( n )
				{
					case "X":
						X = v;
						break;
					case "Y":
						Y = v;
						break;
					case "Z":
						Z = v;
						break;
					case "RX":
						RX = v;
						break;
					case "RY":
						RY = v;
						break;
					case "RZ":
						RZ = v;
						break;
					case "Slider0":
						Slider0 = v;
						break;
					case "Slider1":
						Slider1 = v;
						break;
					
				}
			}
		}
		
	}
	// Update is called once per frame
	void Update () {
	
	}
}

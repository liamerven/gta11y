		using GTA;
using GTA.Native;
using System;
using System.Drawing;
using System.Windows.Forms;
		using System.Collections.Generic;
using System.Linq;
using DavyKager;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Newtonsoft.Json;

namespace GrandTheftAccessibility
{
	class GTA11Y : Script
	{
		private string currentWeapon;
		private string street;
		private string zone;
		private int health;
		private int wantedLevel;
		private float z;
		private float p;
		private bool timeAnnounced;
		private Dictionary<string, string> hashes = new Dictionary<string, string>();
		private bool[] keyState = new bool[20];
		private Random random = new Random();
		private List<Location> locations = new List<Location>();
		private List<VehicleSpawn> spawns = new List<VehicleSpawn>();
		private long targetTicks;
		private long drivingTicks;
		private bool keys_disabled = false;

		private int locationMenuIndex = 0;
		private int spawnMenuIndex= 0;
		private int mainMenuIndex = 0;
		private List<string> mainMenu = new List<string>();
		private int funMenuIndex = 0;
		private List<string> funMenu = new List<string>();
		private int driveMenuIndex = 0;
		private List<string> driveMenu = new List<string>();
		private int settingsMenuIndex = 0;
		private List <Setting> settingsMenu = new List<Setting>();

		private WaveOutEvent out1;
		private WaveOutEvent out2;
		private WaveOutEvent out3;
		private WaveOutEvent out11;
		private WaveOutEvent out12;

		private AudioFileReader tped;
		private AudioFileReader tvehicle;
		private AudioFileReader tprop;
		private SignalGenerator alt;
		private SignalGenerator pitch;

		private bool[] headings = new bool[8];
		private bool climbing = false;
		private bool shifting = false;


		private const double north = 0;
		private const double northnortheast = 22.5;
		private const double northeast = 45;
		private const double eastnortheast = 67.5;
		private const double east = 90;
		private const double eastsoutheast = 112.5;
		private const double southeast = 135;
		private const double southsoutheast = 157.5;
		private const double south = 180;
		private const double southsouthwest = 202.5;
		private const double southwest = 225;
		private const double westsouthwest = 247.5;
		private const double west = 270;
		private const double westnorthwest = 292.5;
		private const double northwest = 315;
		private const double northnorthwest = 337.5;

		public GTA11Y()
		{

			this.Tick += onTick;
			this.KeyUp += onKeyUp;
			this.KeyDown += onKeyDown;
			Tolk.Load();
			Tolk.Speak("Mod Ready");

			currentWeapon = Game.Player.Character.Weapons.Current.Hash.ToString();
			string[] lines = System.IO.File.ReadAllLines("scripts/hashes.txt");
			string[] result;
			foreach (string line in lines)
			{
				result = line.Split('=');
				if (!hashes.ContainsKey(result[1]))
					hashes.Add(result[1], result[0]);
			}

			locations.Add( new Location("MICHAEL'S HOUSE", new GTA.Math.Vector3(-852.4f, 160.0f, 65.6f)));
locations.Add(new Location("FRANKLIN'S HOUSE", new GTA.Math.Vector3(7.9f, 548.1f, 175.5f)));
			locations.Add(new Location("TREVOR'S TRAILER", new GTA.Math.Vector3(1985.7f, 3812.2f, 32.2f)));
			locations.Add(new Location("AIRPORT ENTRANCE", new GTA.Math.Vector3(-1034.6f, -2733.6f, 13.8f)));
			locations.Add(new Location("AIRPORT FIELD", new GTA.Math.Vector3(-1336.0f, -3044.0f, 13.9f)));
			locations.Add(new Location("ELYSIAN ISLAND", new GTA.Math.Vector3(338.2f, -2715.9f, 38.5f)));
			locations.Add(new Location("JETSAM", new GTA.Math.Vector3(760.4f, -2943.2f, 5.8f)));
			locations.Add(new Location("StripClub", new GTA.Math.Vector3(96.17191f, -1290.668f, 29.26874f)));
			locations.Add(new Location("ELBURRO HEIGHTS", new GTA.Math.Vector3(1384.0f, -2057.1f, 52.0f)));
			locations.Add(new Location("FERRIS WHEEL", new GTA.Math.Vector3(-1670.7f, -1125.0f, 13.0f)));
			locations.Add(new Location("CHUMASH", new GTA.Math.Vector3(-3192.6f, 1100.0f, 20.2f)));
			locations.Add(new Location("Altruist Cult Camp", new GTA.Math.Vector3(-1170.841f, 4926.646f, 224.295f)));
			locations.Add(new Location("Hippy Camp", new GTA.Math.Vector3(2476.712f, 3789.645f, 41.226f)));
 locations.Add(new Location("Far North San Andreas", new GTA.Math.Vector3(24.775f, 7644.102f, 19.055f)));
locations.Add(new Location("Fort Zancudo", new GTA.Math.Vector3(-2047.4f, 3132.1f, 32.8f)));
locations.Add(new Location("Fort Zancudo ATC Entrance", new GTA.Math.Vector3(-2344.373f, 3267.498f, 32.811f)));
locations.Add(new Location("Playboy Mansion", new GTA.Math.Vector3(-1475.234f, 167.088f, 55.841f)));
						locations.Add(new Location("WINDFARM", new GTA.Math.Vector3(2354.0f, 1830.3f, 101.1f)));
			locations.Add(new Location("MCKENZIE AIRFIELD", new GTA.Math.Vector3(2121.7f, 4796.3f, 41.1f)));
			locations.Add(new Location("DESERT AIRFIELD", new GTA.Math.Vector3(1747.0f, 3273.7f, 41.1f)));
			locations.Add(new Location("CHILLIAD", new GTA.Math.Vector3(425.4f, 5614.3f, 766.5f)));
			locations.Add(new Location("Police Station", new GTA.Math.Vector3(436.491f, -982.172f, 30.699f)));
			locations.Add(new Location("Casino", new GTA.Math.Vector3(925.329f, 46.152f, 80.908f)));
locations.Add(new Location("Vinewood sign", new GTA.Math.Vector3(711.362f, 1198.134f, 348.526f)));
			locations.Add(new Location("Blaine County Savings Bank", new GTA.Math.Vector3(-109.299f, 6464.035f, 31.627f)));
			locations.Add(new Location("LS Government Facility", new GTA.Math.Vector3(2522.98f, -384.436f, 92.9928f)));
			locations.Add(new Location("CHILIAD MOUNTAIN STATE WILDERNESS", new GTA.Math.Vector3(2994.917f, 2774.16f, 42.33663f)));
				locations.Add(new Location("Beaker's Garage", new GTA.Math.Vector3(116.3748f, 6621.362f, 31.6078f)));

foreach (VehicleHash v in Enum.GetValues(typeof(VehicleHash)))
			{
				string i = Game.GetLocalizedString(Function.Call<string>(Hash.GET_DISPLAY_NAME_FROM_VEHICLE_MODEL, v));
				spawns.Add(new VehicleSpawn(i, v));
			}
			spawns.Sort();


			mainMenu.Add("Teleport to location. ");
			mainMenu.Add("Spawn Vehicle. ");
			mainMenu.Add("Functions. ");
			mainMenu.Add("Settings. ");

			funMenu.Add("Blow up all nearby vehicles");
			funMenu.Add("Make all nearby pedestrians attack each other.");
			funMenu.Add("instantly kill all nearby pedestrians.");
			funMenu.Add("Raise Wanted Level. ");
			funMenu.Add("Clear Wanted Level. ");

			driveMenu.Add("Cancel Autodrive. ");
			driveMenu.Add("Drive Slow. ");
			driveMenu.Add("Drive normal. "); 
			driveMenu.Add("Drive Fast. ");
			driveMenu.Add("Drive Normally. ");
			driveMenu.Add("Drive Rushed. ");
			driveMenu.Add("Drive Wrecklessly. ");


			tped = new AudioFileReader(@"scripts/tped.wav");
			tvehicle = new AudioFileReader(@"scripts/tvehicle.wav");
			tprop = new AudioFileReader(@"scripts/tprop.wav");
			out1 = new WaveOutEvent();
			out2 = new WaveOutEvent();
			out3 = new WaveOutEvent();
			out11 = new WaveOutEvent();
			out12 = new WaveOutEvent();
			out1.Init(tped);
						out2.Init(tvehicle);
			out3.Init(tprop);
			alt = new SignalGenerator();
			out11.Init(alt);
			pitch = new SignalGenerator();
			out12.Init(pitch);

			setupSettings();
		}

		private void onTick(object sender, EventArgs e)
		{
			if (!Game.IsLoading)
			{
				if (Game.Player.Character.HeightAboveGround - z > 1f || Game.Player.Character.HeightAboveGround - z < -1f)
				{
					z = Game.Player.Character.HeightAboveGround;
					if (getSetting("altitudeIndicator") == 1)
					{
						out11.Stop();
						alt.Gain = 0.1;
						alt.Frequency = 120 + (z * 40);
						alt.Type = SignalGeneratorType.Triangle;
						out11.Init(alt.Take(TimeSpan.FromSeconds(0.075)));
						out11.Play();
					}
				}

if (GTA.GameplayCamera.RelativePitch - p > 1f || GTA.GameplayCamera.RelativePitch -p < -1f)
					{
						p = GTA.GameplayCamera.RelativePitch;
						if (getSetting("targetPitchIndicator") == 1)
						{
						if (GTA.GameplayCamera.IsAimCamActive)
						{
							out12.Stop();
							pitch.Gain = 0.08;
							pitch.Frequency = 600 + (p * 6);
							pitch.Type = SignalGeneratorType.Square;
							out12.Init(pitch.Take(TimeSpan.FromSeconds(0.025)));
							out12.Play();
						}
					}

				}

				if (wantedLevel != Game.Player.WantedLevel)
				{
					wantedLevel = Game.Player.WantedLevel;
					if (getSetting("neverWanted") == 1)
					{

					}
					else
					{
						Tolk.Speak("Wanted level is now " + wantedLevel);
					}

									}

				if (getSetting("radioOff") == 1)
				{
					if (Game.Player.Character.CurrentVehicle != null)
					{
						Game.Player.Character.CurrentVehicle.IsRadioEnabled = false;
					}
				}
				else
				{
					if (Game.Player.Character.CurrentVehicle != null)
					{
						Game.Player.Character.CurrentVehicle.IsRadioEnabled = true;
					}
				}

				//cheats

				if (getSetting("godMode") == 1)
				{
					Game.Player.IsInvincible = true;
					Game.Player.Character.CanBeDraggedOutOfVehicle = false;
					Game.Player.Character.CanBeKnockedOffBike = false;
					Game.Player.Character.CanBeShotInVehicle = false;
					Game.Player.Character.CanFlyThroughWindscreen = false;
					Game.Player.Character.DrownsInSinkingVehicle = false;

				}
				else
				{
					Game.Player.IsInvincible = false;
					Game.Player.Character.CanBeDraggedOutOfVehicle = true;
					Game.Player.Character.CanBeKnockedOffBike = true;
					Game.Player.Character.CanBeShotInVehicle = true;
					Game.Player.Character.CanFlyThroughWindscreen = true;
					Game.Player.Character.DrownsInSinkingVehicle = true;

				}

				if (getSetting("vehicleGodMode") == 1)
				{
					if (Game.Player.Character.CurrentVehicle != null && Game.Player.Character.IsInVehicle())
					{
						Vehicle vehicle = Game.Player.Character.CurrentVehicle;
						vehicle.IsInvincible = true;
						vehicle.CanWheelsBreak = false;
						vehicle.CanTiresBurst = false;
						vehicle.CanBeVisiblyDamaged = false;
						vehicle.IsBulletProof = true;
						vehicle.IsCollisionProof = true;
						vehicle.IsExplosionProof = true;
						vehicle.IsMeleeProof = true;
						vehicle.IsFireProof = true;
					}
if (Game.Player.Character.LastVehicle != null && !Game.Player.Character.IsInVehicle())
					{
Vehicle vehicle = Game.Player.Character.LastVehicle;
						vehicle.CanWheelsBreak = true;
						vehicle.CanTiresBurst = true;
						vehicle.CanBeVisiblyDamaged = true;
						vehicle.IsBulletProof = false;
						vehicle.IsCollisionProof = false;
						vehicle.IsExplosionProof = false;
						vehicle.IsMeleeProof = false;
						vehicle.IsFireProof = false;
						vehicle.IsInvincible = false;
					}
				}
				else
				{
					if (Game.Player.Character.CurrentVehicle != null)
					{
						Vehicle vehicle = Game.Player.Character.CurrentVehicle;
						vehicle.IsInvincible = false;
						vehicle.CanWheelsBreak = true;
						vehicle.CanTiresBurst = true;
						vehicle.CanBeVisiblyDamaged = true;
						vehicle.IsBulletProof = false;
						vehicle.IsCollisionProof = false;
						vehicle.IsExplosionProof = false;
						vehicle.IsMeleeProof = false;
						vehicle.IsFireProof = false;
					}
				}

				if (getSetting("policeIgnore") == 1)
				{
					Game.Player.IgnoredByPolice = true;
				}
				else
				{
					Game.Player.IgnoredByPolice = false;
				}

				if (getSetting("neverWanted") == 1)
				{
					Game.Player.WantedLevel = 0;
				}

				if (getSetting("infiniteAmmo") == 1)
				{
					Game.Player.Character.Weapons.Current.InfiniteAmmoClip = true;
					Game.Player.Character.Weapons.Current.InfiniteAmmo = true;
				}
				else
				{
					Game.Player.Character.Weapons.Current.InfiniteAmmo = false;
					Game.Player.Character.Weapons.Current.InfiniteAmmo = true;					Game.Player.Character.Weapons.Current.InfiniteAmmoClip = false;
				}

				if (getSetting("exsplosiveAmmo") == 1)
					Game.Player.SetExplosiveAmmoThisFrame();
				if (getSetting("fireAmmo") == 1)
					Game.Player.SetFireAmmoThisFrame();
				if (getSetting("explosiveMelee") == 1)
					Game.Player.SetExplosiveMeleeThisFrame();
				if (getSetting("superJump") == 1)
					Game.Player.SetSuperJumpThisFrame();
				if (getSetting("runFaster") == 1)
					Game.Player.SetRunSpeedMultThisFrame(2f);
				if (getSetting("swimFaster") == 1)
					Game.Player.SetSwimSpeedMultThisFrame(2f);

				if (Game.Player.Character.IsFalling || Game.Player.Character.IsGettingIntoVehicle || Game.Player.Character.IsGettingUp || Game.Player.Character.IsProne || Game.Player.Character.IsRagdoll)
				{
				}
				else 
				{
				double heading = Game.Player.Character.Heading;
				if (headings[headingSlice(heading)] == false)
				{
					headings[headingSlice(heading)] = true;
					for (int i = 0; i < headings.Length; i++)
					{
						if (i != headingSlice(heading))
							headings[i] = false;

					}
if (getSetting("announceHeadings") == 1)
					Tolk.Speak(headingSliceName(heading), true);
				}
				}

				TimeSpan t = World.CurrentTimeOfDay;
				if (t.Minutes == 0)
				{
					if ((t.Hours == 3 || t.Hours == 6 || t.Hours == 9 || t.Hours == 12 || t.Hours == 15 || t.Hours == 18 || t.Hours == 21) && timeAnnounced == false)
					{
						timeAnnounced = true;
						if (getSetting("announceTime") == 1)
						Tolk.Speak("The time is now: " + t.Hours + ":00");
					}
				}
				else
				{
					timeAnnounced = false;
				}

				if (street != World.GetStreetName(Game.Player.Character.Position))
				{
					street = World.GetStreetName(Game.Player.Character.Position);
if (getSetting("announceZones") == 1)
											Tolk.Speak(street);
				}

				if (zone != World.GetZoneLocalizedName(Game.Player.Character.Position))
				{
					zone = World.GetZoneLocalizedName(Game.Player.Character.Position);
					if (getSetting("announceZones") == 1)
						Tolk.Speak(zone);
				}

				if (Game.Player.Character.Weapons.Current.Hash.ToString() != currentWeapon)
				{
					currentWeapon = Game.Player.Character.Weapons.Current.Hash.ToString();
					Tolk.Speak(currentWeapon);
				}

				if (DateTime.Now.Ticks - drivingTicks > 25000000 && Game.Player.Character.CurrentVehicle != null && Game.Player.Character.CurrentVehicle.Speed > 1)
				{
					drivingTicks = DateTime.Now.Ticks;
					Tolk.Speak("" + Math.Round(Game.Player.Character.CurrentVehicle.Speed) + " miles per hour");
									}


					if (DateTime.Now.Ticks - targetTicks > 2000000 && Game.Player.TargetedEntity != null && Game.Player.Character.Weapons.Current.Hash != WeaponHash.HomingLauncher)
				{
					targetTicks = DateTime.Now.Ticks;
					if (Game.Player.TargetedEntity.EntityType == EntityType.Ped && !Game.Player.TargetedEntity.IsDead)
					{
						out1.Stop();
						tped.Position = 0;
						out1.Play();
					}

					if (Game.Player.TargetedEntity.EntityType == EntityType.Ped && !Game.Player.TargetedEntity.IsDead)
					{
						out1.Stop();
						tped.Position = 0;
						out1.Play();
					}

					if (Game.Player.TargetedEntity.EntityType == EntityType.Vehicle && !Game.Player.TargetedEntity.IsDead)
					{
						out2.Stop();
						tvehicle.Position = 0;
						out2.Play();
					}

					if (Game.Player.TargetedEntity.EntityType == EntityType.Prop && (!Game.Player.TargetedEntity.IsExplosionProof || !Game.Player.TargetedEntity.IsBulletProof))
					{
						out3.Stop();
						tprop.Position = 0;
						out3.Play();
					}

				}
			}
		}



		private void onKeyDown(object sender, KeyEventArgs e)
		{

			if (e.Control)
			{
				shifting = true;
			}

			/*
			if (e.KeyCode == Keys.NumPad7 && !shifting && !keyState[7])
			{
				keyState[7] = true;
				Vehicle[] vehicles = World.GetNearbyVehicles(Game.Player.Character.Position, 500);
				foreach (Vehicle vehicle in vehicles)
				{
					if (vehicle.IsDead == false)
					{
						vehicle.Explode();

					}
				}


			}
			*/

			if (e.KeyCode == Keys.NumPad2 && shifting && !keyState[2])
			{
				keyState[2] = true;

				if (!keys_disabled)
				{
					keys_disabled = true;
					Tolk.Speak("Accessibility keys deactivated.");
				}
				else if (keys_disabled)
				{
					keys_disabled = false;
					Tolk.Speak("Accessibility keys activated.");
				}
			}


			if (!keys_disabled)
			{
				if (e.KeyCode == Keys.NumPad4 && !keyState[4])
				{
					keyState[4] = true;
					Vehicle[] vehicles = World.GetNearbyVehicles(Game.Player.Character.Position, 50);
					string status;
					List<Result> results = new List<Result>();
					bool valid = false;
					foreach (Vehicle vehicle in vehicles)
					{
						valid = false;

						if (getSetting("onscreen") == 0 && vehicle.IsVisible && !vehicle.IsDead)
							valid = true;

						if (getSetting("onscreen") == 1 && vehicle.IsVisible && !vehicle.IsDead && vehicle.IsOnScreen)
							valid = true;

						if (valid)
						{
							if (vehicle.IsStopped)
							{
								status = "a stationary";
							}
							else
							{
								status = "a moving";
							}
							if (Game.Player.Character.CurrentVehicle != vehicle)
							{
								string name = (status + " " + vehicle.LocalizedName);
								double xyDistance = Math.Round(World.GetDistance(Game.Player.Character.Position, vehicle.Position) - Math.Abs(Game.Player.Character.Position.Z - vehicle.Position.Z), 1);
								double zDistance = Math.Round(vehicle.Position.Z - Game.Player.Character.Position.Z, 1);
								string direction = getDir(calculate_x_y_angle(Game.Player.Character.Position.X, Game.Player.Character.Position.Y, vehicle.Position.X, vehicle.Position.Y, 0));
								Result result = new Result(name, xyDistance, zDistance, direction);
								results.Add(result);
							}

						}
					}

					Tolk.Speak(listToString(results, "Nearest Vehicles: "));

				}


				if (e.KeyCode == Keys.Decimal && !keyState[10])
				{
					keyState[10] = true;
					Tolk.Speak("facing " + getDir(Game.Player.Character.Heading));

				}

				if (e.KeyCode == Keys.NumPad6)
				{
					Ped[] peds = World.GetNearbyPeds(Game.Player.Character.Position, 50);
					string status = "";
					List<Result> results = new List<Result>();
					bool valid = false;

					foreach (Ped ped in peds)
					{
						valid = false;
						if (getSetting("onscreen") == 0 && hashes.ContainsKey(ped.Model.NativeValue.ToString()) && ped.IsVisible && !ped.IsDead)
							valid = true;

						if (getSetting("onscreen") == 1 && hashes.ContainsKey(ped.Model.NativeValue.ToString()) && ped.IsVisible && ped.IsOnScreen && !ped.IsDead)
							valid = true;
						if (valid)
						{
							if (hashes[ped.Model.NativeValue.ToString()] != "player_one" && hashes[ped.Model.NativeValue.ToString()] != "player_two" && hashes[ped.Model.NativeValue.ToString()] != "player_zero")
							{
								string name = (status + " " + hashes[ped.Model.NativeValue.ToString()]);
								double xyDistance = Math.Round(World.GetDistance(Game.Player.Character.Position, ped.Position) - Math.Abs(Game.Player.Character.Position.Z - ped.Position.Z), 1);
								double zDistance = Math.Round(ped.Position.Z - Game.Player.Character.Position.Z, 1);
								string direction = getDir(calculate_x_y_angle(Game.Player.Character.Position.X, Game.Player.Character.Position.Y, ped.Position.X, ped.Position.Y, 0));
								Result result = new Result(name, xyDistance, zDistance, direction);
								results.Add(result);
							}
						}
					}

					Tolk.Speak(listToString(results, "Nearest Characters: "));

				}

				if (e.KeyCode == Keys.NumPad5 && !keyState[5])
				{
					keyState[5] = true;
					Prop[] props = World.GetNearbyProps(Game.Player.Character.Position, 50);
					string status = "";
					List<Result> results = new List<Result>();

					bool valid = false;

					foreach (Prop prop in props)
					{
						valid = false;

						if (getSetting("onscreen") == 0 && hashes.ContainsKey(prop.Model.NativeValue.ToString()) && prop.IsVisible && !prop.IsAttachedTo(Game.Player.Character) && (hashes[prop.Model.NativeValue.ToString()].Contains("door") || hashes[prop.Model.NativeValue.ToString()].Contains("gate")))
							valid = true;

						if (getSetting("onscreen") == 1 && hashes.ContainsKey(prop.Model.NativeValue.ToString()) && prop.IsVisible && prop.IsOnScreen && !prop.IsAttachedTo(Game.Player.Character) && (hashes[prop.Model.NativeValue.ToString()].Contains("door") || hashes[prop.Model.NativeValue.ToString()].Contains("gate")))
							valid = true;

						if (valid)
						{
							string name = (status + " " + hashes[prop.Model.NativeValue.ToString()]);
							double xyDistance = Math.Round(World.GetDistance(Game.Player.Character.Position, prop.Position) - Math.Abs(Game.Player.Character.Position.Z - prop.Position.Z), 1);
							double zDistance = Math.Round(prop.Position.Z - Game.Player.Character.Position.Z, 1);
							string direction = getDir(calculate_x_y_angle(Game.Player.Character.Position.X, Game.Player.Character.Position.Y, prop.Position.X, prop.Position.Y, 0));
							Result result = new Result(name, xyDistance, zDistance, direction);
							results.Add(result);

						}
					}

					Tolk.Speak(listToString(results, "Nearest Doors: "));
				}

				if (e.KeyCode == Keys.NumPad8 && !keyState[8])
				{
					keyState[8] = true;
					Prop[] props = World.GetNearbyProps(Game.Player.Character.Position, 50);
					string status = "";
					List<Result> results = new List<Result>();
					bool valid = false;

					foreach (Prop prop in props)
					{
						valid = false;

						if (getSetting("onscreen") == 0 && hashes.ContainsKey(prop.Model.NativeValue.ToString()) && prop.IsVisible && !prop.IsAttachedTo(Game.Player.Character) && (hashes[prop.Model.NativeValue.ToString()].Contains("door") == false || !hashes[prop.Model.NativeValue.ToString()].Contains("gate") == false))
							valid = true;

						if (getSetting("onscreen") == 1 && hashes.ContainsKey(prop.Model.NativeValue.ToString()) && prop.IsVisible && prop.IsOnScreen && !prop.IsAttachedTo(Game.Player.Character) && (hashes[prop.Model.NativeValue.ToString()].Contains("door") == false || !hashes[prop.Model.NativeValue.ToString()].Contains("gate") == false))
							valid = true;

						if (valid)
						{
							string name = (status + " " + hashes[prop.Model.NativeValue.ToString()]);
							double xyDistance = Math.Round(World.GetDistance(Game.Player.Character.Position, prop.Position) - Math.Abs(Game.Player.Character.Position.Z - prop.Position.Z), 1);
							double zDistance = Math.Round(prop.Position.Z - Game.Player.Character.Position.Z, 1);
							string direction = getDir(calculate_x_y_angle(Game.Player.Character.Position.X, Game.Player.Character.Position.Y, prop.Position.X, prop.Position.Y, 0));
							Result result = new Result(name, xyDistance, zDistance, direction);
							results.Add(result);

						}
					}

					Tolk.Speak(listToString(results, "Nearest Objects: "));

				}

				if (e.KeyCode == Keys.NumPad0 && !keyState[0])
				{
					keyState[0] = true;
					if (e.Control)
					{
						TimeSpan t = World.CurrentTimeOfDay;
						string zero = "";
						if (t.Minutes > 0 && t.Minutes < 10)
							zero = "0";
						Tolk.Speak("the time is: " + t.Hours + ":" + zero + t.Minutes);
					}
					else
					{
						if (Game.Player.Character.CurrentVehicle == null)
						{
							Tolk.Speak("Current location: " + World.GetStreetName(Game.Player.Character.Position) + ", " + World.GetZoneLocalizedName(Game.Player.Character.Position) + ".");
						}
						else
						{
							Tolk.Speak("Current location: " + "Inside of a " + Game.Player.Character.CurrentVehicle.DisplayName + " at " + World.GetStreetName(Game.Player.Character.Position) + ", " + World.GetZoneLocalizedName(Game.Player.Character.Position) + ".");
						}


					}

				}

				if (e.KeyCode == Keys.NumPad2 && !shifting && !keyState[2])
				{
					GTA.Audio.PlaySoundFrontend("SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
					keyState[2] = true;

					if (mainMenuIndex == 0)
					{

						if (Game.Player.Character.CurrentVehicle != null)
						{
							Game.Player.Character.CurrentVehicle.Position = locations[locationMenuIndex].coords;

						}
						else
						{

							Game.Player.Character.Position = locations[locationMenuIndex].coords;
						}
					}

					if (mainMenuIndex == 1)
					{
						Vehicle vehicle = World.CreateVehicle(spawns[spawnMenuIndex].id, Game.Player.Character.Position + Game.Player.Character.ForwardVector * 2.0f, Game.Player.Character.Heading + 90);
						vehicle.PlaceOnGround();
						if (getSetting("warpInsideVehicle") == 1)
						{
							Game.Player.Character.SetIntoVehicle(vehicle, VehicleSeat.Driver);
						}

					}


					if (mainMenuIndex == 2)
					{
						if (funMenuIndex == 0)
						{
							Vehicle[] vehicles = World.GetNearbyVehicles(Game.Player.Character.Position, 100);
							foreach (Vehicle v in vehicles)
							{
								if (!v.IsDead)
								{
									if (getSetting("vehicleGodMode") == 0 && Game.Player.Character.CurrentVehicle == v)
									{
										v.CanBeVisiblyDamaged = true;
										v.CanEngineDegrade = true;
										v.CanTiresBurst = true;
										v.CanWheelsBreak = true;
										v.IsExplosionProof = false;
										v.IsFireProof = false;
										v.IsInvincible = false;
										v.IsBulletProof = false;
										v.IsMeleeProof = false;
									}
									v.Explode();
									v.MarkAsNoLongerNeeded();
								}
							}
						}

						if (funMenuIndex == 1)
						{
							Ped[] tempPeds = World.GetNearbyPeds(Game.Player.Character.Position, 5000);

							List<Ped> peds = new List<Ped>();

							foreach (Ped ped in tempPeds)
							{

								if (hashes.ContainsKey(ped.Model.NativeValue.ToString()) && !ped.IsDead)
								{
									if (hashes[ped.Model.NativeValue.ToString()] != "player_one" && hashes[ped.Model.NativeValue.ToString()] != "player_two" && hashes[ped.Model.NativeValue.ToString()] != "player_zero")
									{
										peds.Add(ped);
									}
								}
							}
							if (peds.Count < 4)
							{
								Tolk.Speak("More nearby people are needed.");
							}
							else
							{

								int r = 0;
								for (int i = 0; i < peds.Count; i++)
								{
									r = random.Next(0, peds.Count - 1);
									while (r == i)
									{
										r = random.Next(0, peds.Count - 1);
									}
									peds[i].Task.ClearAllImmediately();
									peds[i].AlwaysKeepTask = false;
									peds[i].BlockPermanentEvents = false;
									peds[i].Weapons.Give(WeaponHash.APPistol, 1000, true, true);
									peds[i].Task.FightAgainst(peds[r]);
									peds[i].AlwaysKeepTask = true;
									peds[i].BlockPermanentEvents = true;

								}
							}
						}

						if (funMenuIndex == 2)
						{
							Ped[] tempPeds = World.GetNearbyPeds(Game.Player.Character.Position, 5000);

							List<Ped> peds = new List<Ped>();
							foreach (Ped ped in tempPeds)
							{
								if (hashes.ContainsKey(ped.Model.NativeValue.ToString()) && !ped.IsDead)
								{
									if (hashes[ped.Model.NativeValue.ToString()] != "player_one" && hashes[ped.Model.NativeValue.ToString()] != "player_two" && hashes[ped.Model.NativeValue.ToString()] != "player_zero")
									{
										peds.Add(ped);
									}
								}
							}

							foreach (Ped ped in peds)
							{
								ped.Kill();
							}

						}

						if (funMenuIndex == 3)
						{
							if (Game.Player.WantedLevel < 5)
								Game.Player.WantedLevel++;
						}

						if (funMenuIndex == 4)
						{
							Game.Player.WantedLevel = 0;

						}


					}


					if (mainMenuIndex == 3)
					{

						if (settingsMenu[settingsMenuIndex].value == 0)
						{
							settingsMenu[settingsMenuIndex].value = 1;
							Tolk.Speak(settingsMenu[settingsMenuIndex].displayName + "On! ");
						}

						else if (settingsMenu[settingsMenuIndex].value == 1)
						{
							settingsMenu[settingsMenuIndex].value = 0;
							Tolk.Speak(settingsMenu[settingsMenuIndex].displayName + "Off! ");
						}

						saveSettings();
					}
				}

				if (e.KeyCode == Keys.NumPad1 && !keyState[1])
				{
					GTA.Audio.PlaySoundFrontend("NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET");

					keyState[1] = true;
					if (mainMenuIndex == 0)
					{
						if (locationMenuIndex > 00)
						{
							locationMenuIndex--;
							Tolk.Speak(locations[locationMenuIndex].name);
						}

						else
						{
							locationMenuIndex = locations.Count - 1;
							Tolk.Speak(locations[locationMenuIndex].name);
						}
					}

					if (mainMenuIndex == 1)
					{
						if (!shifting)
						{
							if (spawnMenuIndex > 0)
							{
								spawnMenuIndex--;
								Tolk.Speak(spawns[spawnMenuIndex].name);
							}

							else
							{
								spawnMenuIndex = spawns.Count - 1;
								Tolk.Speak(spawns[spawnMenuIndex].name);
							}

						}

						if (shifting)
						{
							if (spawnMenuIndex > 25)
							{
								spawnMenuIndex = spawnMenuIndex - 25;
								Tolk.Speak(spawns[spawnMenuIndex].name);
							}

							else
							{
								int rem = spawnMenuIndex;
								spawnMenuIndex = spawns.Count - 1 - rem;
								Tolk.Speak(spawns[spawnMenuIndex].name);
							}
						}

					}

					if (mainMenuIndex == 2)
					{
						if (funMenuIndex > 0)
						{
							funMenuIndex--;
							Tolk.Speak(funMenu[funMenuIndex]);
						}

						else
						{
							funMenuIndex = funMenu.Count - 1;
							Tolk.Speak(funMenu[funMenuIndex]);
						}
					}

					if (mainMenuIndex == 3)
					{
						if (settingsMenuIndex > 0)
						{
							settingsMenuIndex--;
							string toggle = "";
							if (settingsMenu[settingsMenuIndex].value == 0)
								toggle = "Off";
							if (settingsMenu[settingsMenuIndex].value == 1)
								toggle = "On";
							Tolk.Speak(settingsMenu[settingsMenuIndex].displayName + toggle);

						}

						else
						{
							settingsMenuIndex = settingsMenu.Count - 1;
							string toggle = "";
							if (settingsMenu[settingsMenuIndex].value == 0)
								toggle = "Off";
							if (settingsMenu[settingsMenuIndex].value == 1)
								toggle = "On";
							Tolk.Speak(settingsMenu[settingsMenuIndex].displayName + toggle);

						}
					}

				}

				if (e.KeyCode == Keys.NumPad3 & !keyState[3])
				{
					GTA.Audio.PlaySoundFrontend("NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
					keyState[3] = true;
					if (mainMenuIndex == 0)
					{
						if (locationMenuIndex < locations.Count - 1)
						{
							locationMenuIndex++;
							Tolk.Speak(locations[locationMenuIndex].name);

						}
						else
						{
							locationMenuIndex = 0;
							Tolk.Speak(locations[locationMenuIndex].name);

						}
					}

					if (mainMenuIndex == 1)
					{
						if (!shifting)
						{
							if (spawnMenuIndex < spawns.Count - 1)
							{
								spawnMenuIndex++;
								Tolk.Speak(spawns[spawnMenuIndex].name);

							}
							else
							{
								spawnMenuIndex = 0;
								Tolk.Speak(spawns[spawnMenuIndex].name);

							}
						}

						if (shifting)
						{
							if (spawnMenuIndex < spawns.Count - 26)
							{
								spawnMenuIndex = spawnMenuIndex + 25;
								Tolk.Speak(spawns[spawnMenuIndex].name);

							}
							else
							{
								int rem = spawns.Count - 1 - spawnMenuIndex;
								spawnMenuIndex = rem;
								Tolk.Speak(spawns[spawnMenuIndex].name);

							}
						}

					}

					if (mainMenuIndex == 2)
					{
						if (funMenuIndex < funMenu.Count - 1)
						{
							funMenuIndex++;
							Tolk.Speak(funMenu[funMenuIndex]);

						}
						else
						{
							funMenuIndex = 0;
							Tolk.Speak(funMenu[funMenuIndex]);

						}
					}

					if (mainMenuIndex == 3)
					{
						if (settingsMenuIndex < settingsMenu.Count - 1)
						{
							settingsMenuIndex++;
							string toggle = "";
							if (settingsMenu[settingsMenuIndex].value == 0)
								toggle = "Off";
							if (settingsMenu[settingsMenuIndex].value == 1)
								toggle = "On";
							Tolk.Speak(settingsMenu[settingsMenuIndex].displayName + toggle);

						}
						else
						{
							settingsMenuIndex = 0;
							string toggle = "";
							if (settingsMenu[settingsMenuIndex].value == 0)
								toggle = "Off";
							if (settingsMenu[settingsMenuIndex].value == 1)
								toggle = "On";
							Tolk.Speak(settingsMenu[settingsMenuIndex].displayName + toggle);

						}
					}


				}

				if (e.KeyCode == Keys.NumPad7 && !keyState[7])
				{
					GTA.Audio.PlaySoundFrontend("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
					keyState[7] = true;
					if (mainMenuIndex > 0)
					{
						mainMenuIndex--;
						speakMenu();
					}

					else
					{
						mainMenuIndex = mainMenu.Count - 1;
						speakMenu();
					}
				}

				if (e.KeyCode == Keys.NumPad9 && !keyState[9])
				{
					GTA.Audio.PlaySoundFrontend("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
					keyState[9] = true;
					if (mainMenuIndex < mainMenu.Count - 1)
					{
						mainMenuIndex++;
						speakMenu();

					}
					else
					{
						mainMenuIndex = 0;
						speakMenu();

					}
				}

			}
		}

		private void onKeyUp(object sender, KeyEventArgs e)
		{
			if (!e.Control)
			{
				shifting = false;
			}
			if (e.KeyCode == Keys.NumPad0 && keyState[0])
				keyState[0] = false;
			if (e.KeyCode == Keys.NumPad1 && keyState[1])
				keyState[1] = false;
			if (e.KeyCode == Keys.NumPad2 && keyState[2])
				keyState[2] = false;
			if (e.KeyCode == Keys.NumPad3 && keyState[3])
				keyState[3] = false;
			if (e.KeyCode == Keys.NumPad4 && keyState[4])
				keyState[4] = false;
			if (e.KeyCode == Keys.NumPad5 && keyState[5])
				keyState[5] = false;
			if (e.KeyCode == Keys.NumPad6 && keyState[6])
				keyState[6] = false;
			if (e.KeyCode == Keys.NumPad7 && keyState[7])
				keyState[7] = false;
			if (e.KeyCode == Keys.NumPad8 && keyState[8])
				keyState[8] = false;
			if (e.KeyCode == Keys.NumPad9 && keyState[9])
				keyState[9] = false;
			if (e.KeyCode == Keys.Decimal && keyState[10])
				keyState[10] = false;


		}


		private double calculate_x_y_angle(double x1, double y1, double x2, double y2, double deg)
		{
			double x = x1 - x2;
			double y = y2 - y1;
			double rad = 0;
			if (x == 0 || y == 0)
			{
				rad = Math.Atan(0);
			}
			else
			{
				rad = Math.Atan(y / x);
			}
			double arctan = rad / Math.PI * 180;
			double fdeg = 0;
			if (x > 0)
			{
				fdeg = 90 - arctan;
			}
			else if (x < 0)
			{
				fdeg = 270 - arctan;
			}
			if (x == 0)
			{
				if (y > 0)
				{
					fdeg = 0;
				}
				else if (y < 0)
				{
					fdeg = 180;
				}
			}
			fdeg -= deg;
			if (fdeg < 0)
			{
				fdeg += 360;
			}
			fdeg = Math.Floor(fdeg);
			return fdeg;
		}

		private string getDir(double facing)
		{
			if (facing >= north && facing < northnortheast)
			{
				return "north";
			}
			if (facing >= northnortheast && facing < northeast)
			{
				return "north-northwest";
			}

			if (facing >= northeast && facing < eastnortheast)
			{
				return "northwest";
			}
			if (facing >= eastnortheast && facing < east)
			{
				return "west-northwest";
			}

			if (facing >= east && facing < eastsoutheast)
			{
				return "west";
			}
			if (facing >= eastsoutheast && facing < southeast)
			{
				return "west-southwest";
			}

			if (facing >= southeast && facing < southsoutheast)
			{
				return "southwest";
			}
			if (facing >= southsoutheast && facing < south)
			{
				return "south-southwest";
			}
			if (facing >= south && facing < southsouthwest)
			{
				return "south";
			}
			if (facing >= southsouthwest && facing < west)
			{
				return "south-southeast";
			}
			if (facing >= southwest && facing < westsouthwest)
			{
				return "southeast";
			}
			if (facing >= westsouthwest && facing < west)
			{
				return "east-southeast";
			}
			if (facing >= west && facing < westnorthwest)
			{
				return "east";
			}
			if (facing >= westnorthwest && facing < northwest)
			{
				return "east-northeast";
			}
			if (facing >= northwest && facing < northnorthwest)
			{
				return "northeast";
			}
			if (facing >= northnorthwest)
			{
				return "north-northeast";
			}
			return "";

		}

		private double fixHeading(double heading)
		{
			double new_heading = 0;

			if (heading <= 180)
			{
				new_heading = heading + 180;
			}
			else if (heading > 180)
			{
				new_heading = heading - 180;
			}

			return new_heading;
		}

		private double GetAngleOfLineBetweenTwoPoints(GTA.Math.Vector3 p1, GTA.Math.Vector3 p2)
		{
			double xDiff = p2.X - p1.X;
			double yDiff = p2.Y - p1.Y;
			return Math.Atan2(yDiff, xDiff) * (180 / Math.PI) + 180;
		}

		private int headingSlice(double heading)
		{

			if (heading >= 0 && heading < 45)
				return 0;
			if (heading >= 45 && heading < 90)
				return 1;
			if (heading >= 90 && heading < 135)
				return 2;
			if (heading >= 135 && heading < 180)
				return 3;
			if (heading >= 180 && heading < 225)
				return 4;
			if (heading >= 225 && heading < 270)
				return 5;
			if (heading >= 270 && heading < 315)
				return 6;
			if (heading >= 315)
				return 7;
			return -1;
		}

		private string headingSliceName(double heading)
		{
			if (headingSlice(heading) == 0)
				return ("north");
			if (headingSlice(heading) == 1)
				return ("northwest");
			if (headingSlice(heading) == 2)
				return ("west");
			if (headingSlice(heading) == 3)
				return ("southwest");
			if (headingSlice(heading) == 4)
				return ("south");
			if (headingSlice(heading) == 5)
				return ("southeast");
			if (headingSlice(heading) == 6)
				return ("east");
			if (headingSlice(heading) == 7)
				return ("northeast");
			return "None";

		}

		public string listToString(List<Result> results, string prependedText = "")
		{
			string text = prependedText;
			string vertical = "";

			Result[] r = results.ToArray();
			Array.Sort(r);
			foreach (Result i in r)
			{
				if (i.zDistance != 0)
				{
					if (i.zDistance > 0)
					{
						vertical = " " + Math.Abs(i.zDistance) + " meters above , ";
					}
					else
					{
						vertical = " " + Math.Abs(i.zDistance) + " meters below, ";
					}


				}
				text = text + i.xyDistance + " meters " + i.direction + ", " + vertical + i.name + ". ";
			}
			return (text);

		}

		private void speakMenu()
		{
			string result = mainMenu[mainMenuIndex];
			if (mainMenuIndex == 0)
				result = result + locations[locationMenuIndex].name;
						if (mainMenuIndex == 1)
				result = result + spawns[spawnMenuIndex].name;
			if (mainMenuIndex == 2)
				result = result + funMenu[funMenuIndex];
			if (mainMenuIndex == 3)
			{
				string toggle = "";
				if (settingsMenu[settingsMenuIndex].value == 0)
					toggle = "Off";
				if (settingsMenu[settingsMenuIndex].value == 1)
					toggle = "On";
				result = result + settingsMenu[settingsMenuIndex].displayName + toggle;
			}

			Tolk.Speak(result, true);

		}

void setupSettings()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			string json;
			string[] ids = {"announceHeadings", "announceZones", "announceTime", "altitudeIndicator", "targetPitchIndicator", "radioOff", "warpInsideVehicle", "onscreen", "speed", "godMode", "policeIgnore", "vehicleSpeed", "vehicleGodMode", "infiniteAmmo", "neverWanted", "superJump", "runFaster", "swimFaster", "exsplosiveAmmo", "fireAmmo", "explosiveMelee"};
			System.IO.StreamWriter fileOut;

			if (!System.IO.Directory.Exists(@Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Rockstar Games/GTA V/ModSettings"))
				System.IO.Directory.CreateDirectory(@Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Rockstar Games/GTA V/ModSettings");

				if (!System.IO.File.Exists(@Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Rockstar Games/GTA V/ModSettings/gta11ySettings.json"))
				{
				fileOut = new System.IO.StreamWriter(@Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Rockstar Games/GTA V/ModSettings/gta11ySettings.json");

				foreach (string i in ids)
				{
					if (i == "announceHeadings" || i == "announceZones" || i == "altitudeIndicator" || i == "announceTime")
					{
						dictionary.Add(i, 1);
					}
					else
					{
						dictionary.Add(i, 0);
					}

				}

				json = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
				fileOut.Write(json);
				fileOut.Close();
			}
			dictionary.Clear();
			System.IO.StreamReader fileIn = new System.IO.StreamReader(@Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Rockstar Games/GTA V/ModSettings/gta11ySettings.json");
			json = fileIn.ReadToEnd();
			fileIn.Close();
			try
			{
				dictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
			}
catch(Exception e)
			{
				System.IO.File.Delete(@Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Rockstar Games/GTA V/ModSettings/gta11ySettings.json");
				setupSettings();
			}
			string current = "";
			try
			{
				settingsMenu.Clear();
				foreach (string i in ids)
				{
					if (i != "vehicleSpeed")
					{
						if (dictionary.ContainsKey(i))
						{
							settingsMenu.Add(new Setting(i, idToName(i), dictionary[i]));
						}
						else
						{
							if (i == "announceHeadings" || i == "announceZones" || i == "altitudeIndicator" || i == "announceTime" || i == "targetPitchIndicator" || i == "speed")
							{
								settingsMenu.Add(new Setting(i, idToName(i), 1));
							}
							else
							{
								settingsMenu.Add(new Setting(i, idToName(i), 0));
							}

						}
					}
				}


			}
			catch(Exception e)
			{
				System.IO.File.Delete(@Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Rockstar Games/GTA V/ModSettings/gta11ySettings.json");
								setupSettings();
			}

		}

		void saveSettings()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			foreach (Setting i in settingsMenu)
			{
				dictionary.Add(i.id, i.value);
			}
						if (!System.IO.Directory.Exists(@Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Rockstar Games/GTA V/ModSettings/"))
				System.IO.Directory.CreateDirectory(@Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Rockstar Games / GTA V / ModSettings/");
				System.IO.StreamWriter  fileOut = new System.IO.StreamWriter(@Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Rockstar Games/GTA V/ModSettings/gta11ySettings.json");
				string result = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
			fileOut.Write(result);
			fileOut.Close();
		}

			string idToName(string id)
		{
			string result = "None";

			if (id == "godMode")
			result = "God Mode. ;";
			if (id == "radioOff")
				result = "Always Disable vehicle radios. ";
			if (id == "warpInsideVehicle")
				result = "Teleport player inside newly spawned vehicles. ";
			if (id == "onscreen")
				result = "Announce only visible nearby items. ";
			if (id == "speed")
				result = "Announce current vehicle speed. ";

			if (id == "policeIgnore")
				result = "Police Ignore Player. ";
			if (id == "vehicleGodMode")
				result = "Make Current vehicle indestructable. ";
						if (id == "altitudeIndicator")
				result = "audible Altitude Indicator. ";
			if (id == "targetPitchIndicator")
				result = "audible Targetting Pitch Indicator. ";
						if (id == "infiniteAmmo")
				result = "Unlimitted Ammo. ";
			if (id == "neverWanted")
				result = "Wanted Level Never Increases. ";
			if (id == "superJump")
				result = "Super Jump. ";
			if (id == "runFaster")
				result = "Run Faster. ";
			if (id == "swimFaster")
				result = "Fast Swimming. ";
			if (id == "exsplosiveAmmo")
				result = "Explosive Ammo. ";
			if (id == "fireAmmo")
				result = "Fire Ammo. ";
			if (id == "explosiveMelee")
				result = "Explosive Melee. ";
			if (id == "announceTime")
				result = "Time of Day Announcements. ";
			if (id == "announceHeadings")
				result = "Heading Change Announcements. ";
			if (id == "announceZones")
				result = "Street and Zone Change Announcements. ";

			//if (id == )
			return result;


		}

int getSetting(string id)
		{
			int result = -1;
			for (int i = 0; i < settingsMenu.Count; i ++)
			{
				if (settingsMenu[i].id == id)
					result = settingsMenu[i].value;
			}
			return result;
		}

	}
	}
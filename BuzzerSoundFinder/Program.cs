// See https://aka.ms/new-console-template for more information
using System.Media;
using System.Reflection;

//SoundPlayer soundPlayer = new SoundPlayer();

//// getting root path
//string rootLocation = typeof(Program).Assembly.Location;
//// appending sound location
//string fullPathToSound = Path.Combine(rootLocation, @"Buzzer-Sound.wav");
//soundPlayer.SoundLocation = fullPathToSound;


//soundPlayer.Play();


string x = (Assembly.GetEntryAssembly().Location + "");
Console.WriteLine(x);

x = x.Replace("bin\\Release\\net6.0\\BuzzerSoundFinder.dll", "Buzzer-Sound.wav");
Console.WriteLine(x);

SoundPlayer player1 = new SoundPlayer(x);
player1.PlaySync();

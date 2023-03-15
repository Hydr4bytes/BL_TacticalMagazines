using MelonLoader;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(BL_TacticalMagazines.BuildInfo.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(BL_TacticalMagazines.BuildInfo.Company)]
[assembly: AssemblyProduct(BL_TacticalMagazines.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + BL_TacticalMagazines.BuildInfo.Author)]
[assembly: AssemblyTrademark(BL_TacticalMagazines.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: Guid("")]
[assembly: AssemblyVersion(BL_TacticalMagazines.BuildInfo.Version)]
[assembly: AssemblyFileVersion(BL_TacticalMagazines.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonInfo(typeof(BL_TacticalMagazines.BL_TacticalMagazines), BL_TacticalMagazines.BuildInfo.Name, BL_TacticalMagazines.BuildInfo.Version, BL_TacticalMagazines.BuildInfo.Author, BL_TacticalMagazines.BuildInfo.DownloadLink)]


// Create and Setup a MelonModGame to mark a Mod as Universal or Compatible with specific Games.
// If no MelonModGameAttribute is found or any of the Values for any MelonModGame on the Mod is null or empty it will be assumed the Mod is Universal.
// Values for MelonModGame can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("", "")]
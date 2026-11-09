#ifndef MyAppVersion
  #define MyAppVersion "1.0.0"
#endif
#ifndef MyFramework
  #define MyFramework "net8.0-windows"
#endif
#ifndef PublishDir
  #define PublishDir "..\artifacts\publish\net8.0-windows-win-x64"
#endif
#ifndef OutputDir
  #define OutputDir "..\artifacts"
#endif

[Setup]
AppId={{A8F87D12-8C39-4F7D-9D35-8B71F3B84C21}
AppName=TVisionUpperDemp
AppVersion={#MyAppVersion}
AppPublisher=yiyang1314
DefaultDirName={autopf}\TVisionUpperDemp
DefaultGroupName=TVisionUpperDemp
UninstallDisplayIcon={app}\TVisionUpperDemp.exe
OutputDir={#OutputDir}
OutputBaseFilename=TVisionUpperDemp-Setup-{#MyFramework}-win-x64
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
ArchitecturesInstallIn64BitMode=x64
PrivilegesRequired=admin
DisableProgramGroupPage=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "chinesesimp"; MessagesFile: "compiler:Languages\ChineseSimplified.isl"

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional shortcuts:"; Flags: unchecked

[Files]
Source: "{#PublishDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\TVisionUpperDemp"; Filename: "{app}\TVisionUpperDemp.exe"
Name: "{autodesktop}\TVisionUpperDemp"; Filename: "{app}\TVisionUpperDemp.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\TVisionUpperDemp.exe"; Description: "Launch TVisionUpperDemp"; Flags: postinstall nowait skipifsilent

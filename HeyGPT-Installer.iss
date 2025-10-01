; HeyGPT Installer Script for Inno Setup
; Creates a Windows installer with full uninstall support

#define MyAppName "HeyGPT"
#define MyAppVersion "1.1.0"
#define MyAppPublisher "Aarav Sai Anugula"
#define MyAppURL "https://github.com/aaravsaianugula/HeyGPT"
#define MyAppExeName "HeyGPT.exe"
#define MyAppDescription "Voice-Activated ChatGPT Assistant with Porcupine AI"

[Setup]
AppId={{E8F3D4A2-8B7C-4E9F-A1D3-6C5B8E9F2A4D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}/issues
AppUpdatesURL={#MyAppURL}/releases
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=LICENSE
OutputDir=Release
OutputBaseFilename=HeyGPT-v{#MyAppVersion}-Setup
SetupIconFile=Assets\SqaureLog.ico
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
UninstallDisplayIcon={app}\{#MyAppExeName}
UninstallDisplayName={#MyAppName} - Voice-Activated ChatGPT Assistant
VersionInfoVersion={#MyAppVersion}
VersionInfoDescription={#MyAppDescription}
ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "startupicon"; Description: "Launch {#MyAppName} at Windows startup"; GroupDescription: "Startup Options:"; Flags: unchecked

[Files]
Source: "ChatGptVoiceAssistant\bin\Release\net8.0-windows10.0.19041.0\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "Release\HeyGPT-v1.1.0-Windows\README.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "Release\HeyGPT-v1.1.0-Windows\RELEASE_NOTES.txt"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{group}\Release Notes"; Filename: "{app}\RELEASE_NOTES.txt"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Registry]
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "{#MyAppName}"; ValueData: """{app}\{#MyAppExeName}"""; Flags: uninsdeletevalue; Tasks: startupicon

[Code]
function InitializeSetup(): Boolean;
var
  ErrorCode: Integer;
  DotNetInstalled: Boolean;
begin
  Result := True;
  DotNetInstalled := RegKeyExists(HKLM, 'SOFTWARE\dotnet\Setup\InstalledVersions\x64\sharedhost') or
                     RegKeyExists(HKLM, 'SOFTWARE\dotnet\Setup\InstalledVersions\x86\sharedhost');

  if not DotNetInstalled then
  begin
    if MsgBox('.NET 8.0 Runtime is required but not installed.' + #13#10 + #13#10 +
              'Would you like to download it now?' + #13#10 + #13#10 +
              'Click Yes to open the download page, or No to cancel installation.',
              mbConfirmation, MB_YESNO) = IDYES then
    begin
      ShellExec('', 'https://dotnet.microsoft.com/download/dotnet/8.0', '', '', SW_SHOWNORMAL, ewNoWait, ErrorCode);
    end;
    Result := False;
  end;
end;

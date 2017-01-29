# PuTTYStorm
Lightweight tabbed window manager for putty sessions

### Features
 - Open multiple sessions with one click
 - Draggable TabControl
 - Fast session tabs management (right click on session tab for quick close)
 - Group sessions to custom defined groups for easy sessions password management
 - Secure Login (hashed SHA256) and encrypted sesssions passwords (secured AES string encryption with random IV)
 - Search already saved sessions
 - Vertical split screen (Activate - CTRL + F1 | Deactivate - CTRL + F2)
 - SFTP Manager with fast access (Activate CTRL + F3). It connects to the currently selected tab (session) and uploads/downloads files to/from user's home directory
 - Passwordless login using PPK and OpenSSH private/public keys (Note: currently SFTP Manager supports only OpenSSH private/public keys, so there is a need to have both PPK - for putty.exe and OpenSSH - for SFTP manager keys added. Will be changed in future).
 
### ToDo
 - Global HotKeys Manager
 - Security improvements (encryption) NOTE: This is an early test version - use at your own risk!
 
### License
Licensed under a liberal MIT/X11 License, which allows this program and source code to be used in both commercial and non-commercial applications. Complete text can be found in the License.txt file.

### System Requirements
  * A PC running Windows (Windows 7, Windows 8, Windows 10)
  * The Microsoft .NET Framework 4.0 or newer
  * 32 and 64 bit operating systems are supported
  * The [PuTTY](http://www.chiark.greenend.org.uk/~sgtatham/putty/) SSH Client
  
### Latest Releases
Available for download at https://github.com/kasbst/puttystorm/releases/latest

### Preview
![Alt text](/img/LoginForm.png?raw=true "Login Form")

![Alt text](/img/MainForm.png?raw=true "Main Form")

![Alt text](/img/AdvancedForm.png?raw=true "Advanced Form")

![Alt text](/img/SessionsForm.png?raw=true "Sessions Form")

![Alt text](/img/SplitScreen.png?raw=true "Split Screen")

![Alt text](/img/ManageSessions.png?raw=true "Manage Sessions")

![Alt text](/img/SFTPManager.png?raw=true "SFTP Manager")

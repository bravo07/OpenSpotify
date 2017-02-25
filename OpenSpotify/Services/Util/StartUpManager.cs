using System;
using System.Reflection;
using System.Security.Principal;
using Microsoft.Win32;

namespace OpenSpotify.Services.Util {

    public class StartUpManager {

        public static void AddApplicationToCurrentUserStartup() {
            using (var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
                key?.SetValue("OpenSpotify", "\"" + Assembly.GetExecutingAssembly().Location + "\"");
            }
        }

        public static void RemoveApplicationFromCurrentUserStartup() {
            using (var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
                key?.DeleteValue("OpenSpotify", false);
            }
        }

        public static bool IsUserAdministrator() {
            bool isAdmin;
            try {
                var user = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex) {
                isAdmin = false;
            }
            catch (Exception ex) {
                isAdmin = false;
            }
            return isAdmin;
        }
    }
}
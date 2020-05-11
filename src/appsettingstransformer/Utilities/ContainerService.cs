using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;

namespace appsettingstransformer
{
    public static class ContainerService
    {
        public static void createLocalAdmin(string userName, string password)
        {
            try
            {
                Console.WriteLine("creating user account {0}", userName);

                DirectoryEntry AD = new DirectoryEntry("WinNT://" +
                                    Environment.MachineName + ",computer");
                DirectoryEntry NewUser = AD.Children.Add(userName, "user");
                NewUser.Invoke("SetPassword", new object[] { password });
                NewUser.Invoke("Put", new object[] { "Description", "Container Admin" });
                NewUser.CommitChanges();
                DirectoryEntry grp;

                grp = AD.Children.Find("Administrators", "group");
                if (grp != null) { grp.Invoke("Add", new object[] { NewUser.Path.ToString() }); }
                Console.WriteLine("Account Created Successfully");                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

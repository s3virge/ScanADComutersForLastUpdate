using System;
using System.Collections;
using System.DirectoryServices;

class ActiveDirectory {
    /// <summary>
    /// receive from AD list of computers with given name
    /// </summary>
    /// <param name="comuter Name"></param>
    /// <returns>array list whith computers names</returns>
    public static ArrayList GetListOfComputers(string comuterName = null) {
        DirectorySearcher mySearcher = new DirectorySearcher();
        ArrayList compList = new ArrayList();

        if (comuterName == null || comuterName == "") {
            mySearcher.Filter = $"( &(objectClass=computer))";
        }
        else {
            mySearcher.Filter = $"( &(objectClass=computer)(cn=*{comuterName}*))";
        }

        mySearcher.SizeLimit = int.MaxValue;
        mySearcher.PageSize = int.MaxValue;

        foreach (SearchResult resEnt in mySearcher.FindAll()) {
            DirectoryEntry directoryEntry = resEnt.GetDirectoryEntry();

            if (IsActive(directoryEntry)) {
                string ComputerName = directoryEntry.Name;

                if (ComputerName.StartsWith("CN=")) {
                    ComputerName = ComputerName.Remove(0, "CN=".Length);
                }

                compList.Add(ComputerName);
            }
        }

        mySearcher.Dispose();

        return compList;
    }

    /// <summary>
    /// check if derectory entry is disabled
    /// </summary>
    /// <param name="dirEntry"></param>
    /// <returns></returns>
    private static bool IsActive(DirectoryEntry dirEntry) {
        if (dirEntry.NativeGuid == null)
            return false;

        int flags = (int)dirEntry.Properties["userAccountControl"].Value;

        return !Convert.ToBoolean(flags & 0x0002);
    }

    public static string GetCanonicalName(string computer) {
       string CanonicalName = "";

        //DirectoryEntry entry = new DirectoryEntry("LDAP://" + _domainOU);
        //Инициализирует новый экземпляр класса DirectorySearcher с указанным корнем и фильтром поиска, а также с указанием извлекаемых свойств.
        string searchfilter = $"(&(objectCategory=computer)(Name={computer}))";
        //string searchfilter = "(&(objectClass=computer))";
        DirectorySearcher mySearcher = new DirectorySearcher(searchfilter, new string[] { "CanonicalName" });

        SearchResult resEnt = mySearcher.FindOne();

        if (resEnt != null) {
            CanonicalName = resEnt.Properties["CanonicalName"][0].ToString();
        }

        return CanonicalName;
    }
}


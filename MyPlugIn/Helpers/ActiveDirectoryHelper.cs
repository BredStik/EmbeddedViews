using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using MyPlugIn.Models;

namespace MyPlugIn.Helpers
{
    public class ActiveDirectoryHelper
    {
        public const string AD_SECURITY_ID = "objectsid";
        public const string AD_NAME = "cn";
        public const string AD_LOGIN = "sAMAccountName";
        public const string AD_EMAIL = "mail";
        public const string AD_TELEPHONE = "telephoneNumber";
        public const string AD_FIRST_NAME = "givenName";
        public const string AD_LAST_NAME = "sn";
        public const string AD_PRIMARY_USER_ADDRESS = "msRTCSIP-PrimaryUserAddress";
        public const string AD_USER_ACCOUNT_CONTROL = "useraccountcontrol";

        protected const string AD_ACTIVE_USER_FILTER = "(!(userAccountControl:1.2.840.113556.1.4.803:=2))";
        public static Dictionary<string, string> GetUserInfo(string loginName)
        {
            if (string.IsNullOrEmpty(loginName))
            {
                return null;
            }

            if (loginName.IndexOf("\\") != -1)
            {
                loginName = loginName.Substring(loginName.IndexOf("\\") + 1);
            }

            var userInfo = new Dictionary<string, string>();

            using (var directoryRoot = new DirectoryEntry(ConfigurationManager.AppSettings["ADPath"]))
            {
                using (var directorySearcher = new DirectorySearcher(directoryRoot)
                                                   {
                                                       Filter =
                                                           "(&(objectClass=user)(" + AD_LOGIN + "=" + loginName + "))",
                                                       SearchScope = SearchScope.Subtree,
                                                       PageSize = 1
                                                   })
                {
                    var searchResult = directorySearcher.FindOne();

                    if (searchResult == null) return null;

                    var properties = new[]
                                         {
                                             AD_SECURITY_ID, AD_NAME, AD_LOGIN, AD_EMAIL, AD_TELEPHONE, AD_FIRST_NAME,
                                             AD_LAST_NAME, AD_PRIMARY_USER_ADDRESS, AD_USER_ACCOUNT_CONTROL
                                         };

                    foreach (var property in properties)
                    {
                        userInfo.Add(property,
                                     searchResult.Properties[property].Count > 0
                                         ? searchResult.Properties[property][0].ToString()
                                         : string.Empty);
                    }

                }
            }


            return userInfo;
        }

        public static string GetUserEmail(string loginName)
        {
            if (string.IsNullOrEmpty(loginName))
            {
                return null;
            }

            if (loginName.IndexOf("\\") != -1)
            {
                loginName = loginName.Substring(loginName.IndexOf("\\") + 1);
            }

            using (var directoryRoot = new DirectoryEntry(ConfigurationManager.AppSettings["ADPath"]))
            {
                using (var directorySearcher = new DirectorySearcher(directoryRoot)
                                                   {
                                                       Filter =
                                                           "(&(objectClass=user)(" + AD_LOGIN + "=" + loginName + ")" + AD_ACTIVE_USER_FILTER + ")",
                                                       SearchScope = SearchScope.Subtree,
                                                       PageSize = 1000
                                                   })
                {
                    directorySearcher.PropertiesToLoad.Add(AD_EMAIL);

                    SearchResult searchResult = directorySearcher.FindOne();

                    if (searchResult == null)
                    {
                        return null;
                    }

                    return searchResult.Properties[AD_EMAIL].Count > 0
                               ? searchResult.Properties[AD_EMAIL][0].ToString()
                               : null;
                }
            }
        }

        public static string[] GetUserEmails(params string[] logins)
        {
            var trimmedLogins = logins.Select(x => x.IndexOf("\\") != -1 ? x.Substring(x.IndexOf("\\") + 1) : x);
            
            if(trimmedLogins.Count() == 0) return new string[]{};

            string[] emails;

            using (var directoryRoot = new DirectoryEntry(ConfigurationManager.AppSettings["ADPath"]))
            {
                using (var directorySearcher = new DirectorySearcher(directoryRoot)
                                                   {
                                                       SearchScope = SearchScope.Subtree,
                                                       PageSize = 50
                                                   })
                {
                    var sb = new StringBuilder("(&(objectClass=user)");
                    sb.Append(AD_ACTIVE_USER_FILTER);
                    
                    if (trimmedLogins.Count() > 1)
                    {
                        sb.Append("(|");
                    }

                    foreach (var trimmedLogin in trimmedLogins)
                    {
                        sb.AppendFormat("({0}={1})", AD_LOGIN, trimmedLogin);
                    }

                    if (trimmedLogins.Count() > 1)
                    {
                        sb.Append(")");
                    }

                    sb.Append(")");

                    directorySearcher.Filter = sb.ToString();

                    directorySearcher.PropertiesToLoad.Add(AD_EMAIL);

                    using (var results = directorySearcher.FindAll())
                    {
                        emails =
                            results.Cast<SearchResult>().Where(x => x.Properties[AD_EMAIL].Count > 0).Select(
                                x => x.Properties[AD_EMAIL][0].ToString()).ToArray();
                    }

                }
            }

            return emails;
        }


        public static IList<PeoplePickerSearchResultViewModel> FindUsers(string filter)
        {
            filter = filter.Trim();
            filter = Regex.Replace(filter, "\\s+", " ");
            
            if (string.IsNullOrEmpty(filter))
            {
                return new List<PeoplePickerSearchResultViewModel>();
            }

            IList<PeoplePickerSearchResultViewModel> users = new List<PeoplePickerSearchResultViewModel>();

            using (var directoryRoot = new DirectoryEntry(ConfigurationManager.AppSettings["ADPath"]))
            {



                var strSubFilter = "";
                if (filter.Contains(','))
                {
                    var filterParts = filter.Split(',');
                    var lastnameFilter = filterParts[0].Trim();
                    var firstnameFilter = filter.Substring(filter.IndexOf(',') + 1).Trim();
                    strSubFilter =
                        string.Format("(&(givenName={0}*)(sn={1}*))(&(givenName={1}*)(sn={0}*))", firstnameFilter,
                                      lastnameFilter);
                }
                else if (filter.Contains(' '))
                {

                    var filterParts = filter.Split(' ');
                    if (filterParts.Length == 2)
                    {
                        var firstnameFilter = filterParts[0].Trim();
                        var lastnameFilter = filterParts[1].Trim();
                        strSubFilter =
                            string.Format("(&(givenName={0}*)(sn={1}*))(&(givenName={1}*)(sn={0}*))", firstnameFilter,
                                          lastnameFilter);
                    }
                }

                var strFilter =
                    string.Format(
                        "(&(objectCategory=person)(objectClass=user)(|(cn={0}*)(sAMAccountName={0}*)(givenName={0}*)(sn={0}*){1}))",
                        filter, strSubFilter);

                using (var directorySearcher = new DirectorySearcher(directoryRoot)
                                                   {
                                                       Filter = strFilter,
                                                       SearchScope = SearchScope.Subtree,
                                                       PageSize = 1000,
                                                       Sort =
                                                           new SortOption("cn", SortDirection.Ascending)
                                                   })
                {
                    using (SearchResultCollection searchResults = directorySearcher.FindAll())
                    {
                        foreach (SearchResult searchResult in searchResults)
                        {
                            string name = string.Empty;
                            string login = string.Empty;
                            string email = string.Empty;
                            string telephone = string.Empty;
                            string firstname = string.Empty;
                            string lastname = string.Empty;
                            int userAccountControl = 0;

                            if (searchResult.Properties[AD_NAME].Count > 0)
                            {
                                name = searchResult.Properties[AD_NAME][0].ToString();
                            }

                            if (searchResult.Properties[AD_FIRST_NAME].Count > 0)
                            {
                                firstname = searchResult.Properties[AD_FIRST_NAME][0].ToString();
                            }

                            if (searchResult.Properties[AD_LAST_NAME].Count > 0)
                            {
                                lastname = searchResult.Properties[AD_LAST_NAME][0].ToString();
                            }

                            if (searchResult.Properties[AD_LOGIN].Count > 0)
                            {
                                login = string.Format("SLI\\{0}",
                                                      searchResult.Properties[AD_LOGIN][0].ToString().ToUpper());
                            }

                            if (!login.StartsWith("xx", StringComparison.CurrentCultureIgnoreCase))
                            {
                                if (searchResult.Properties[AD_SECURITY_ID].Count > 0)
                                {
                                    try
                                    {

                                        var sid = new SecurityIdentifier(
                                            (byte[]) searchResult.Properties[AD_SECURITY_ID][0], 0);
                                        var account = (NTAccount) sid.Translate(typeof (NTAccount));
                                        var username = account.ToString().ToUpper();
                                        // This give the DOMAIN\User format for the account 
                                        if (!string.IsNullOrEmpty(username) && username.Contains(login.ToUpper()))
                                        {
                                            login = username;
                                        }
                                    }
                                    catch
                                    {
                                        // do nothing
                                    }
                                }

                                users.Add(new PeoplePickerSearchResultViewModel { FirstName = firstname, LastName = lastname, Login = login });
                            }
                        }
                    }
                }
            }

            return users;
        }
    }
}
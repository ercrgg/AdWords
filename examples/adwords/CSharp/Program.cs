// Copyright 2011, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// Author: api.anash@gmail.com (Anash P. Oommen)

using Google.Api.Ads.AdWords.Lib;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using ExamplePair = System.Collections.Generic.KeyValuePair<string, object>;
using System.Threading;
using Google.Api.Ads.AdWords.v201309;
using Google.Api.Ads.AdWords.Util.Reports;

namespace Google.Api.Ads.AdWords.Examples.CSharp {
  /// <summary>
  /// The Main class for this application.
  /// </summary>
  class Program {
    /// <summary>
    /// A map to hold the code examples to be executed.
    /// </summary>
    static List<ExamplePair> codeExampleMap = new List<ExamplePair>();

    /// <summary>
    /// A flag to keep track of whether help message was shown earlier.
    /// </summary>
    private static bool helpShown = false;
    private static string v201309;

    /// <summary>
    /// Registers the code example.
    /// </summary>
    /// <param name="key">The code example name.</param>
    /// <param name="value">The code example instance.</param>
    static void RegisterCodeExample(string key, object value) {
      codeExampleMap.Add(new ExamplePair(key, value));
    }

    /// <summary>
    /// Static constructor to initialize the sample map.
    /// </summary>
    static Program() {
      Type[] types = Assembly.GetExecutingAssembly().GetTypes();

      foreach (Type type in types) {
        if (type.IsSubclassOf(typeof(ExampleBase))) {
          RegisterCodeExample(type.FullName.Replace(typeof(Program).Namespace + ".", ""),
              Activator.CreateInstance(type));
        }
      }
    }

    /// <summary>
    /// The main method.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public static void Main(string[] args) {
      if (args.Length == 0) {
          AdWordsUser user = new AdWordsUser();
          
          user.OAuthProvider.RefreshAccessTokenIfExpiring();
          
          
          Selector managedSelector = new Selector();
          managedSelector.fields = new string[] { "Name", "CustomerId", "CanManageClients"};
          managedSelector.paging = new Paging();
          managedSelector.paging.startIndex = 0;
          managedSelector.paging.numberResults = 100;

          ManagedCustomerService managedCustomerService = (ManagedCustomerService)user.GetService(AdWordsService.v201309.ManagedCustomerService);          
          ManagedCustomerPage managedPage = new ManagedCustomerPage();
          managedPage = managedCustomerService.get(managedSelector);

          List<Int64> CustomerIDs = new List<Int64>();          
          foreach (ManagedCustomer managedCustomer in managedPage.entries)
          {
              CustomerIDs.Add(managedCustomer.customerId);
          }

          AdWordsUser Customer = new AdWordsUser();
          foreach (Int64 CustomerID in CustomerIDs)
          {
              
          }


          CampaignService campaignService =
              (CampaignService)user.GetService(AdWordsService.v201309.CampaignService);
          Selector selector = new Selector();
          selector.fields = new string[] { "Id", "Name" };
          
          // Set the selector paging.
          selector.paging = new Paging();
          int offset = 0;
          int pageSize = 500;

          CampaignPage page = new CampaignPage();

          try
          {
              do
              {
                  selector.paging.startIndex = offset;
                  selector.paging.numberResults = pageSize;

                  // Get the campaigns.
                  page = campaignService.get(selector);

                  // Display the results.
                  if (page != null && page.entries != null)
                  {
                      int i = offset;
                      foreach (Campaign campaign in page.entries)
                      {
                          Console.WriteLine("{0}) Campaign with id = '{1}', name = '{2}' " +
                            " was found.", i + 1, campaign.id, campaign.name);
                          i++;
                      }
                  }
                  offset += pageSize;
              } while (offset < page.totalNumEntries);
              Console.WriteLine("Number of campaigns found: {0}", page.totalNumEntries);
          }
          catch (Exception ex)
          {
              throw new System.ApplicationException("Failed to retrieve campaigns", ex);
          }





          ReportDefinition definition = new ReportDefinition();
          definition.reportName = "Adgroup Report";
          definition.reportType = ReportDefinitionReportType.KEYWORDS_PERFORMANCE_REPORT;
          definition.downloadFormat = DownloadFormat.CSV;
          definition.dateRangeType = ReportDefinitionDateRangeType.ALL_TIME;
          Selector selector1 = new Selector();
          selector1.fields = new string[]
        {
            "Clicks",
        };
          definition.selector = selector1;
          //if (!Directory.Exists(ReportFolder))
          //    Directory.CreateDirectory(ReportFolder);

          ReportUtilities utilities = new ReportUtilities(user) { ReportVersion = "v201309" };
          Console.WriteLine(utilities.ReportVersion);
          ClientReport report = utilities.DownloadClientReport(definition, "C:\\Bjork\\" + definition.reportName + ".xls");

          foreach (string cmdArgs in args)
          {
              ExamplePair matchingPair = codeExampleMap.Find(delegate(ExamplePair pair)
              {
                  return string.Compare(pair.Key, cmdArgs, true) == 0;
              });

              if (matchingPair.Key != null)
              {
                  RunACodeExample(user, matchingPair.Value);
              }
              else
              {
                  ShowUsage();
              }
          }
          ShowUsage();
          return;
      }

    }

    /// <summary>
    /// Runs a code example.
    /// </summary>
    /// <param name="user">The user whose credentials should be used for
    /// running the code example.</param>
    /// <param name="codeExample">The code example to run.</param>
    private static void RunACodeExample(AdWordsUser user, object codeExample) {
      try {
        Console.WriteLine(GetDescription(codeExample));
        InvokeRun(codeExample, user);
      } catch (Exception ex) {
        Console.WriteLine("An exception occurred while running this code example. {0}",
            ExampleUtilities.FormatException(ex));
      } finally {
        Console.WriteLine("Press [Enter] to continue");
        Console.ReadLine();
      }
    }

    /// <summary>
    /// Gets the description for the code example.
    /// </summary>
    /// <param name="codeExample">The code example.</param>
    /// <returns>The description.</returns>
    private static object GetDescription(object codeExample) {
      Type exampleType = codeExample.GetType();
      return exampleType.GetProperty("Description").GetValue(codeExample, null);
    }

    /// <summary>
    /// Gets the parameters for running a code example.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="codeExample">The code example.</param>
    /// <returns>The list of parameters.</returns>
    private static object[] GetParameters(AdWordsUser user, object codeExample) {
      MethodInfo methodInfo = codeExample.GetType().GetMethod("Run");
      List<object> parameters = new List<object>();
      parameters.Add(user);
      parameters.AddRange(ExampleUtilities.GetParameters(methodInfo));
      return parameters.ToArray();
    }
      
    /// <summary>
    /// Invokes the run method.
    /// </summary>
    /// <param name="codeExample">The code example.</param>
    /// <param name="user">The user.</param>
    private static void InvokeRun(object codeExample, AdWordsUser user) {
      MethodInfo methodInfo = codeExample.GetType().GetMethod("Run");
      methodInfo.Invoke(codeExample, GetParameters(user, codeExample));
    }

    /// <summary>
    /// Prints program usage message.
    /// </summary>
    private static void ShowUsage() {
      if (helpShown) {
        return;
      } else {
        helpShown = true;
      }
      string exeName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
      Console.WriteLine("Runs AdWords API code examples");
/*      Console.WriteLine("Usage : {0} [flags]\n", exeName);
      Console.WriteLine("Available flags\n");
      Console.WriteLine("--help\t\t : Prints this help message.", exeName);
      Console.WriteLine("--all\t\t : Run all code examples.", exeName);
      Console.WriteLine("examplename1 [examplename1 ...] : " +
          "Run specific code examples. Example name can be one of the following:\n", exeName);
      foreach (ExamplePair pair in codeExampleMap) {
        Console.WriteLine("{0} : {1}", pair.Key, GetDescription(pair.Value));
      }
*/
        
      Console.WriteLine("Press [Enter] to continue");
      Console.ReadLine();
    }
  }
}
